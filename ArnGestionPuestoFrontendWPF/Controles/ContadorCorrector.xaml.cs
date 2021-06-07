using ArnGestionPuestoFrontendWPF.Ventanas;
using BDSQL;
using Entidades.EntidadesDTO;
using MQTT;
using Newtonsoft.Json;
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
    /// Lógica de interacción para ContadorCorrector.xaml
    /// </summary>
    public partial class ContadorCorrector : UserControl, INotifyPropertyChanged
    {
        private DispatcherTimer timerCorreccion;
        public int CorreccionEditar { get; set; } = 0;

        public int Correccion
        {
            get
            {
                return Store.Correcciones.Sum(x=>x.Pares);
            }
        }
        public ContadorCorrector()
        {
            InitializeComponent();
            this.DataContext = this;
            BusEventos.OnTareasCargadas += BusEventos_OnTareasCargadas; ;
            this.timerCorreccion = new DispatcherTimer();
            this.timerCorreccion.Interval = new TimeSpan(0, 0, 0, 3);
            this.timerCorreccion.Tick += TimerCorreccion_Tick; ;
        }

        private void BusEventos_OnTareasCargadas(object sender, EventosTareas.NuevasTareasCargadasEventArgs e)
        {
            Notifica("CorreccionEditar");
            Notifica("Correccion");
        }

        private void TimerCorreccion_Tick(object sender, EventArgs e)
        {
            if (Store.Operarios.Any())
            {
                if (Store.HayAlgunaTarea)
                {
                    Store.Correcciones.Add(new PulsoMaquina
                    {
                        Fecha = DateTime.Now,
                        IdOperario = Store.OperarioEjecucion.Id,
                        IdPuesto = Store.Bancada.ID,
                        Pares = CorreccionEditar,
                    });
                    BusEventos.ParesActualizados();
                    BusEventos.CorreccionActualizada(CorreccionEditar);

                }
                else
                {
                    new Aviso("No hay tarea").Show();
                }
            }
            else
            {
                new Aviso("No hay operarios").Show();
            }

            this.PanelSuma.Visibility = Visibility.Hidden;
            this.CorreccionEditar = 0;
            this.timerCorreccion.Stop();
            Notifica("Correccion");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Notifica(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CheckColorSaldosEditar()
        {
            this.PanelSuma.Visibility = Visibility.Visible;
            if (this.CorreccionEditar > 0)
            {
                this.TbSaldosEditar.Foreground = Brushes.Green;
            }
            else
            {
                this.TbSaldosEditar.Foreground = Brushes.Red;
            }
        }

        private void BtMenos_Click(object sender, RoutedEventArgs e)
        {
            if (InputManager.Current.MostRecentInputDevice is KeyboardDevice)
            {
                e.Handled = true;
                return;
            }

            this.timerCorreccion.Stop();
            this.timerCorreccion.Start();
            this.CorreccionEditar--;
            CheckColorSaldosEditar();
            Notifica("CorreccionEditar");
        }

        private void BtMas_Click(object sender, RoutedEventArgs e)
        {
            if (InputManager.Current.MostRecentInputDevice is KeyboardDevice)
            {
                e.Handled = true;
                return;
            }

            this.timerCorreccion.Stop();
            this.timerCorreccion.Start();
            this.CorreccionEditar++;
            CheckColorSaldosEditar();
            Notifica("CorreccionEditar");
        }
    }
}
