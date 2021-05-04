using ArnGestionPuestoFrontendWPF.EventosTareas;
using Entidades.EntidadesBD;
using Entidades.EntidadesDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArnGestionPuestoFrontendWPF
{
    public static class BusEventos
    {
        public static event EventHandler<NuevasTareasCargadasEventArgs> OnTareasCargadas;
        public static event EventHandler<ParesActualizadosEventArgs> OnParesActualizados;
        public static event EventHandler<OperarioSalidaEventArgs> OnOperarioSale;
        public static event EventHandler<OperarioEntradaEventArgs> OnOperarioEntra;


        public static void TareasCargadas(List<Tarea> tareas)
        {
            if (OnTareasCargadas != null)
            {
                OnTareasCargadas(null, new NuevasTareasCargadasEventArgs(tareas));
            }
        }

        public static void ParesActualizados()
        {
            if(OnParesActualizados!= null)
            {
                OnParesActualizados(null, new ParesActualizadosEventArgs(null));
            }
        }

        public static void OperarioEntra(Operarios o)
        {
            if (OnOperarioEntra != null)
            {
                OnOperarioEntra(null, new OperarioEntradaEventArgs());
            }
        }

        public static void OperarioSale(Operarios o)
        {
            if (OnOperarioSale != null)
            {
                OnOperarioSale(null, new OperarioSalidaEventArgs());
            }
        }
    }
}
