using ArnGestionPuestoFrontendWPF.Ventanas;
using BDSQL;
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
    /// Lógica de interacción para ContadorPaquete.xaml
    /// </summary>
    public partial class ContadorPaquete : UserControl, INotifyPropertyChanged
    {
        public int Monton
        {
            get
            {
                if (Store.Tareas.Any())
                {
                    return Store.TareaConsumir.Monton;
                }
                else
                {
                    return 0;
                }
            }
        }
        public int Contador
        {
            get
            {
                if (Store.Bancada != null)
                {
                    return (int)Store.Bancada.BancadasConfiguracionesPins.ContadorPaquetes;
                }
                return 5;
            }
        }
        private DispatcherTimer timerExceso;

        public ContadorPaquete()
        {
            InitializeComponent();
            this.DataContext = this;
            BusEventos.OnParesActualizados += BusEventos_OnParesActualizados;
            BusEventos.OnTareasCargadas += BusEventos_OnTareasCargadas;

            this.timerExceso = new DispatcherTimer();
            this.timerExceso.Interval = new TimeSpan(0, 0, 0, 0, 400);
            this.timerExceso.Tick += TimerExceso_Tick;
            this.timerExceso.Start();
        }

        private void TimerExceso_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Store.Bancada != null && this.Monton == this.Contador)
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

        private void BusEventos_OnTareasCargadas(object sender, EventosTareas.NuevasTareasCargadasEventArgs e)
        {
            Notifica();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Notifica(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void BusEventos_OnParesActualizados(object sender, EventosTareas.ParesActualizadosEventArgs e)
        {
            if (Monton != 0 && Contador != 0 && Monton == Contador)
            {
                if (Store.Bancada.BancadasConfiguracionesPins.AvisarFinPaquete)
                {
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"Assets/beep_monton.wav");
                    player.Play();
                }
            }
            Notifica();
        }

        private void BtEditarContador_Click(object sender, RoutedEventArgs e)
        {
            if (InputManager.Current.MostRecentInputDevice is KeyboardDevice)
            {
                e.Handled = true;
                return;
            }
            EditarContador();
        }

        private void EditarContador()
        {
            try
            {
                Calculadora c = new Calculadora(this.TbContadorPaquetes);
                c.ShowDialog();
                int contador = 0;
                int.TryParse(TbContadorPaquetes.Text, out contador);
                Update.UpdateContadorPaquetesBancada(Store.Bancada, contador);
                
            }
            catch (Exception ex)
            {
                Logs.Log.Write(ex);
            }
            Notifica();
            PonerColorFrio();

        }


        private void BtSumar_Click(object sender, RoutedEventArgs e)
        {
            if (Store.TareaConsumir != null)
            {
                Store.TareaConsumir.Monton++;
                if (Store.TareaConsumir.Monton == Store.Bancada.BancadasConfiguracionesPins.ContadorPaquetes + 1)
                {
                    Store.TareaConsumir.Monton = 1;
                }
                Notifica("Monton");
            }
        }

        private void BtRestar_Click(object sender, RoutedEventArgs e)
        {
            if (Store.TareaConsumir != null)
            {
                Store.TareaConsumir.Monton--;
                if (Store.TareaConsumir.Monton == Store.Bancada.BancadasConfiguracionesPins.ContadorPaquetes + 1)
                {
                    Store.TareaConsumir.Monton = 1;
                }
                Notifica("Monton");
            }
        }

        private void TbContadorPaquetes_MouseDown(object sender, MouseButtonEventArgs e)
        {
            EditarContador();

        }
    }
}
