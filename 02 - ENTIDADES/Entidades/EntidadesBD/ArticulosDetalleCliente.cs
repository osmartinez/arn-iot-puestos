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
    
    public partial class ArticulosDetalleCliente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ArticulosDetalleCliente()
        {
            this.Articulos = new HashSet<Articulos>();
        }
    
        public int Id { get; set; }
        public string SuDescripcion { get; set; }
        public string SuUnidadMedida { get; set; }
        public string SuReferencia { get; set; }
        public string SuDescripcionHaix { get; set; }
        public string SuColorHaix { get; set; }
        public string SuCodigoColorHaix { get; set; }
        public string SuNumeroMaterialHaix { get; set; }
        public string SuUnidadMedidaHaix { get; set; }
        public string SuDescripcionMaspica { get; set; }
        public string SuColorMaspica { get; set; }
        public string SuDescripcionElten { get; set; }
        public string SuCodigoEcco { get; set; }
        public string SuDescripcionEcco { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Articulos> Articulos { get; set; }
    }
}
