using ArnGestionPuestoFrontendWPF.Ventanas;
using BDSQL;
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
            if (InputManager.Current.MostRecentInputDevice is KeyboardDevice)
            {
                e.Handled = true;
                return;
            }


            if (Store.Bancada.EsManual)
            {
                FinalizarManual fm = new FinalizarManual();
                fm.ShowDialog();
            }
            else
            {

                foreach (var maquina in Store.Bancada.Maquinas)
                {
                    if (maquina.TrabajoEjecucion != null)
                    {
                        var cola = Insert.EliminarDeColaTrabajo(maquina.TrabajoEjecucion.CodigoEtiquetaFichada,maquina.ID);
                        maquina.AsignarColaTrabajo(cola);
                    }
                    else
                    {
                        new Aviso(string.Format("No hay tarea")).Show();
                    }
                }
            }

            Store.Saldos.Clear();
            Store.Correcciones.Clear();

            BusEventos.TareasCargadas();



        }
    }
}
