using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ArnGestionPuestoFrontendWPF.EventosNavegacion
{
    public class NuevaPaginaEventArgs:EventArgs
    {
        public Page Pagina { get; set; }

        public NuevaPaginaEventArgs(Page pagina)
        {
            Pagina = pagina;
        }
    }
}
