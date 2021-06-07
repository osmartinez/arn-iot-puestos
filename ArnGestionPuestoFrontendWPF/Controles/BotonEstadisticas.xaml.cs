using ArnGestionPuestoFrontendWPF.Paginas;
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
    /// Lógica de interacción para BotonEstadisticas.xaml
    /// </summary>
    public partial class BotonEstadisticas : UserControl
    {
        public BotonEstadisticas()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavegacionEventos.CargarNuevaPagina(new EstadisticasPagina());
        }
    }
}
