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
    
    public partial class PaletBultos
    {
        public string Palet { get; set; }
        public string ReferenciaBulto { get; set; }
        public int NumeroBulto { get; set; }
        public System.DateTime FechaModificacion { get; set; }
        public Nullable<bool> ModificadoExcel { get; set; }
    
        public virtual Bultos Bultos { get; set; }
    }
}