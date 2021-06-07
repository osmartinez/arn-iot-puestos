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
using System.Windows.Threading;

namespace ArnGestionPuestoFrontendWPF.Controles
{
    /// <summary>
    /// Lógica de interacción para Reloj.xaml
    /// </summary>
    public partial class Reloj : UserControl
    {
        DispatcherTimer timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 1) };

        public Reloj()
        {
            InitializeComponent();
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {

            DateTime ahora = DateTime.Now;
            this.TbHora.Text = string.Format("{0}:{1}", ahora.Hour.ToString().PadLeft(2, '0'), ahora.Minute.ToString().PadLeft(2, '0'));
        }
    }
}
