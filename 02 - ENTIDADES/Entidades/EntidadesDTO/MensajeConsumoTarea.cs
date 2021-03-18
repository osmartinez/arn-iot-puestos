using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.EntidadesDTO
{
    public class MensajeConsumoTarea
    {
        public int IdPuesto { get; set; }
        public int IdTarea { get; set; }
        public int ParesConsumidos { get; set; }
        public bool PiezaIntroducida { get; set; }
    }
}
