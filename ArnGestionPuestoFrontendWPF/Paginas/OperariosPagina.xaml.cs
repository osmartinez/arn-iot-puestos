using BDSQL;
using Entidades.EntidadesBD;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ArnGestionPuestoFrontendWPF.Paginas
{
    /// <summary>
    /// Lógica de interacción para OperariosPagina.xaml
    /// </summary>
    public partial class OperariosPagina : Page, INotifyPropertyChanged
    {
        public string CodigoOperario { get; set; } = "";
        public ObservableCollection<Operarios> Operarios
        {
            get
            {
                return new ObservableCollection<Operarios>(Store.Operarios);
            }
        }
     
        public OperariosPagina()
        {
            InitializeComponent();
            this.DataContext = this;
            BusEventos.OnOperarioEntra += BusEventos_OnOperarioEntra;
            BusEventos.OnOperarioSale += BusEventos_OnOperarioSale; ;
        }

        private void BusEventos_OnOperarioSale(object sender, EventosTareas.OperarioSalidaEventArgs e)
        {
            Notifica();
        }

        private void BusEventos_OnOperarioEntra(object sender, EventosTareas.OperarioEntradaEventArgs e)
        {
            Notifica();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Notifica(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void BtNumeroClick(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            string text = bt.Name.Replace("Bt", "");
            this.CodigoOperario += text;
            Notifica("CodigoOperario");
        }

        private void BtBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (this.CodigoOperario.Length > 0)
            {
                this.CodigoOperario = this.CodigoOperario.Substring(0, this.CodigoOperario.Length - 1);
                Notifica("CodigoOperario");
            }
        }

        private void btOk_Click(object sender, RoutedEventArgs e)
        {
            if (this.CodigoOperario.Length > 0)
            {
                Operarios o = Select.BuscarOperarioPorCodigo(this.CodigoOperario.Trim());
                if (o != null)
                {
                    if (!Store.Operarios.Any(x=>x.Id == o.Id))
                    {
                        Store.Operarios.Add(o);
                        BusEventos.OperarioEntra(o);
                    }

                }
                this.CodigoOperario = "";
                Notifica("CodigoOperario");
            }
        }

        private void BtOperarioSalir_Click(object sender, RoutedEventArgs e)
        {
            if(this.TablaOperarios.SelectedItems.Count == 1)
            {
                Operarios o = this.TablaOperarios.SelectedItems[0] as Operarios;
                Store.Operarios.Remove(o);
                BusEventos.OperarioSale(o);
            }
        }
    }
}
