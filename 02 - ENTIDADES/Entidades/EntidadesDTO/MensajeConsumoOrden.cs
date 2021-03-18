using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.EntidadesDTO
{
    public class MensajeConsumoOrden
    {
        public string CodigoOrden { get; set; }
        public int IdMaquina { get; set; }
        public string CodSeccion { get; set; }
        public int CantidadPaquete { get; set; }
    }
}
