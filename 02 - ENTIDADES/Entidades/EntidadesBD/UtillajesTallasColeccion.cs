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
    
    public partial class UtillajesTallasColeccion
    {
        public int ID { get; set; }
        public int IdUtillajeTalla { get; set; }
        public string CodigoEtiqueta { get; set; }
        public int Activos { get; set; }
        public Nullable<int> Estado { get; set; }
        public string Componentes { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; }
        public string IdContenedor { get; set; }
        public string CodUbicacion { get; set; }
        public Nullable<System.DateTime> FechaAsociacion { get; set; }
    
        public virtual Contenedores Contenedores { get; set; }
        public virtual Ubicaciones Ubicaciones { get; set; }
        public virtual UtillajesTallas UtillajesTallas { get; set; }
    }
}
