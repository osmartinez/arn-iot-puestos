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
    
    public partial class Talleres
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Talleres()
        {
            this.Maquinas = new HashSet<Maquinas>();
            this.PreciosLaboralesSubcontratas = new HashSet<PreciosLaboralesSubcontratas>();
            this.TalleresPaquetesEnvios = new HashSet<TalleresPaquetesEnvios>();
            this.TalleresPaquetesRecepciones = new HashSet<TalleresPaquetesRecepciones>();
            this.Utillajes = new HashSet<Utillajes>();
        }
    
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string NIF { get; set; }
        public string Direccion { get; set; }
        public string Poblacion { get; set; }
        public string Telefono { get; set; }
        public string Contacto { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> FechaBorrado { get; set; }
        public string UsuarioBorrado { get; set; }
        public string Apodo { get; set; }
        public bool Habilitado { get; set; }
        public string CodUbicacion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Maquinas> Maquinas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PreciosLaboralesSubcontratas> PreciosLaboralesSubcontratas { get; set; }
        public virtual Ubicaciones Ubicaciones { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TalleresPaquetesEnvios> TalleresPaquetesEnvios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TalleresPaquetesRecepciones> TalleresPaquetesRecepciones { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Utillajes> Utillajes { get; set; }
    }
}
