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
    
    public partial class Impresoras
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Impresoras()
        {
            this.UsuariosAD = new HashSet<UsuariosAD>();
        }
    
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string IP { get; set; }
        public string COM { get; set; }
        public Nullable<int> DPI { get; set; }
        public Nullable<int> BaudRate { get; set; }
        public Nullable<int> DataBits { get; set; }
        public Nullable<int> Parity { get; set; }
        public Nullable<int> StopBits { get; set; }
        public Nullable<int> Handshake { get; set; }
        public Nullable<bool> PrioridadIP { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsuariosAD> UsuariosAD { get; set; }
    }
}