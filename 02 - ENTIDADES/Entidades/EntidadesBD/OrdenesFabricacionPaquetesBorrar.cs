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
    
    public partial class OrdenesFabricacionPaquetesBorrar
    {
        public int IdOrdenFabricacionOperacion { get; set; }
        public int IdOrdenFabricacionOperacionesTallasCantidad { get; set; }
        public long IdPaquete { get; set; }
        public int TipoOperacion { get; set; }
    
        public virtual OrdenesFabricacionOperaciones OrdenesFabricacionOperaciones { get; set; }
        public virtual OrdenesFabricacionOperacionesTallasCantidad OrdenesFabricacionOperacionesTallasCantidad { get; set; }
        public virtual Paquetes Paquetes { get; set; }
    }
}