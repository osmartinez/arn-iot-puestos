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
    
    public partial class OrdenesFabricacionOperacionesTallasCantidadPlanificacion
    {
        public int ID { get; set; }
        public Nullable<int> IdTarea { get; set; }
        public Nullable<int> UnidadesCorte { get; set; }
        public Nullable<System.DateTime> FechaInicio { get; set; }
        public Nullable<System.DateTime> FechaFinal { get; set; }
        public Nullable<int> Tipo { get; set; }
        public Nullable<decimal> TiempoCorte { get; set; }
        public Nullable<bool> Calculado { get; set; }
    
        public virtual OrdenesFabricacionOperacionesTallasCantidad OrdenesFabricacionOperacionesTallasCantidad { get; set; }
    }
}
