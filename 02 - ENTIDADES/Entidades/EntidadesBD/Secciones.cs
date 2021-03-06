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
    
    public partial class Secciones
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Secciones()
        {
            this.ArticulosOperaciones = new HashSet<ArticulosOperaciones>();
            this.Maquinas = new HashSet<Maquinas>();
            this.Operaciones = new HashSet<Operaciones>();
            this.OrdenesFabricacionOperaciones = new HashSet<OrdenesFabricacionOperaciones>();
            this.Utillajes = new HashSet<Utillajes>();
            this.Ubicaciones = new HashSet<Ubicaciones>();
        }
    
        public string CodSeccion { get; set; }
        public string Nombre { get; set; }
        public Nullable<double> CosteTiempo { get; set; }
        public Nullable<double> CosteIndirecto { get; set; }
        public Nullable<double> CosteTiempoSubcontrata { get; set; }
        public Nullable<double> CosteIndirectoSubcontrata { get; set; }
        public Nullable<bool> EsMolde { get; set; }
        public Nullable<bool> EsCorte { get; set; }
        public Nullable<int> ParesPorDia { get; set; }
        public string Grupo { get; set; }
        public string CodigoEtiqueta { get; set; }
        public Nullable<int> DiasDesfase { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ArticulosOperaciones> ArticulosOperaciones { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Maquinas> Maquinas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Operaciones> Operaciones { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrdenesFabricacionOperaciones> OrdenesFabricacionOperaciones { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Utillajes> Utillajes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ubicaciones> Ubicaciones { get; set; }
    }
}
