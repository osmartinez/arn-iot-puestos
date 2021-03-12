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
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (s, e) =>
            {
                this.config = BDConfiguracion.Leer();
                Store.Bancada = BDSQL.Select.ObtenerBancadaPorId(this.config.IdBancada);
            };

            bw.RunWorkerCompleted += (s, e) =>
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
                        foreach (Tarea tarea in Store.Tareas)
                        {
                            foreach (var maquina in tarea.MaquinasEjecucion)
                            {
                                if (maquina.ID == e.Maquina.ID)
                                {
                                    tarea.Pulsos.Add(new PulsoMaquina
                                    {
                                        Pares = 1,
                                        Fecha = DateTime.Now,
                                        IdOperario = Store.Operarios.First().Id,
                                    });

                                    //Insert.InsertarConsumo(tarea.IdTarea, 1, Store.Operarios.First().Id, maquina.ID);

                                    tarea.Monton++;
                                    if (tarea.Monton == Store.Bancada.BancadasConfiguracionesPins.ContadorPaquetes + 1)
                                    {
                                        tarea.Monton = 1;
                                    }
                                    BusEventos.ParesActualizados(tarea);
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
    }
}
