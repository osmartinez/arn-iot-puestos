//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Entidades.EntidadesBD
{
    using System;
    using System.Collections.Generic;
    
    public partial class MaquinasConfiguracionesPins
    {
        public int IdMaquina { get; set; }
        public bool EsPulsoManual { get; set; }
        public double ProductoPorPulso { get; set; }
        public bool DescontarAutomaticamente { get; set; }
        public int DireccionPulso { get; set; }
    
        public virtual Maquinas Maquinas { get; set; }
    }
}
