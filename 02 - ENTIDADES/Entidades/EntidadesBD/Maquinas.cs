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
    
    public partial class Maquinas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Maquinas()
        {
            this.MaquinasColasTrabajo = new HashSet<MaquinasColasTrabajo>();
            this.MaquinasHistoricoCiclos = new HashSet<MaquinasHistoricoCiclos>();
            this.MaquinasParadas = new HashSet<MaquinasParadas>();
            this.OrdenesFabricacionOperacionesTallasCantidad = new HashSet<OrdenesFabricacionOperacionesTallasCantidad>();
            this.OrdenesFabricacionPaquetes = new HashSet<OrdenesFabricacionPaquetes>();
            this.OrdenesFabricacionProductos = new HashSet<OrdenesFabricacionProductos>();
            this.PrepaquetesConsumidos = new HashSet<PrepaquetesConsumidos>();
        }
    
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string CodigoAgrupacion { get; set; }
        public string CodSeccion { get; set; }
        public Nullable<int> IDPlantillaCalendario { get; set; }
        public Nullable<System.DateTime> PlantillaCalendarioDesde { get; set; }
        public Nullable<bool> Averia { get; set; }
        public Nullable<int> IdIncidencia { get; set; }
        public Nullable<bool> IncluirTiempoPreparacion { get; set; }
        public Nullable<int> IdTaller { get; set; }
        public Nullable<int> TallerHorasCapacidadDiaria { get; set; }
        public Nullable<System.DateTime> FechaBorrado { get; set; }
        public string UsuarioBorrado { get; set; }
        public Nullable<double> CorrectorCapacidad { get; set; }
        public string CodUbicacion { get; set; }
        public string CodigoEtiqueta { get; set; }
        public Nullable<int> IdBancada { get; set; }
        public string IpAutomata { get; set; }
        public int Posicion { get; set; }
        public double UITopMargin { get; set; }
        public double UILeftMargin { get; set; }
        public Nullable<int> PosicionGlobal { get; set; }
        public Nullable<int> IdTipo { get; set; }
    
        public virtual Bancadas Bancadas { get; set; }
        public virtual CalendarioPlantillas CalendarioPlantillas { get; set; }
        public virtual Incidencias Incidencias { get; set; }
        public virtual MaquinasTipos MaquinasTipos { get; set; }
        public virtual Secciones Secciones { get; set; }
        public virtual Talleres Talleres { get; set; }
        public virtual Ubicaciones Ubicaciones { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MaquinasColasTrabajo> MaquinasColasTrabajo { get; set; }
        public virtual MaquinasConfiguracionesPins MaquinasConfiguracionesPins { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MaquinasHistoricoCiclos> MaquinasHistoricoCiclos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MaquinasParadas> MaquinasParadas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrdenesFabricacionOperacionesTallasCantidad> OrdenesFabricacionOperacionesTallasCantidad { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrdenesFabricacionPaquetes> OrdenesFabricacionPaquetes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrdenesFabricacionProductos> OrdenesFabricacionProductos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PrepaquetesConsumidos> PrepaquetesConsumidos { get; set; }
    }
}
