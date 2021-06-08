using LiveCharts;
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
    /// Lógica de interacción para GraficoRitmo.xaml
    /// </summary>
    public partial class GraficoRitmo : UserControl
    {
        public ChartValues<double> Values { get; set; } = new ChartValues<double>();

        public GraficoRitmo()
        {
            InitializeComponent();

            try
            {
                var pulsos = Store.MaquinaPrincipal.Pulsos.Where(x => x.IdOperario == Store.OperarioEjecucion.Id).ToList();
                if (pulsos.Any())
                {
                    DateTime ahora = DateTime.Now;
                    pulsos = pulsos.OrderBy(x => x.Fecha).ToList();
                    DateTime aux = pulsos.First().Fecha;

                    while (aux < ahora)
                    {
                        DateTime copia = aux;
                        aux = aux.AddHours(1);
                        var pulsosRango = pulsos.Where(x => copia <= x.Fecha && x.Fecha <= aux);
                        Values.Add(pulsosRango.Sum(x => x.Pares));
                    }
                }
            }
            catch(Exception ex)
            {
                Logs.Log.Write(ex);
            }
           

            DataContext = this;
        }

        private void UpdateOnclick(object sender, RoutedEventArgs e)
        {
            Chart.Update(true);
        }
    }
}
