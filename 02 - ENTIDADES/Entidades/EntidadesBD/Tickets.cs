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
    
    public partial class Tickets
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tickets()
        {
            this.TicketsCantidad = new HashSet<TicketsCantidad>();
        }
    
        public int Id { get; set; }
        public string CodigoEtiqueta { get; set; }
        public int Capas { get; set; }
        public bool Impreso { get; set; }
        public Nullable<System.DateTime> FechaConsumo { get; set; }
        public string UsuarioConsumo { get; set; }
        public bool Consumido { get; set; }
        public int IdTicketAgrupacion { get; set; }
        public int IdTicketTipo { get; set; }
    
        public virtual TicketsAgrupacion TicketsAgrupacion { get; set; }
        public virtual TicketTipos TicketTipos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TicketsCantidad> TicketsCantidad { get; set; }
    }
}