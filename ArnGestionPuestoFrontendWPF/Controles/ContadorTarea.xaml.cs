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
                    return (int)Store.MaquinaPrincipal.CantidadCajaRealizada;
                    //return Store.MaquinaPrincipal.Pulsos.Where(x=>x.IdTarea == Store.MaquinaPrincipal.IdTarea && x.IdOperario == Store.OperarioEjecucion.Id).Sum(x=>x.Pares)
                    //    - Store.Saldos.Sum(x=>x.Pares)
                    //    + Store.Correcciones.Sum(x=>x.Pares);
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
                return (int)Store.MaquinaPrincipal.CantidadCaja;
            }
        }
        public string Talla
        {
            get
            {
                return Store.MaquinaPrincipal.TallaArticulo;
            }
        }
        public int CantidadStock
        {
            get
            {
                return (int)Store.Stocks.Sum(x => x.StockArticulosTallas.Sum(y => y.Cantidad));
            }
        }
        public ContadorTarea()
        {
            InitializeComponent();
            this.DataContext = this;
            BusEventos.OnTareasCargadas += BusEventos_OnTareasCargadas;
            BusEventos.OnTareasCargando += BusEventos_OnTareasCargando;
            BusEventos.OnParesActualizados += BusEventos_OnParesActualizados;
            this.timerExceso = new DispatcherTimer();
            this.timerExceso.Interval = new TimeSpan(0, 0, 0, 0, 400);
            this.timerExceso.Tick += TimerExceso_Tick;
            this.timerExceso.Start();
        }

        private void BusEventos_OnTareasCargando(object sender, EventosTareas.NuevasTareasCargandoEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void TimerExceso_Tick(object sender, EventArgs e)
        {
            try
            {
                if (this.CantidadFabricada >= this.CantidadFabricar && Store.HayAlgunaTarea)
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
            Notifica("CantidadStock");

        }

        private void BusEventos_OnTareasCargadas(object sender, EventosTareas.NuevasTareasCargadasEventArgs e)
        {
            PonerColorFrio();
            this.Visibility = Visibility.Visible;
            Notifica();
        }

        private void Notifica(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtStock_Click(object sender, RoutedEventArgs e)
        {
            new VerStockAlmacenado().ShowDialog();
        }
    }
}
