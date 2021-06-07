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
        public string Talla { get; set; }
        public string CodigoOrden { get; set; }
        public int IdTarea { get; set; }
        public string CodigoEtiqueta { get; set; }
        public OperacionesControles Control { get; set; }
        public int IdPuesto { get; set; }
        public int Pares { get; set; }
        public DateTime Fecha { get; set; }
        public int IdOperario { get; set; }
    }
}
