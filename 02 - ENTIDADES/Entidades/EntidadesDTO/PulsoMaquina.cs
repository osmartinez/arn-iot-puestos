using Entidades.EntidadesBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.EntidadesDTO
{
    public class PulsoMaquina
    {
        public int IdPuesto { get; set; }
        public int Pares { get; set; }
        public DateTime Fecha { get; set; }
        public int IdOperario { get; set; }
    }
}
