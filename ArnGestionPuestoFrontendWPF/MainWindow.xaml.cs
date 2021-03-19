using ArnGestionPuestoFrontendWPF.Ventanas;
using BDSQL;
using DatosConfiguracion;
using Entidades.EntidadesConfiguracion;
using Entidades.EntidadesDTO;
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
                this.PreviewKeyUp += MainWindow_PreviewKeyUp;
                this.fichajes.OnBarquillaFichada += Fichajes_OnBarquillaFichada;
                NavegacionEventos.OnNuevaPagina += NavegacionEventos_OnNuevaPagina;
                NavegacionEventos.CargarNuevaPagina(NavegacionEventos.PaginaOperarios);
                CargarConfiguracion();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }

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
                            uart = new Uart(Store.Bancada);
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
            ClienteMQTT.Topics.Add(new Topic(1, "/puesto/+/asociarTarea", new Regex(@"^\/puesto\/\s?[0-9]+\/asociarTarea$"), 3, 1, qos: 1));
            ClienteMQTT.Topics.Add(new Topic(2, "/puesto/+/normal", new Regex(@"^\/puesto\/\s?[0-9]+\/normal$"), 3, 1, qos: 2));
            ClienteMQTT.Topics.Add(new Topic(3, "/puesto/+/preparar", new Regex(@"^\/puesto\/\s?[0-9]+\/preparar"), 3, 1, qos: 1));
            // si no es master me tengo que suscribir al pulso interno de mi master /puesto/mi_id/pulsoInterno
            if (!Store.Bancada.EsMaster)
            {
                ClienteMQTT.Topics.Add(new Topic(4, string.Format("/puesto/{0}/pulsoInterno", Store.Bancada.ID), new Regex(@"^\/puesto\/\s?[0-9]+\/pulsoInterno"), 3, 1, qos: 2));
                ClienteMQTT.Topics[3].Callbacks.Add(PulsoInterno);
            }
            ClienteMQTT.Topics[1].Callbacks.Add(Normal);

            ClienteMQTT.Iniciar(Store.Bancada.Nombre);
        }

        private void PulsoInterno(string msg, string topicRecibido, Topic topic)
        {
            PulsoGenerado(new PulsoGeneradoEventArgs(Store.Bancada.ID));
        }
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
                    Tarea tareaAfectada = Store.Tareas.FirstOrDefault(x => x.IdTarea == consumo.IdTarea);
                    if (tareaAfectada != null)
                    {
                        if (consumo.PiezaIntroducida)
                        {
                            tareaAfectada.Pulsos.Add(new PulsoMaquina
                            {
                                Fecha = DateTime.Now,
                                IdOperario = 0,
                                IdPuesto = consumo.IdPuesto,
                                Pares = consumo.ParesConsumidos,
                            });
                            BusEventos.ParesActualizados(tareaAfectada);
                        }
                    }
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


        private void Fichajes_OnBarquillaFichada(object sender, BarquillaFichadaEventArgs e)
        {
            try
            {
                var infos = Select.BuscarTareasPorCodigoBarquilla(e.CodigoEtiqueta, Store.Bancada.Maquinas.ToList());
                List<Tarea> tareas = new List<Tarea>();
                foreach (var info in infos)
                {
                    tareas.Add(new Tarea(info));
                }
                Store.Tareas = tareas;
                BusEventos.TareasCargadas(Store.Tareas);
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
                    if (etiqueta.Length == 12)
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
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }


        private void PulsoGenerado(PulsoGeneradoEventArgs e)
        {
            try
            {
                if (Store.Operarios.Any())
                {
                    if (Store.Tareas.Any())
                    {

                        Tarea tareaConsumir = Store.TareaConsumir;

                        if (tareaConsumir != null)
                        {
                            var maquina = tareaConsumir.MaquinasEjecucion.First();

                            if (maquina.IdBancada == e.IdBancada)
                            {
                                tareaConsumir.Pulsos.Add(new PulsoMaquina
                                {
                                    Pares = (int)maquina.MaquinasConfiguracionesPins.ProductoPorPulso,
                                    Fecha = DateTime.Now,
                                    IdOperario = Store.Operarios.First().Id,
                                });



                                if (maquina.MaquinasConfiguracionesPins.DescontarAutomaticamente)
                                {
                                    Insert.InsertarConsumo(tareaConsumir.IdTarea, (int)maquina.MaquinasConfiguracionesPins.ProductoPorPulso, Store.Operarios.First().Id, maquina.ID);

                                }

                                Insert.InsertarPulso(tareaConsumir, Store.Operarios.First().Id, Store.Bancada.ID, (int)maquina.MaquinasConfiguracionesPins.ProductoPorPulso);

                                tareaConsumir.Monton++;
                                if (tareaConsumir.Monton == Store.Bancada.BancadasConfiguracionesPins.ContadorPaquetes + 1)
                                {
                                    tareaConsumir.Monton = 1;
                                }
                                BusEventos.ParesActualizados(tareaConsumir);

                                ClienteMQTT.Publicar(string.Format("/ordenFabricacion/{0}/{1}/consumo", tareaConsumir.IdOrden, maquina.CodSeccion),
                                    JsonConvert.SerializeObject(new MensajeConsumoOrden
                                    {
                                        CodigoOrden = tareaConsumir.CodigoOrden,
                                        IdMaquina = maquina.ID,
                                        CodSeccion = maquina.CodSeccion,
                                        CantidadPaquete = (int)tareaConsumir.CantidadEtiqueta
                                    }), 2);

                                ClienteMQTT.Publicar(string.Format("/puesto/{0}/normal", Store.Bancada.ID),
                                JsonConvert.SerializeObject(new MensajeConsumoTarea
                                {
                                    IdPuesto = Store.Bancada.ID,
                                    IdTarea = tareaConsumir.IdTarea,
                                    ParesConsumidos = (int)maquina.MaquinasConfiguracionesPins.ProductoPorPulso,
                                    PiezaIntroducida = true,
                                }), 2);

                            }
                            else
                            {
                                ClienteMQTT.Publicar(string.Format("/puesto/{0}/pulsoInterno", Store.Bancada.IdHermano),
                                "", 2);
                            }
                        }
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
