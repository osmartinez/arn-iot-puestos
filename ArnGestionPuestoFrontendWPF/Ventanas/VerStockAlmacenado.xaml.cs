using Entidades.EntidadesDTO;
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
    /// Lógica de interacción para VerStockAlmacenado.xaml
    /// </summary>
    public partial class VerStockAlmacenado : Window
    {
        public AgrupacionStock AgrupacionStock { get; set; }
        public VerStockAlmacenado()
        {
            InitializeComponent();
            AgrupacionStock = new AgrupacionStock(Store.Stocks);
            this.DataContext = this;
        }

        private void ModificarCantidad(double pares)
        {
            if (TablaCantidades.SelectedItems.Count == 1)
            {
                TallaCantidad tc = TablaCantidades.SelectedItems[0] as TallaCantidad;
                AgrupacionStock.Actualizar(tc.Talla, pares);
                BusEventos.ParesActualizados();
            }
        }

        private void BtSumar_Click(object sender, RoutedEventArgs e)
        {
            ModificarCantidad(+1);
        }
        private void BtRestar_Click(object sender, RoutedEventArgs e)
        {
            ModificarCantidad(-1);

        }

        private void BtOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtBorrarTodo_Click(object sender, RoutedEventArgs e)
        {
            Store.Stocks.Clear();
            BusEventos.ParesActualizados();
            this.Close();
        }
    }
}
