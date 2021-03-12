using ArnGestionPuestoFrontendWPF.Ventanas;
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
    /// Lógica de interacción para ContadorSaldos.xaml
    /// </summary>
    public partial class ContadorSaldos : UserControl,INotifyPropertyChanged
    {
        private DispatcherTimer timerSaldos;
        public int Saldos
        {
            get
            {
                if (Store.Tareas.Any())
                {
                    return Store.Tareas.First().Saldos.Sum(x => x.Pares);
                }
                else
                {
                    return 0;
                }
            }
        }
        public int SaldosEditar { get; set; } = 0;
        
        public ContadorSaldos()
        {
            InitializeComponent();
            this.DataContext = this;
            BusEventos.OnTareasCargadas += BusEventos_OnTareasCargadas;
            this.timerSaldos = new DispatcherTimer();
            this.timerSaldos.Interval = new TimeSpan(0, 0, 0, 3);
            this.timerSaldos.Tick += TimerSaldos_Tick;
        }

        private void TimerSaldos_Tick(object sender, EventArgs e)
        {
            if (Store.Operarios.Any())
            {
                if (Store.Tareas.Any())
                {
                    Store.Tareas.First().Saldos.Add(new Entidades.EntidadesDTO.PulsoMaquina
                    {
                        Pares = this.SaldosEditar,
                        Fecha = DateTime.Now,
                        IdOperario = Store.Operarios.Any() ? Store.Operarios.First().Id : 0,
                    });
                    BusEventos.ParesActualizados(Store.Tareas.First());
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
            this.SaldosEditar = 0;
            this.timerSaldos.Stop();
            Notifica("Saldos");

        }

        private void Notifica(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void BusEventos_OnTareasCargadas(object sender, EventosTareas.NuevasTareasCargadasEventArgs e)
        {
            Notifica("Saldos");
        }

        private void CheckColorSaldosEditar()
        {
            this.PanelSuma.Visibility = Visibility.Visible;
            if (this.SaldosEditar>0)
            {
                this.TbSaldosEditar.Foreground = Brushes.Green;
            }
            else
            {
                this.TbSaldosEditar.Foreground = Brushes.Red;
            }
        }

        private void BtQuitarSaldos_Click(object sender, RoutedEventArgs e)
        {
            this.timerSaldos.Stop();
            this.timerSaldos.Start();
            this.SaldosEditar--;
            CheckColorSaldosEditar();
            Notifica("SaldosEditar");
        }

        private void BtSumarSaldos_Click(object sender, RoutedEventArgs e)
        {
            this.timerSaldos.Stop();
            this.timerSaldos.Start();
            this.SaldosEditar++;
            CheckColorSaldosEditar();
            Notifica("SaldosEditar");
        }
    }
}
