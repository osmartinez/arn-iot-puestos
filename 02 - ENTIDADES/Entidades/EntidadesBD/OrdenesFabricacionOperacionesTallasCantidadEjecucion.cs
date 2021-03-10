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
    
    public partial class OrdenesFabricacionOperacionesTallasCantidadEjecucion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrdenesFabricacionOperacionesTallasCantidadEjecucion()
        {
            this.OrdenesFabricacionPaquetes = new HashSet<OrdenesFabricacionPaquetes>();
            this.OrdenesFabricacionPaquetesUtilizados = new HashSet<OrdenesFabricacionPaquetesUtilizados>();
        }
    
        public int ID { get; set; }
        public Nullable<int> IdOrdenFabricacionOperacionesTallasCantidad { get; set; }
        public Nullable<System.DateTime> FechaInicio { get; set; }
        public Nullable<System.DateTime> FechaFin { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }
        public Nullable<double> CantidadProducida { get; set; }
        public Nullable<double> CantidadDefectuosa { get; set; }
        public Nullable<int> IdMaquinaEjecucion { get; set; }
        public Nullable<System.DateTime> FechaPDA { get; set; }
    
        public virtual OrdenesFabricacionOperacionesTallasCantidad OrdenesFabricacionOperacionesTallasCantidad { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrdenesFabricacionPaquetes> OrdenesFabricacionPaquetes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrdenesFabricacionPaquetesUtilizados> OrdenesFabricacionPaquetesUtilizados { get; set; }
    }
}