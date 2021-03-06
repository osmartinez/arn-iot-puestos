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
        public event EventHandler<BarquillaFichadaEventArgs> OnOperacionFichada;
        public event EventHandler<BarquillaFichadaEventArgs> OnContenedorFichado;


        public FichajeLectorServicio()
        {
          
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

        private void OperacionFichada(string codigo)
        {
            if (OnOperacionFichada != null)
            {
                OnOperacionFichada(this, new BarquillaFichadaEventArgs(codigo));
            }
            this.Limpiar();
        }

        private void ContenedorFichado(string codigo)
        {
            if (OnContenedorFichado != null)
            {
                OnContenedorFichado(this, new BarquillaFichadaEventArgs(codigo));
            }
            this.Limpiar();
        }

        public void EtiquetaFichada(string cod)
        {
            if(cod.Length > 0)
            {
                if (cod[0] == '5')
                {
                    // barquilla
                    BarquillaFichada("0" + cod);
                }
                else if (cod[0] == '0')
                {
                    OperacionFichada("0" + cod);
                }
                else if (cod[0] == '1')
                {
                    ContenedorFichado("0" + cod);
                }
                else if (cod[0] == '2')
                {
                    // maquina
                }
            }
           
        }

       
    }
}
