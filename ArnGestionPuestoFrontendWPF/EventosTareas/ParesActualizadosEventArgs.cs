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

        public ParesActualizadosEventArgs(Tarea tarea)
        {
            Tarea = tarea;
        }
    }
}
