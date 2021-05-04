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
        public static Operarios OperarioEjecucion
        {
            get
            {
                return Operarios.FirstOrDefault();
            }
        }
        public static bool HayAlgunaTarea
        {
            get
            {
                return Bancada != null && Bancada.Maquinas.Any(x => x.TrabajoEjecucion != null);
            }
        }
        public static Maquinas MaquinaPrincipal
        {
            get
            {
                if(Bancada != null)
                {
                    Maquinas maq = Bancada.Maquinas.FirstOrDefault(x => x.TrabajoEjecucion != null);
                    if(maq != null)
                    {
                        return maq;
                    }
                    else
                    {
                        return Bancada.Maquinas.FirstOrDefault();
                    }
                }
                else
                {
                    return Maquinas.Default;
                }
            }
        }
        public static Bancadas Bancada { get; set; }
        public static Bancadas BancadaEsclavo { get; set; }
        public static List<Operarios> Operarios { get; set; } = new List<Operarios>();
        public static List<OperacionesControles> Controles { get; set; } = new List<OperacionesControles>();

        // variables globales interfaz pero que van asociadas a las tareas que se 
        // están ejecutando actualmente
        public static List<PulsoMaquina> Correcciones { get; set; } = new List<PulsoMaquina>();
        public static List<PulsoMaquina> Saldos { get; set; } = new List<PulsoMaquina>();
        public static int Monton { get; set; } = 0;
    }
}
