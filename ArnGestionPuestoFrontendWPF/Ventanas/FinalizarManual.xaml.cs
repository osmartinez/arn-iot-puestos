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
using System.Windows.Shapes;

namespace ArnGestionPuestoFrontendWPF.Ventanas
{
    /// <summary>
    /// Lógica de interacción para FinalizarManual.xaml
    /// </summary>
    public partial class FinalizarManual : Window
    {
        public string CodigoOrden
        {
            get
            {
                return Store.TareasCodigoOrdenToString;
            }
        }
        public string NombreCliente
        {
            get
            {
                return Store.TareasClienteToString;
            }
        }

        public string CodigoUtillaje
        {
            get
            {
                return Store.TareasUtillajeToString;
            }
        }
        public string Talla
        {
            get
            {
                return Store.TareasTallaToString;
            }
        }

        public FinalizarManual()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void BtIntroducirPares_Click(object sender, RoutedEventArgs e)
        {
            Calculadora c = new Calculadora(this.TbPares);
            c.ShowDialog();
        }
    }
}
