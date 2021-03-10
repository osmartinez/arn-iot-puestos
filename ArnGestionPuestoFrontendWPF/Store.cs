using Entidades.EntidadesBD;
using Entidades.EntidadesDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArnGestionPuestoFrontendWPF
{
    public static class Store
    {
        public static Bancadas Bancada { get; set; }
        public static List<Tarea> Tareas { get; set; } = new List<Tarea>();
        public static List<Operarios> Operarios { get; set; } = new List<Operarios>();
    }
}
