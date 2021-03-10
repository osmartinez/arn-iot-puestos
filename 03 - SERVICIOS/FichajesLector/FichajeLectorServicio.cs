using Entidades.Eventos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace FichajesLector
{
    public class FichajeLectorServicio
    {
        public string UltimaEtiqueta { get; set; } = "";
        public string CodigoBarquilla { get; set; } = "";

        public event EventHandler<BarquillaFichadaEventArgs> OnBarquillaFichada;
        private Timer timer = new Timer();

        public FichajeLectorServicio()
        {
            this.timer.Interval = 10 * 1000;
            this.timer.Elapsed += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            
        }

        private void Limpiar()
        {
            this.CodigoBarquilla = "";
            this.UltimaEtiqueta = "";
        }

        private void BarquillaFichada(string codigo)
        {
            if (OnBarquillaFichada != null)
            {
                OnBarquillaFichada(this, new BarquillaFichadaEventArgs(codigo));
            }
            this.Limpiar();
        }

        private bool EsEtiquetaMaquina(string cod)
        {
            return cod.StartsWith("02") || cod.StartsWith("2");
        }

        private bool EsEtiquetaBarquilla(string cod)
        {
            return cod.StartsWith("05") || cod.StartsWith("5");
        }

        public void EtiquetaFichada(string cod)
        {
            if (cod[0] == '5')
            {
                // barquilla
                BarquillaFichada("0" + cod);
            }
            else if (cod[0] == '2')
            {
                // maquina

            }
        }

       
    }
}
