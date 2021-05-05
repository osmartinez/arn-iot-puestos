using ArnGestionPuestoFrontendWPF.Ventanas;
using BDSQL;
using DatosConfiguracion;
using Entidades.EntidadesBD;
using Entidades.EntidadesConfiguracion;
using Entidades.EntidadesDTO;
using Entidades.Enum;
using Entidades.Eventos;
using FichajesLector;
using Logs;
using MQTT;
using Newtonsoft.Json;
using SerialCom;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Turnos;

namespace ArnGestionPuestoFrontendWPF
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public string NombrePuesto
        {
            get
            {
                if (Store.Bancada != null)
                    return Store.Bancada.Nombre;
                else
                    return "<SIN PUESTO>";
            }
        }
        private Configuracion config;
        private string etiqueta = "";
        private FichajeLectorServicio fichajes = new FichajeLectorServicio();
        private Uart uart;
        public event PropertyChangedEventHandler PropertyChanged;
        private DispatcherTimer timerInactividad;
        private Random rnd = new Random();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            try
            {
                if (!System.Security.Principal.WindowsIdentity.GetCurrent().Name.Contains("omartinez"))
                {
                    KillExplorer();
                    this.WindowStyle = WindowStyle.None;

                }
                this.timerInactividad = new DispatcherTimer();
                this.timerInactividad.Interval = new TimeSpan(0, 30, 0);
                this.timerInactividad.Tick += TimerInactividad_Tick;

                this.PreviewKeyUp += MainWindow_PreviewKeyUp;
                this.fichajes.OnBarquillaFichada += Fichajes_OnBarquillaFichada;
                this.fichajes.OnOperacionFichada += Fichajes_OnOperacionFichada;
                NavegacionEventos.OnNuevaPagina += NavegacionEventos_OnNuevaPagina;
                NavegacionEventos.CargarNuevaPagina(NavegacionEventos.PaginaOperarios);
                CargarConfiguracion();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }

        }

        private void TimerInactividad_Tick(object sender, EventArgs e)
        {
            this.timerInactividad.Stop();
        }

        private void CargarConfiguracion()
        {
            Cargando loading = new Cargando();
            if (!System.Security.Principal.WindowsIdentity.GetCurrent().Name.Contains("omartinez"))
            {
                loading.Topmost = true;
                loading.WindowStyle = WindowStyle.None;

            }
            loading.Show();

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (s, e) =>
            {
                try
                {
                    this.config = BDConfiguracion.Leer();
                    Store.Bancada = BDSQL.Select.ObtenerBancadaPorId(this.config.IdBancada);
                    if (Store.Bancada != null && Store.Bancada.EsMaster && Store.Bancada.IdHermano.HasValue)
                    {
                        Store.BancadaEsclavo = BDSQL.Select.ObtenerBancadaPorId(Store.Bancada.IdHermano.Value);
                    }

                    DateTime ahora = DateTime.Now;
                    Turno turno = Horario.CalcularTurnoAFecha(ahora);
                    DateTime fechaInicio;
                    DateTime fechaFin;
                    Horario.CalcularHorarioTurno(turno, ahora, out fechaInicio, out fechaFin);

                    var paquetes = new List<MaquinasRegistrosDatos>();
                    foreach(var maquina in Store.Bancada.Maquinas)
                    {
                        paquetes.AddRange(Select.HistoricoPaquetesOperario(maquina.IpAutomata,maquina.Posicion,fechaInicio, fechaFin));

                    }

                    foreach (var paquete in paquetes.Where(x => x.PiezaIntroducida))
                    {
                        var maq = Store.Bancada.Maquinas.FirstOrDefault(x => x.IpAutomata == paquete.IpAutomata && x.Posicion == paquete.PosicionMaquina);
                        if (maq != null)
                        {
                            maq.Pulsos.Add(new PulsoMaquina
                            {
                                IdTarea = paquete.IdTarea,
                                CodigoEtiqueta = paquete.CodigoEtiqueta,
                                Control = BuscarControl(paquete.IdOperacion, maq),
                                Fecha = paquete.FechaCreacion,
                                Pares = paquete.Pares,
                                IdOperario = paquete.IdOperario,
                            }); ;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                }
            };

            bw.RunWorkerCompleted += (s, e) =>
            {
                try
                {
                    if (Store.Bancada != null && Store.Bancada.Maquinas.Any())
                    {
                        if (Store.Bancada.EsMaster)
                        {
                            var maquinasUart = Store.Bancada.Maquinas.ToList();
                            if (Store.BancadaEsclavo != null)
                            {
                                maquinasUart.AddRange(Store.BancadaEsclavo.Maquinas.ToList());
                            }
                            uart = new Uart(maquinasUart);
                            uart.OnPulsoGenerado += this.Uart_OnPulsoGenerado;
                        }

                        this.IniciarMQTT();
                    }
                    else
                    {
                        MessageBox.Show("Bancada no configurada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    loading.Close();
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    loading.Close();
                }
                Notifica();
            };

            bw.RunWorkerAsync();
        }

        private void IniciarMQTT()
        {
            ClienteMQTT.Topics.Add(new Topic(1, "/puesto/+/asociarTarea"
                , new Regex(@"^\/puesto\/\s?[0-9]+\/asociarTarea$"), 3, 1, qos: 1));
            ClienteMQTT.Topics.Add(new Topic(2, "/puesto/+/normal"
                , new Regex(@"^\/puesto\/\s?[0-9]+\/normal$"), 3, 1, qos: 2));
            ClienteMQTT.Topics.Add(new Topic(3, "/puesto/+/preparar"
                , new Regex(@"^\/puesto\/\s?[0-9]+\/preparar"), 3, 1, qos: 1));
            // si no es master me tengo que suscribir al pulso interno de mi master /puesto/mi_id/pulsoInterno
            if (!Store.Bancada.EsMaster)
            {
                ClienteMQTT.Topics.Add(new Topic(4, string.Format("/puesto/{0}/pulsoInterno"
                    , Store.Bancada.ID), new Regex(@"^\/puesto\/\s?[0-9]+\/pulsoInterno"), 3, 1, qos: 2));
                ClienteMQTT.Topics[3].Callbacks.Add(PulsoInterno);
            }
            ClienteMQTT.Topics[1].Callbacks.Add(Normal);

            ClienteMQTT.Iniciar(Store.Bancada.Nombre);
        }

        private void PulsoInterno(string msg, string topicRecibido, Topic topic)
        {
            PulsoGenerado(new PulsoGeneradoEventArgs(Store.Bancada.ID));
        }

        /// <summary>
        /// Callback que se debe ejecutar cuando otro puesto genera un pulso de una tarea que 
        /// estoy haciendo yo también
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="topicRecibido"></param>
        /// <param name="topic"></param>
        private void Normal(string msg, string topicRecibido, Topic topic)
        {
            try
            {
                MensajeConsumoTarea consumo = JsonConvert.DeserializeObject<MensajeConsumoTarea>(msg);
                if (consumo == null)
                {
                    Log.Write(new Exception("Consumo recibido nulo " + msg));
                }
                // si viene de otro puesto y es una tarea que me afecta
                if (consumo.IdPuesto != Store.Bancada.ID)
                {

                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        private void Notifica(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AsignarTareaEjecucion(List<SP_BarquillaBuscarInformacionEnSeccion_Result> infoBarquillaSeccion, Maquinas maquina,string codigoEtiqueta)
        {
            if (infoBarquillaSeccion.Any())
            {
                var idsOrden = infoBarquillaSeccion.Select(x => x.IdOrden);
                var idsOrdenDistinto = idsOrden.Distinct();
                if (idsOrden.Count() != idsOrdenDistinto.Count())
                {
                    // multiOperacion
                }
                else
                {
                    var idsTareas = infoBarquillaSeccion.Select(x => x.IdTarea.Value).Distinct().ToList();

                    var control = BuscarControl(infoBarquillaSeccion.First().IdOperacion, maquina);

                    // bd
                    BackgroundWorker bwActualizarCola = new BackgroundWorker();
                    List<MaquinasColasTrabajo> cola = new List<MaquinasColasTrabajo>();
                    bwActualizarCola.DoWork += (se, ev) =>
                    {
                        cola = Insert.ActualizarColaTrabajo(codigoEtiqueta, idsTareas, infoBarquillaSeccion.First().Agrupacion ?? 0, maquina.ID, Store.OperarioEjecucion.Id, infoBarquillaSeccion.Sum(x => x.Cantidad));
                    };
                    bwActualizarCola.RunWorkerCompleted += (se, ev) =>
                    {
                        maquina.AsignarColaTrabajo(cola);
                        BusEventos.TareasCargadas();
                    };
                    bwActualizarCola.RunWorkerAsync();

                    // mqtt
                    MqttAsociarBarquilla(infoBarquillaSeccion, maquina);
                }
            }

        }
        private void Fichajes_OnBarquillaFichada(object sender, BarquillaFichadaEventArgs e)
        {
            try
            {
                foreach (var maquina in Store.Bancada.Maquinas)
                {
                    var infoBarquillaSeccion = Select.BuscarTareasPorCodigoBarquilla(e.CodigoEtiqueta, maquina.CodSeccion);

                    this.AsignarTareaEjecucion(infoBarquillaSeccion, maquina, e.CodigoEtiqueta);
                }

            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }
        private void MqttAsociarBarquilla(List<SP_BarquillaBuscarInformacionEnSeccion_Result> prepaquete, Maquinas maquina, bool asociacion = true)
        {
            try
            {
                string nombreCliente = prepaquete.First().NOMBRECLI ?? "ARNEPLANT S.L.";
                nombreCliente = new Regex("[^A-Za-z0-9 ]").Replace(nombreCliente, " ");
                if (nombreCliente.Length > 25)
                {
                    nombreCliente = nombreCliente.Substring(0, 24);
                }

                string mensaje = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};",
                    maquina.Posicion
                    , prepaquete.First().IdTarea.ToString().PadLeft(10, '0')
                    , prepaquete.First().CantidadFabricar.ToString().PadLeft(10, '0')
                    , prepaquete.First().Codigo.PadLeft(13)
                    , prepaquete.First().CodUtillaje.PadLeft(25)
                    , prepaquete.First().IdUtillajeTalla.PadLeft(10)
                    , prepaquete.First().Talla.PadLeft(10)
                    , prepaquete.First().CodigoEtiqueta.PadLeft(13)
                    , prepaquete.First().IdOrden.ToString().PadLeft(10, '0')
                    , prepaquete.First().IdOperacion.ToString().PadLeft(10, '0')
                    , nombreCliente.PadLeft(25)
                    , prepaquete.First().CodigoArticulo.PadLeft(20)
                    , prepaquete.First().Productividad.ToString().PadLeft(3)
                    , Store.OperarioEjecucion.Id.ToString().PadLeft(5));

                ClienteMQTT.Publicar(string.Format("/moldeado/plc/{0}/asociarTarea", maquina.IpAutomata.PadLeft(3)), mensaje, 1);
                ClienteMQTT.Publicar(string.Format("/moldeado/plc/{0}/asociarTarea", maquina.IpAutomata.PadLeft(3)), mensaje, 1);
                ClienteMQTT.Publicar(string.Format("/moldeado/plc/{0}/asociarTarea", maquina.IpAutomata.PadLeft(3)), mensaje, 1);
                ClienteMQTT.Publicar(string.Format("/moldeado/plc/{0}/asociarTarea", maquina.IpAutomata.PadLeft(3)), mensaje, 1);

            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        /// <summary>
        /// evento que se produce cuando el usuario ficha una etiqueta de proceso
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Fichajes_OnOperacionFichada(object sender, BarquillaFichadaEventArgs e)
        {
            try
            {
                string codigoEtiqueta = e.CodigoEtiqueta.Remove(e.CodigoEtiqueta.Length - 1);
                int idOperacion = 0;
                bool ok = int.TryParse(codigoEtiqueta, out idOperacion);
                if (ok)
                {
                    var operacionesTallas = Select.ObtenerOperacionesTallasOperacion(idOperacion);
                    ElegirOperacionTalla eot = new ElegirOperacionTalla(operacionesTallas);
                    eot.ShowDialog();
                    if (eot.OfotElegida != null)
                    {
                        // quizás lo más elegante es buscar una etiqueta de barquilla de esa ofot elegida
                        // invocar a la función de barquilla

                        foreach (var maquina in Store.Bancada.Maquinas)
                        {
                            var infoBarquillaSeccion = Select.BuscarTareasPorOfot(eot.OfotElegida.ID);

                            this.AsignarTareaEjecucion(infoBarquillaSeccion, maquina, e.CodigoEtiqueta);
                        }
                        
                    }
                }
                else
                {
                    Aviso a = new Aviso("Operación no encontrada");
                    a.Show();
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        private void MainWindow_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key != Key.Return)
                {
                    etiqueta += e.Key.ToString().Replace("D", "").Replace("NumPad", "");
                }
                else
                {
                    if (etiqueta.Length >= 12)
                    {
                        this.fichajes.EtiquetaFichada(this.etiqueta);
                    }
                    etiqueta = "";
                }

                if (e.Key == Key.F1)
                {
                    Configurar c = new Configurar();
                    c.ShowDialog();
                }

                if (e.Key == Key.F2)
                {
                    PulsoGenerado(new PulsoGeneradoEventArgs(Store.Bancada.ID));
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        private void ConsumirTareaNormal(PulsoGeneradoEventArgs e)
        {
            foreach (var maquina in Store.Bancada.Maquinas)
            {
                if (maquina.TrabajoEjecucion != null)
                {
                    var ahora = DateTime.Now;

                    if (maquina.MaquinasConfiguracionesPins.DescontarAutomaticamente)
                    {

                        ClienteMQTT.Publicar(string.Format("/puesto/{0}/pulso", Store.Bancada.ID), JsonConvert.SerializeObject(new MensajePulsoNormalMQTT
                        {
                            IpAutomata = maquina.IpAutomata,
                            CodigoArticulo = maquina.CodigoArticulo,
                            CodigoBarras = maquina.TrabajoEjecucion.CodigoEtiquetaFichada,
                            CodigoOF = maquina.CodigoOrden,
                            IdObrero = Store.OperarioEjecucion.Id,
                            IdOF = maquina.IdOrden,
                            IdOperacion = maquina.IdOperacion,
                            IdTarea = maquina.TrabajoEjecucion.IdTarea,
                            NombreCliente = maquina.Cliente,
                            NumUtillajes = 1,
                            Posicion = maquina.Posicion,
                            ParesTarea = Convert.ToInt32(maquina.TrabajoEjecucion.ParesFabricar),
                            PiezaIntroducida = 1,
                            ParesUtillaje = 1,
                            TallaArticulo = maquina.TallaArticulo,
                            TallaUtillaje = maquina.TallaUtillaje,
                            Tipo = 1,
                            Utillaje = maquina.Utillaje,
                            Hora = string.Format("{0}-{1}-{2} {3}:{4}:{5}", ahora.Year, ahora.Month, ahora.Day,
                            ahora.Hour, ahora.Minute, ahora.Second)
                        }), 2); ;

                        Store.Monton++;
                        if (Store.Monton == Store.Bancada.BancadasConfiguracionesPins.ContadorPaquetes + 1)
                        {
                            Store.Monton = 1;
                        }

                    }

                    maquina.Pulsos.Add(new PulsoMaquina
                    {
                        IdTarea = maquina.IdTarea,
                        CodigoEtiqueta = maquina.TrabajoEjecucion.CodigoEtiquetaFichada,
                        Fecha = ahora,
                        IdOperario = Store.OperarioEjecucion.Id,
                        Pares = 1,
                        IdPuesto = Store.Bancada.ID,
                    });

                    ClienteMQTT.Publicar(string.Format("/ordenFabricacion/{0}/{1}/consumo", maquina.IdOrden, maquina.CodSeccion),
                        JsonConvert.SerializeObject(new MensajeConsumoOrden
                        {
                            CodigoOrden = maquina.CodigoOrden,
                            IdMaquina = maquina.ID,
                            CodSeccion = maquina.CodSeccion,
                            CantidadPaquete = (int)maquina.TrabajoEjecucion.CantidadEtiquetaFichada
                        }), 2);

                    ClienteMQTT.Publicar(string.Format("/puesto/{0}/normal", Store.Bancada.ID),
                    JsonConvert.SerializeObject(new MensajeConsumoTarea
                    {
                        IdPuesto = Store.Bancada.ID,
                        IdTarea = maquina.IdTarea,
                        ParesConsumidos = (int)maquina.MaquinasConfiguracionesPins.ProductoPorPulso,
                        PiezaIntroducida = true,
                    }), 2);

                    BusEventos.ParesActualizados();

                }
            }
        }

        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int x, int y);
        private void ResetearInactividad()
        {
            if (!this.timerInactividad.IsEnabled)
            {
                SetCursorPos(rnd.Next(0, 200), rnd.Next(0, 200));
                this.timerInactividad.Start();
            }
        }

        /// <summary>
        /// Este evento se desencadena al estar ejecutandose desde la pantalla maestra.
        /// Si e pulso viene del esclavo publica un MQTT a la pantalla esclava.
        /// Si no, lo utilizamos desde el maestro
        /// </summary>
        /// <param name="e"></param>
        private void PulsoGenerado(PulsoGeneradoEventArgs e)
        {
            try
            {
                ResetearInactividad();
                //si la maquina que produce el pulso es del esclavo
                if (Store.BancadaEsclavo != null && e.IdBancada == Store.BancadaEsclavo.ID)
                {
                    // publico el pulso para el esclavo
                    ClienteMQTT.Publicar(string.Format("/puesto/{0}/pulsoInterno", Store.Bancada.IdHermano), "", 2);
                }
                else
                {
                    // si hay operarios
                    if (Store.OperarioEjecucion != null)
                    {
                        // SI HAY TAREA
                        if (Store.Bancada.Maquinas.Any(x => x.TrabajoEjecucion != null))
                        {
                            ConsumirTareaNormal(e);
                        }
                        else
                        {
                            this.Dispatcher.BeginInvoke((Action)(() =>
                            {
                                new Aviso("No hay tarea").Show();
                            }));
                        }
                    }
                    else
                    {
                        this.Dispatcher.BeginInvoke((Action)(() =>
                        {
                            new Aviso("No hay operario").Show();

                        }));
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.Log.Write(ex);
            }

        }
        private void Uart_OnPulsoGenerado(object sender, PulsoGeneradoEventArgs e)
        {
            PulsoGenerado(e);
        }

        private void NavegacionEventos_OnNuevaPagina(object sender, EventosNavegacion.NuevaPaginaEventArgs e)
        {
            this.Frame.Navigate(e.Pagina);
        }

        private void BtTitulo_Click(object sender, RoutedEventArgs e)
        {
            if (InputManager.Current.MostRecentInputDevice is KeyboardDevice)
            {
                e.Handled = true;
                return;
            }
            NavegacionEventos.CargarNuevaPagina(NavegacionEventos.PaginaMenuPrincipal);
        }

        private void BtTituloOperarios_Click(object sender, RoutedEventArgs e)
        {
            if (InputManager.Current.MostRecentInputDevice is KeyboardDevice)
            {
                e.Handled = true;
                return;
            }
            NavegacionEventos.CargarNuevaPagina(NavegacionEventos.PaginaOperarios);
        }

        private void KillExplorer()
        {
            try
            {
                // Create a ProcessStartInfo, otherwise Explorer comes back to haunt you.
                ProcessStartInfo TaskKillPSI = new ProcessStartInfo("taskkill", "/F /IM explorer.exe");
                // Don't show a window
                TaskKillPSI.WindowStyle = ProcessWindowStyle.Hidden;
                // Create and start the process, then wait for it to exit.
                Process process = new Process();
                process.StartInfo = TaskKillPSI;
                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        private OperacionesControles BuscarControl(int idOperacion, Maquinas maq)
        {
            OperacionesControles ctrl = Store.Controles.FirstOrDefault(x => x.idOfos.Contains(idOperacion));

            if (ctrl == null)
            {
                ctrl = Select.BuscarControlOperacion(idOperacion, maq.IdTipo ?? 0);
                if (ctrl != null)
                {
                    Store.Controles.Add(ctrl);
                }
            }
            return ctrl;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            ClienteMQTT.Cerrar();
            if (uart != null)
            {
                uart.Cerrar();
            }
        }
    }
}
