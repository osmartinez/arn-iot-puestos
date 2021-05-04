using ArnGestionPuestoFrontendWPF.Ventanas;
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
        public event PropertyChangedEventHandler PropertyChanged;
        private bool estaAvisado = false;
        public string Utillaje
        {
            get
            {
                return Store.MaquinaPrincipal.Utillaje;
            }
        }
        public int CantidadFabricada
        {
            get
            {
                if (Store.HayAlgunaTarea)
                {
                    return Store.MaquinaPrincipal.Pulsos.Where(x=>x.IdTarea == Store.MaquinaPrincipal.IdTarea).Sum(x=>x.Pares);
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
                return Store.MaquinaPrincipal.ParesFabricando;
            }
        }
        public string Talla
        {
            get
            {
                return Store.MaquinaPrincipal.TallaArticulo;
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
                    if(Store.Bancada.BancadasConfiguracionesPins.AvisarFinPaquete && !estaAvisado)
                    {
                        new Aviso("Has finalizado la tarea", hablar: true);
                        estaAvisado = true;
                    }

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
                else
                {
                    estaAvisado = false;
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
            PonerColorFrio();
            Notifica();
        }

        private void Notifica(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
