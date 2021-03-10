using ArnGestionPuestoFrontendWPF.EventosNavegacion;
using ArnGestionPuestoFrontendWPF.Paginas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ArnGestionPuestoFrontendWPF
{
    public static class NavegacionEventos
    {
        private static OperariosPagina paginaOperarios;
        private static MenuPrincipal menuPrincipal;
        private static TareaPagina paginaTarea;

        public static TareaPagina PaginaTarea
        {
            get
            {
                if (paginaTarea == null)
                {
                    paginaTarea = new TareaPagina();
                }

                return paginaTarea;
            }
        }

        public static OperariosPagina PaginaOperarios
        {
            get
            {
                if(paginaOperarios == null)
                {
                    paginaOperarios = new OperariosPagina();
                }

                return paginaOperarios;
            }
        }

        public static MenuPrincipal PaginaMenuPrincipal
        {
            get
            {
                if (menuPrincipal == null)
                {
                    menuPrincipal = new MenuPrincipal();
                }

                return menuPrincipal;
            }
        }
        public static event EventHandler<NuevaPaginaEventArgs> OnNuevaPagina;

        public static void CargarNuevaPagina(Page page)
        {
            if (OnNuevaPagina != null)
            {
                OnNuevaPagina(null, new NuevaPaginaEventArgs(page));
            }
        }
    }
}
