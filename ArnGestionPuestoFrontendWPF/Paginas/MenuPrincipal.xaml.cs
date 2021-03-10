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

namespace ArnGestionPuestoFrontendWPF.Paginas
{
    /// <summary>
    /// Lógica de interacción para MenuPrincipal.xaml
    /// </summary>
    public partial class MenuPrincipal : Page
    {
        public MenuPrincipal()
        {
            InitializeComponent();
        }

        private void BtOperarios_Click(object sender, RoutedEventArgs e)
        {
            NavegacionEventos.CargarNuevaPagina(NavegacionEventos.PaginaOperarios);
        }

        private void BtTarea_Click(object sender, RoutedEventArgs e)
        {
            NavegacionEventos.CargarNuevaPagina(NavegacionEventos.PaginaTarea);
        }
    }
}
