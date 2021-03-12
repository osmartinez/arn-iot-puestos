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

namespace ArnGestionPuestoFrontendWPF.Controles
{
    /// <summary>
    /// Lógica de interacción para ContadorPaquete.xaml
    /// </summary>
    public partial class ContadorPaquete : UserControl,INotifyPropertyChanged
    {
        public int Monton
        {
            get
            {
                if (Store.Tareas.Any())
                {
                    return Store.Tareas.First().Monton;
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
                return (int)Store.Bancada.BancadasConfiguracionesPins.ContadorPaquetes;
            }
        }
        public ContadorPaquete()
        {
            InitializeComponent();
            this.DataContext = this;
            BusEventos.OnParesActualizados += BusEventos_OnParesActualizados;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Notifica(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void BusEventos_OnParesActualizados(object sender, EventosTareas.ParesActualizadosEventArgs e)
        {
            if(Monton!=0 && Contador != 0 && Monton == Contador)
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

                int contador = Convert.ToInt32(TbContadorPaquetes.Text);
                Update.UpdateContadorPaquetesBancada(Store.Bancada, contador);
                Notifica();
            }
            catch (Exception ex)
            {
                Logs.Log.Write(ex);
            }
        }

        private void TbContadorPaquetes_MouseUp(object sender, MouseButtonEventArgs e)
        {
            EditarContador();
        }
    }
}
