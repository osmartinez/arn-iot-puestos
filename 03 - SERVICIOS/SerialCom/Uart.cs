using EasyModbus;
using Entidades.EntidadesBD;
using Entidades.Eventos;
using Logs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SerialCom
{
    public class Uart
    {
        private const string MANUFACTURER = "wch.cn";
        private const int OFFSET = 4096;

        public event EventHandler<PulsoGeneradoEventArgs> OnPulsoGenerado;

        private List<Maquinas> maquinas = new List<Maquinas>();
        private string COM = "";
        private ModbusClient client;
        private Timer timer;

        public bool Conectado
        {
            get
            {
                return this.client != null && client.Connected;
            }
        }

        public Uart(IEnumerable<Maquinas> maquinas)
        {
            this.maquinas = maquinas.ToList();
            Conectar();
        }

        private void Conectar()
        {
            using (ManagementClass i_Entity = new ManagementClass("Win32_PnPEntity"))
            {
                foreach (ManagementObject i_Inst in i_Entity.GetInstances())
                {
                    Object o_Guid = i_Inst.GetPropertyValue("ClassGuid");
                    if (o_Guid == null || o_Guid.ToString().ToUpper() != "{4D36E978-E325-11CE-BFC1-08002BE10318}")
                        continue; // Skip all devices except device class "PORTS"

                    String s_Caption = i_Inst.GetPropertyValue("Caption").ToString();
                    String s_Manufact = i_Inst.GetPropertyValue("Manufacturer").ToString();
                    String s_DeviceID = i_Inst.GetPropertyValue("PnpDeviceID").ToString();
                    String s_RegPath = "HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Enum\\" + s_DeviceID + "\\Device Parameters";
                    String s_PortName = Registry.GetValue(s_RegPath, "PortName", "").ToString();

                    int s32_Pos = s_Caption.IndexOf(" (COM");
                    if (s32_Pos > 0) // remove COM port from description
                        s_Caption = s_Caption.Substring(0, s32_Pos);

                    if (s_Manufact == MANUFACTURER)
                    {
                        COM = s_PortName;
                    }
                }
            }

            if (!string.IsNullOrEmpty(COM))
            {
                client = new ModbusClient(COM);
                client.Baudrate = 9600;
                client.Parity = Parity.Even;
                client.StopBits = StopBits.One;
                client.Connect();
            }

            if (this.Conectado)
            {
                this.timer = new Timer();
                TimeSpan timespan = new TimeSpan(0, 0, 0, 0, 200);
                this.timer.Interval = timespan.TotalMilliseconds;
                this.timer.Elapsed += Timer_Elapsed;
                this.timer.Start();
            }
        }

        private void PulsoGenerado(Maquinas maquina)
        {
            if (OnPulsoGenerado != null)
            {
                OnPulsoGenerado(null, new PulsoGeneradoEventArgs(maquina));
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (this.Conectado)
                {
                    foreach(Maquinas maq in this.maquinas)
                    {
                        int valor = this.LeerPulso(maq.MaquinasConfiguracionesPins.DireccionPulso);
                        if (valor > 0)
                        {
                            RestarPulso(maq.MaquinasConfiguracionesPins.DireccionPulso, valor - 1);
                            PulsoGenerado(maq);
                        }
                    }
                    
                }
                else
                {
                    this.Conectar();
                }
            }catch(Exception ex)
            {
                Log.Write(ex);
            }
        }

        private int ObtenerDireccion(int direccion)
        {
            return direccion + OFFSET;
        }

        private void RestarPulso(int direccion, int valor)
        {
            try
            {
                this.client.WriteSingleRegister(ObtenerDireccion(direccion), valor);

            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        private int LeerPulso(int direccionPulso)
        {
            try
            {
                int[] resultado = this.client.ReadHoldingRegisters(ObtenerDireccion(direccionPulso), 1);
                int valor = resultado[0];
                return valor;
            }
            catch(Exception ex)
            {
                Log.Write(ex);
                return 0;
            }

        }

    }
}
