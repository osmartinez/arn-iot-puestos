using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    /// Lógica de interacción para Calculadora.xaml
    /// </summary>
    public partial class Calculadora : Window,INotifyPropertyChanged
    {
        public string Texto { get; set; } = "";
        private TextBlock textbox;
        public Calculadora(TextBlock tb)
        {
            InitializeComponent();
            this.DataContext = this;
            this.textbox = tb;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void Notifica(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void BtNumeroClick(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            string text = bt.Name.Replace("Bt", "").Replace("Menos","-").Replace("Mas","+").Replace("Mul","x");
            this.Texto += text;
            Notifica("Texto");
        }

        private void btOk_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.Texto))
            {
                try
                {
                    this.Texto = Convert.ToString(MathEvaluator.Eval(this.Texto));
                }
                catch(Exception ex)
                {
                    this.Texto = "0";
                }
                
            }
            Notifica("Texto");

            textbox.Text = this.Texto;


            this.Close();

        }

        private void BtBorrar_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public static class MathEvaluator
    {
        public static int Eval(string input)
        {
            int ans = Evaluate(input);
            return ans;
        }

        public static int Evaluate(String input)
        {
            DataTable dt = new DataTable();
            int v = Convert.ToInt32(dt.Compute(input.Replace("x","*"),""));
            return v;
        }
    }
}
