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
    
    public partial class StockArticulosTallas
    {
        public long Id { get; set; }
        public long IdStockArticulo { get; set; }
        public Nullable<int> IdArticuloTalla { get; set; }
        public Nullable<double> Cantidad { get; set; }
        public Nullable<System.DateTime> FechaUltimoMovimiento { get; set; }
        public string Talla { get; set; }
    
        public virtual ArticulosTallas ArticulosTallas { get; set; }
        public virtual StockArticulos StockArticulos { get; set; }
    }
}
