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
    
    public partial class SP_ObtenerCriteriosLiquidacion_Result
    {
        public int ID { get; set; }
        public System.DateTime FechaInicio { get; set; }
        public System.DateTime FechaFin { get; set; }
        public string Familia { get; set; }
        public string SubFamilia { get; set; }
        public string Cliente { get; set; }
        public string Articulo { get; set; }
        public int Agente1 { get; set; }
        public decimal ComisionAgente1 { get; set; }
        public Nullable<decimal> ComisionResponsable1 { get; set; }
        public Nullable<int> Agente2 { get; set; }
        public Nullable<decimal> ComisionAgente2 { get; set; }
        public Nullable<decimal> ComisionResponsable2 { get; set; }
        public int Restrictiva { get; set; }
        public string NombreCliente { get; set; }
        public string NombreAgente1 { get; set; }
        public string NombreAgente2 { get; set; }
    }
}
