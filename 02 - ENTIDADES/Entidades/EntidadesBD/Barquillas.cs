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
    
    public partial class Barquillas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Barquillas()
        {
            this.BarquillasContenidos = new HashSet<BarquillasContenidos>();
        }
    
        public int Id { get; set; }
        public string CodigoEtiqueta { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public Nullable<int> NumeroAgrupacion { get; set; }
        public string CodUbicacion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BarquillasContenidos> BarquillasContenidos { get; set; }
        public virtual Ubicaciones Ubicaciones { get; set; }
    }
}
