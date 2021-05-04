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
        public static Bancadas Bancada { get; set; }
        public static Bancadas BancadaEsclavo { get; set; }
        public static List<Operarios> Operarios { get; set; } = new List<Operarios>();
        public static List<OperacionesControles> Controles { get; set; } = new List<OperacionesControles>();

        public static int Monton { get; set; } = 0;
    }
}
