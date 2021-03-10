using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Eventos
{
    public class BarquillaFichadaEventArgs: EventArgs
    {
        public string CodigoEtiqueta { get; private set; }

        public BarquillaFichadaEventArgs(string codigoEtiqueta)
        {
            CodigoEtiqueta = codigoEtiqueta;
        }
    }
}
