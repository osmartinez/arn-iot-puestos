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
    
    public partial class PrepaquetesConsumidos
    {
        public int Id { get; set; }
        public string CodPrepaquete { get; set; }
        public int IdOrdenFabricacionOperacion { get; set; }
        public Nullable<System.DateTime> FechaConsumido { get; set; }
        public Nullable<int> IdMaquina { get; set; }
        public Nullable<int> IdOperario { get; set; }
    
        public virtual Maquinas Maquinas { get; set; }
        public virtual Operarios Operarios { get; set; }
        public virtual OrdenesFabricacionOperaciones OrdenesFabricacionOperaciones { get; set; }
        public virtual PrePaquetes PrePaquetes { get; set; }
    }
}