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
    
    public partial class PedidoReferencia
    {
        public string Pedido { get; set; }
        public string Referencia { get; set; }
        public string SuModelo { get; set; }
    
        public virtual Referencias Referencias { get; set; }
    }
}
