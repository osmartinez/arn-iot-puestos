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
    
    public partial class Paquetes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Paquetes()
        {
            this.InventarioLecturas = new HashSet<InventarioLecturas>();
            this.OrdenesFabricacionPaquetesBorrar = new HashSet<OrdenesFabricacionPaquetesBorrar>();
            this.PaquetesConsumosInternos = new HashSet<PaquetesConsumosInternos>();
        }
    
        public long ID { get; set; }
        public string CodigoArticulo { get; set; }
        public short Tipo { get; set; }
        public string Partida { get; set; }
        public string Lote { get; set; }
        public decimal CantidadFabricacion { get; set; }
        public string UnidadMedidaFabricacion { get; set; }
        public Nullable<bool> Acabado { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public Nullable<System.DateTime> FechaBorrado { get; set; }
        public string UsuarioBorrado { get; set; }
        public string IdUltimoContenedorVinculado { get; set; }
        public Nullable<decimal> FactorConversionCompras { get; set; }
        public Nullable<decimal> CantidadCompras { get; set; }
        public string UnidadMedidaCompras { get; set; }
        public Nullable<decimal> FactorConversionVentas { get; set; }
        public Nullable<decimal> CantidadVentas { get; set; }
        public string UnidadMedidaVentas { get; set; }
        public string IDContenedor { get; set; }
        public Nullable<long> IDMercanciaRecepcion { get; set; }
        public Nullable<long> IDMercanciaSalida { get; set; }
        public Nullable<long> IDMercanciaDeposito { get; set; }
        public Nullable<System.DateTime> FechaRegistroInventario { get; set; }
        public Nullable<decimal> CantidadComprasReal { get; set; }
        public Nullable<System.DateTime> FechaBaja { get; set; }
        public bool Regularizado { get; set; }
    
        public virtual Contenedores Contenedores { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InventarioLecturas> InventarioLecturas { get; set; }
        public virtual MercanciasDepositos MercanciasDepositos { get; set; }
        public virtual MercanciasRecepcion MercanciasRecepcion { get; set; }
        public virtual MercanciasSalidas MercanciasSalidas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrdenesFabricacionPaquetesBorrar> OrdenesFabricacionPaquetesBorrar { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PaquetesConsumosInternos> PaquetesConsumosInternos { get; set; }
    }
}
