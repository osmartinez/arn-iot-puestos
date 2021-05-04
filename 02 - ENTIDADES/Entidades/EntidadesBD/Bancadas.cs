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
    
    public partial class Bancadas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Bancadas()
        {
            this.Bancadas1 = new HashSet<Bancadas>();
            this.BancadasRegistrosOperarios = new HashSet<BancadasRegistrosOperarios>();
            this.Ciclos = new HashSet<Ciclos>();
            this.IncidenciasBancadasRegistros = new HashSet<IncidenciasBancadasRegistros>();
            this.Maquinas = new HashSet<Maquinas>();
            this.OrdenesFabricacionOperacionesTallasCantidad = new HashSet<OrdenesFabricacionOperacionesTallasCantidad>();
        }
    
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Nullable<bool> Activa { get; set; }
        public Nullable<int> IdHermano { get; set; }
        public bool EsMaster { get; set; }
        public bool EsManual { get; set; }
        public Nullable<decimal> TiempoDesplazamiento { get; set; }
        public Nullable<decimal> TiempoObjetivo { get; set; }
        public Nullable<decimal> PorcentajeDesviacion { get; set; }
        public string CICLO { get; set; }
        public Nullable<decimal> CorrectorBancada { get; set; }
        public Nullable<decimal> TiempoMaquina { get; set; }
        public Nullable<decimal> TiempoOperario { get; set; }
        public Nullable<double> Ritmo { get; set; }
        public Nullable<double> CicloSegundos { get; set; }
        public string Observaciones { get; set; }
        public string CodigoEtiqueta { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bancadas> Bancadas1 { get; set; }
        public virtual Bancadas Bancadas2 { get; set; }
        public virtual BancadasConfiguracionesPins BancadasConfiguracionesPins { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BancadasRegistrosOperarios> BancadasRegistrosOperarios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ciclos> Ciclos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IncidenciasBancadasRegistros> IncidenciasBancadasRegistros { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Maquinas> Maquinas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrdenesFabricacionOperacionesTallasCantidad> OrdenesFabricacionOperacionesTallasCantidad { get; set; }
        public virtual BancadasConfiguracionesIncidencias BancadasConfiguracionesIncidencias { get; set; }
    }
}
