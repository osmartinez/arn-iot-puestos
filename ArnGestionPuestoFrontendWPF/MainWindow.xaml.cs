using ArnGestionPuestoFrontendWPF.Ventanas;
using BDSQL;
using DatosConfiguracion;
using Entidades.EntidadesConfiguracion;
using Entidades.EntidadesDTO;
using Entidades.Eventos;
using FichajesLector;
using Logs;
using SerialCom;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
                        Uart uart = new Uart(Store.Bancada.Maquinas);
                        uart.OnPulsoGenerado += this.Uart_OnPulsoGenerado;
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

        private void Uart_OnPulsoGenerado(object sender, PulsoGeneradoEventArgs e)
        {
            try
            {
                if (Store.Operarios.Any())
                {
                    if (Store.Tareas.Any())
                    {
                        Tarea tareaConsumir = null;
                        foreach (Tarea tarea in Store.Tareas.Where(x => !x.Acabada))
                        {
                            tareaConsumir = tarea;
                            break;
                        }

                        if(tareaConsumir==null && Store.Tareas.Any())
                        {
                            tareaConsumir = Store.Tareas.Last();
                        }

                        if (tareaConsumir != null)
                        {
                            foreach (var maquina in tareaConsumir.MaquinasEjecucion)
                            {
                                if (maquina.ID == e.Maquina.ID)
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

                                    Insert.InsertarPulso(tareaConsumir, Store.Operarios.First().Id,(int)maquina.MaquinasConfiguracionesPins.ProductoPorPulso);

                                    tareaConsumir.Monton++;
                                    if (tareaConsumir.Monton == Store.Bancada.BancadasConfiguracionesPins.ContadorPaquetes + 1)
                                    {
                                        tareaConsumir.Monton = 1;
                                    }
                                    BusEventos.ParesActualizados(tareaConsumir);
                                }
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
    }
}
