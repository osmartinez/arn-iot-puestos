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
    
    public partial class SP_ListadoConsumosInternos_Result
    {
        public long ID { get; set; }
        public decimal CantidadConsumida { get; set; }
        public string Observaciones { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string CodigoEtiqueta { get; set; }
        public string CodigoArticulo { get; set; }
        public string DescripcionArticulo { get; set; }
        public string Partida { get; set; }
        public string Lote { get; set; }
        public Nullable<decimal> CantidadCompras { get; set; }
        public decimal CantidadFabricacion { get; set; }
        public string UnidadMedidaCompras { get; set; }
        public string UnidadMedidaFabricacion { get; set; }
        public Nullable<decimal> FactorConversionCompras { get; set; }
        public Nullable<decimal> CantidadConsumidaCompras { get; set; }
        public string IdUltimoContenedorVinculado { get; set; }
        public Nullable<decimal> CantidadRetorno { get; set; }
        public Nullable<System.DateTime> FechaRetorno { get; set; }
    }
}
