using Entidades.EntidadesBD;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Lógica de interacción para BotonTituloOperarios.xaml
    /// </summary>
    public partial class BotonTituloOperarios : UserControl, INotifyPropertyChanged
    {
        private Brush colorOriginal;
        public ObservableCollection<Operarios> Operarios
        {
            get
            {
                return new ObservableCollection<Operarios>(Store.Operarios);
            }
        }

        public DispatcherTimer timer;

        public BotonTituloOperarios()
        {
            InitializeComponent();
            this.DataContext = this;
            BusEventos.OnOperarioEntra += BusEventos_OnOperarioEntra;
            BusEventos.OnOperarioSale += BusEventos_OnOperarioSale;
            this.timer = new DispatcherTimer();
            this.timer.Interval = new TimeSpan(0, 0, 0, 0, 400);
            this.timer.Tick += Timer_Tick; ;
            this.timer.Start();
            this.colorOriginal = this.BtTituloOperarios.Background;
        }


        private void BusEventos_OnOperarioSale(object sender, EventosTareas.OperarioSalidaEventArgs e)
        {
            Notifica("Operarios");
        }

        private void BusEventos_OnOperarioEntra(object sender, EventosTareas.OperarioEntradaEventArgs e)
        {
            Notifica("Operarios");
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (this.Operarios.Count <= 0)
                {

                    if (this.BtTituloOperarios.Background == this.colorOriginal)
                    {
                        PonerColorCaliente();
                    }
                    else
                    {
                        PonerColorFrio();
                    }
                }
                else
                {
                    PonerColorFrio();
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
                    this.BtTituloOperarios.Background = Brushes.Red;
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
                    this.BtTituloOperarios.Background = this.colorOriginal;

                }));
            }
            catch (Exception ex)
            {
                Logs.Log.Write(ex);
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Notifica(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
