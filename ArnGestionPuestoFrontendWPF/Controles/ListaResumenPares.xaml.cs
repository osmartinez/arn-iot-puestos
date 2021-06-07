using Entidades.EntidadesDTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Lógica de interacción para ListaResumenPares.xaml
    /// </summary>
    public partial class ListaResumenPares : UserControl
    {
        public ObservableCollection<ResumenParesOrden> Informe { get; set; } = new ObservableCollection<ResumenParesOrden>();
        public ListaResumenPares()
        {
            InitializeComponent();
            this.DataContext = this;
            CargarInforme();
            BusEventos.OnParesActualizados += BusEventos_OnParesActualizados;
        }

        private void BusEventos_OnParesActualizados(object sender, EventosTareas.ParesActualizadosEventArgs e)
        {
            CargarInforme();
        }

        private void CargarInforme()
        {
            Informe.Clear();
            if (Store.HayAlgunaTarea)
            {
                var agrupados = Store.MaquinaPrincipal.Pulsos.GroupBy(x => new { x.CodigoOrden, x.Talla });
                foreach (var grupo in agrupados)
                {
                    Informe.Add(new ResumenParesOrden
                    {
                        CodigoOrden = grupo.Key.CodigoOrden,
                        Talla = grupo.Key.Talla,
                        Pares = grupo.Sum(x=>x.Pares),
                    });;
                }
            }
        }
    }
}
