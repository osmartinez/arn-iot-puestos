using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.EntidadesBD
{
    public partial class SP_BarquillaBuscarInformacionEnSeccion_Result
    {
        public List<Maquinas> MaquinasEjecucion { get; set; } = new List<Maquinas>();
    }
}
