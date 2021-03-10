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
        public Maquinas Maquina { get; private set; }

        public PulsoGeneradoEventArgs(Maquinas maquina)
        {
            Maquina = maquina;
        }
    }
}
