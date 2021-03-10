using Entidades.EntidadesDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArnGestionPuestoFrontendWPF.EventosTareas
{
    public class NuevasTareasCargadasEventArgs:EventArgs
    {
        public List<Tarea> Tareas { get; set; }

        public NuevasTareasCargadasEventArgs(List<Tarea> tareas)
        {
            Tareas = tareas;
        }
    }
}
