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

namespace ArnGestionPuestoFrontendWPF.Paginas
{
    /// <summary>
    /// Lógica de interacción para ConfiguracionOperarioPagina.xaml
    /// </summary>
    public partial class ConfiguracionOperarioPagina : Page
    {
        public ConfiguracionOperarioPagina()
        {
            InitializeComponent();
            if (Store.Bancada != null)
            {
                ChckAuditivoMonton.IsChecked = Store.Bancada.BancadasConfiguracionesPins.AvisarFinPaquete;
                ChckAuditivoTarea.IsChecked = Store.Bancada.BancadasConfiguracionesPins.AvisarFinTarea;
            }
            else
            {
                ChckAuditivoMonton.IsEnabled = false;
                ChckAuditivoTarea.IsEnabled = false;
            }
        }

        private void ChckAuditivoMonton_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                Update.UpdateConfiguracionBancada(true, Store.Bancada.BancadasConfiguracionesPins.AvisarFinTarea,Store.Bancada.ID);
                Store.Bancada.BancadasConfiguracionesPins.AvisarFinPaquete = true;
            }catch(Exception ex)
            {
                Logs.Log.Write(ex);
            }
        }

        private void ChckAuditivoMonton_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                Update.UpdateConfiguracionBancada(false, Store.Bancada.BancadasConfiguracionesPins.AvisarFinTarea, Store.Bancada.ID);
                Store.Bancada.BancadasConfiguracionesPins.AvisarFinPaquete = false;
            }
            catch (Exception ex)
            {
                Logs.Log.Write(ex);
            }
        }

        private void ChckAuditivoTarea_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                Update.UpdateConfiguracionBancada(Store.Bancada.BancadasConfiguracionesPins.AvisarFinPaquete,true, Store.Bancada.ID);
                Store.Bancada.BancadasConfiguracionesPins.AvisarFinTarea = true;
            }
            catch (Exception ex)
            {
                Logs.Log.Write(ex);
            }
        }

        private void ChckAuditivoTarea_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                Update.UpdateConfiguracionBancada(Store.Bancada.BancadasConfiguracionesPins.AvisarFinPaquete, false, Store.Bancada.ID);
                Store.Bancada.BancadasConfiguracionesPins.AvisarFinTarea = false;
            }
            catch (Exception ex)
            {
                Logs.Log.Write(ex);
            }
        }
    }
}
