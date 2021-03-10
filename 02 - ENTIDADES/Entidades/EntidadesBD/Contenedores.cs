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
    
    public partial class Contenedores
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Contenedores()
        {
            this.EspaciosVagon = new HashSet<EspaciosVagon>();
            this.InventarioLecturas = new HashSet<InventarioLecturas>();
            this.OrdenesFabricacionPaquetes = new HashSet<OrdenesFabricacionPaquetes>();
            this.Paquetes = new HashSet<Paquetes>();
            this.StockArticulos = new HashSet<StockArticulos>();
            this.UtillajesTallasColeccion = new HashSet<UtillajesTallasColeccion>();
        }
    
        public string CodigoEtiqueta { get; set; }
        public Nullable<short> Tipo { get; set; }
        public string CodUbicacion { get; set; }
        public Nullable<System.DateTime> FechaAsociacion { get; set; }
        public string AgrupacionCodigo { get; set; }
        public Nullable<short> AgrupacionPosicionContenedor { get; set; }
        public Nullable<System.DateTime> AgrupacionFecha { get; set; }
    
        public virtual Ubicaciones Ubicaciones { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EspaciosVagon> EspaciosVagon { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InventarioLecturas> InventarioLecturas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrdenesFabricacionPaquetes> OrdenesFabricacionPaquetes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Paquetes> Paquetes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockArticulos> StockArticulos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UtillajesTallasColeccion> UtillajesTallasColeccion { get; set; }
    }
}
