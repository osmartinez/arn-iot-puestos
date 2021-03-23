using Entidades.EntidadesDTO;
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
using System.Windows.Threading;

namespace ArnGestionPuestoFrontendWPF.Controles
{
    /// <summary>
    /// Lógica de interacción para ContadorTarea.xaml
    /// </summary>
    public partial class ContadorTarea : UserControl,INotifyPropertyChanged
    {
        private DispatcherTimer timerExceso;
        private List<Tarea> tareas = new List<Tarea>();
        public event PropertyChangedEventHandler PropertyChanged;
        public string Utillaje
        {
            get
            {
                if (tareas.Any())
                {
                    return tareas.First().CodigoUtillaje;
                }
                else
                {
                    return "";
                }
            }
        }
        public int CantidadFabricada
        {
            get
            {
                if (tareas.Any())
                {
                    return Store.TareaConsumir.CantidadFabricadaReal;
                }
                else
                {
                    return 0;
                }
            }
        }
        public int CantidadFabricar
        {
            get
            {
                if (tareas.Any())
                {
                    return Convert.ToInt32(tareas.First().CantidadFabricar);
                }
                else
                {
                    return 0;
                }
            }
        }
        public string Talla
        {
            get
            {
                if (tareas.Any())
                {
                    if(tareas.First().TallaUtillaje == "00")
                    {
                        return tareas.First().TallasArticulo;
                    }
                    else
                    {
                        return tareas.First().TallaUtillaje;
                    }
                }
                else
                {
                    return "";
                }
            }
        }
        public ContadorTarea()
        {
            InitializeComponent();
            this.DataContext = this;
            BusEventos.OnTareasCargadas += BusEventos_OnTareasCargadas;
            BusEventos.OnParesActualizados += BusEventos_OnParesActualizados;
            this.timerExceso = new DispatcherTimer();
            this.timerExceso.Interval = new TimeSpan(0, 0, 0, 0, 400);
            this.timerExceso.Tick += TimerExceso_Tick;
            this.timerExceso.Start();
        }

        private void TimerExceso_Tick(object sender, EventArgs e)
        {
            try
            {
                if (this.CantidadFabricada >= this.CantidadFabricar)
                {
                    border.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        if (border.Background == Brushes.White)
                        {
                            PonerColorCaliente();
                        }
                        else
                        {
                            PonerColorFrio();
                        }
                    }));
                }
               
            }
            catch (Exception ex)
            {
                Logs.Log.Write(ex);
            }
        }

        private void PonerColorCaliente()
        {
            try
            {
                this.Dispatcher.BeginInvoke((Action)(() =>
                {
                    border.Background = Brushes.Red;
                }));
            }
            catch (Exception ex)
            {
                Logs.Log.Write(ex);
            }

        }

        private void PonerColorFrio()
        {
            try
            {
                this.Dispatcher.BeginInvoke((Action)(() =>
                {
                    border.Background = Brushes.White;

                }));
            }
            catch (Exception ex)
            {
                Logs.Log.Write(ex);
            }

        }

        private void BusEventos_OnParesActualizados(object sender, EventosTareas.ParesActualizadosEventArgs e)
        {
            Notifica("CantidadFabricada");
        }

        private void BusEventos_OnTareasCargadas(object sender, EventosTareas.NuevasTareasCargadasEventArgs e)
        {
            tareas = e.Tareas;
            PonerColorFrio();
            Notifica();
        }

        private void Notifica(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
