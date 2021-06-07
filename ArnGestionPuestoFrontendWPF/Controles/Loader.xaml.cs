using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para Loader.xaml
    /// </summary>
    public partial class Loader : UserControl
    {
        public Loader()
        {
            InitializeComponent();
            BusEventos.OnTareasCargando += BusEventos_OnTareasCargando;
            BusEventos.OnTareasCargadas += BusEventos_OnTareasCargadas;
        }

        private void BusEventos_OnTareasCargadas(object sender, EventosTareas.NuevasTareasCargadasEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void BusEventos_OnTareasCargando(object sender, EventosTareas.NuevasTareasCargandoEventArgs e)
        {
            this.Visibility = Visibility.Visible;
        }
    }
}
