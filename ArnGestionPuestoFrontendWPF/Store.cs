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
        public static Bancadas BancadaEsclavo { get; set; }
        public static List<Tarea> Tareas { get; set; } = new List<Tarea>();
        public static List<Operarios> Operarios { get; set; } = new List<Operarios>();
        public static Tarea TareaConsumir
        {
            get
            {
                Tarea tareaConsumir = null;

                foreach (Tarea tarea in Store.Tareas.Where(x => !x.Acabada))
                {
                    tareaConsumir = tarea;
                    break;
                }

                if (tareaConsumir == null && Store.Tareas.Any())
                {
                    tareaConsumir = Store.Tareas.Last();
                }

                return tareaConsumir;
            }
        }
        public static string TareasCodigoOrdenToString
        {
            get
            {
                if (Tareas.Any())
                {
                    if (Tareas.Count == 1)
                    {
                        return Tareas.First().CodigoOrden;
                    }
                    else
                    {
                        return string.Format("{0} y {1} más", Tareas.First().CodigoOrden, Tareas.Count - 1);
                    }
                }
                else
                {
                    return "<SIN TAREA>";
                }
            }
        }
        public static string TareasClienteToString
        {
            get
            {
                if (Tareas.Any())
                {
                    if (Tareas.Count == 1)
                    {
                        return Tareas.First().NombreClienteAcortado;
                    }
                    else
                    {
                        return string.Format("{0} y {1} más", Tareas.First().NombreClienteAcortado, Tareas.Count - 1);
                    }
                }
                else
                {
                    return "<SIN TAREA>";
                }
            }
        }
        public static string TareasUtillajeToString
        {
            get
            {
                if (Tareas.Any())
                {
                    return Tareas.First().CodigoUtillaje;
                }
                else
                {
                    return "<SIN TAREA>";
                }
            }
        }
        public static string TareasTallaToString
        {
            get
            {
                if (Tareas.Any())
                {
                    return Tareas.First().TallaUtillaje=="00"? Tareas.First().TallaEtiqueta: Tareas.First().TallaUtillaje;
                }
                else
                {
                    return "<SIN TAREA>";
                }
            }
        }

    }
}
