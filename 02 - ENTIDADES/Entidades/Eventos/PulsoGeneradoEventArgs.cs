using Entidades.EntidadesBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Eventos
{
    public class PulsoGeneradoEventArgs:EventArgs
    {
        public int IdBancada{ get; private set; }

        public PulsoGeneradoEventArgs(int idBancada)
        {
            IdBancada = idBancada;
        }
    }
}
