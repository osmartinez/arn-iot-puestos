using Entidades.EntidadesDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArnGestionPuestoFrontendWPF.EventosTareas
{
    public class ParesActualizadosEventArgs
    {
        public Tarea Tarea { get; private set; }
        public double Pares { get; set; }
        public ParesActualizadosEventArgs(Tarea tarea,double pares=0)
        {
            this.Tarea = tarea;
            this.Pares = pares;
        }
    }
}
