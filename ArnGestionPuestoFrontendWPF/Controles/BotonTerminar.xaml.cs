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
    /// Lógica de interacción para BotonTerminar.xaml
    /// </summary>
    public partial class BotonTerminar : UserControl
    {
        public BotonTerminar()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Store.Tareas = new List<Entidades.EntidadesDTO.Tarea>();
            BusEventos.TareasCargadas(Store.Tareas);
        }
    }
}
