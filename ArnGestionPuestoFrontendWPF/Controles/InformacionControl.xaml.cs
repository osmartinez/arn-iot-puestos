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
    /// Lógica de interacción para InformacionControl.xaml
    /// </summary>
    public partial class InformacionControl : UserControl,INotifyPropertyChanged
    {
        public string Cliente
        {
            get
            {
                if (Store.HayAlgunaTarea)
                {
                    return Store.MaquinaPrincipal.Cliente;
                }
                else
                {
                    return "< >";
                }
            }
        }
        public string OF
        {
            get
            {
                if (Store.HayAlgunaTarea)
                {
                    return Store.MaquinaPrincipal.CodigoOrden;
                }
                else
                {
                    return "< >";
                }
            }
        }
        public string Modelo
        {
            get
            {
                if (Store.HayAlgunaTarea)
                {
                    return Store.MaquinaPrincipal.Modelo;
                }
                else
                {
                    return "< >";
                }
            }
        }
        public InformacionControl()
        {
            InitializeComponent();
            this.DataContext = this;
            BusEventos.OnTareasCargadas += BusEventos_OnTareasCargadas;
            BusEventos.OnParesActualizados += BusEventos_OnParesActualizados;
            BusEventos.OnTareasCargando += BusEventos_OnTareasCargando;
        }

        private void BusEventos_OnTareasCargando(object sender, EventosTareas.NuevasTareasCargandoEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void BusEventos_OnParesActualizados(object sender, EventosTareas.ParesActualizadosEventArgs e)
        {
            Notifica();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Notifica(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void BusEventos_OnTareasCargadas(object sender, EventosTareas.NuevasTareasCargadasEventArgs e)
        {
            this.Visibility = Visibility.Visible;
            Notifica();
        }
    }
}
