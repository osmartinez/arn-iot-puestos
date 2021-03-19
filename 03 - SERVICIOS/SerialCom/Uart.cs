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
        private Timer timerReconexion;
        private bool reconectando = false;
        private bool realizandoLectura = false;

        public bool Conectado
        {
            get
            {
                return this.client != null && client.Connected;
            }
        }


        public Uart(Bancadas b)
        {
            this.maquinas = b.Maquinas.ToList();
            Conectar();
            this.timerReconexion = new Timer();
            this.timerReconexion.Interval = 20 * 1000;
            this.timerReconexion.Elapsed += TimerReconexion_Elapsed;
            this.timerReconexion.Start();
        }

        public void Cerrar()
        {
            if (Conectado)
            {
                this.client.Disconnect();
            }
        }

        private void TimerReconexion_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!reconectando && !Conectado)
            {
                Conectar();
            }
        }

        private void Conectar()
        {
            reconectando = true;
            try
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
                    if (this.timer != null)
                    {
                        this.timer.Stop();
                    }
                    this.timer = new Timer();
                    TimeSpan timespan = new TimeSpan(0, 0, 0, 0, 2000);
                    this.timer.Interval = timespan.TotalMilliseconds;
                    this.timer.Elapsed += Timer_Elapsed;
                    this.timer.Start();
                }
            }
            catch (Exception ex)
            {
                Logs.Log.Write(ex);
            }


            reconectando = false;
        }

        private void PulsoGenerado(Maquinas maquina)
        {
            if (OnPulsoGenerado != null)
            {
                OnPulsoGenerado(null, new PulsoGeneradoEventArgs(maquina.IdBancada.Value));
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (this.Conectado && !realizandoLectura)
                {
                    realizandoLectura = true;
                    foreach (Maquinas maq in this.maquinas)
                    {
                        int valor = this.LeerPulso(maq.MaquinasConfiguracionesPins.DireccionPulso);
                        if (valor > 0)
                        {
                            RestarPulso(maq.MaquinasConfiguracionesPins.DireccionPulso, valor - 1);
                            PulsoGenerado(maq);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                realizandoLectura = false;

            }
            realizandoLectura = false;
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
                 //int[] test = this.client.ReadHoldingRegisters(4096, 100);
                // bool[] inputs = this.client.ReadDiscreteInputs(0, 7);
                int[] resultado = this.client.ReadHoldingRegisters(ObtenerDireccion(direccionPulso), 1);
                int valor = resultado[0];
                return valor;
            }
            catch (Exception ex)
            {
                //Log.Write(ex);
                return 0;
            }

        }

    }
}
