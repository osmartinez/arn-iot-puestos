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
    
    public partial class AUDITAR_UBICACIONES
    {
        public int Id { get; set; }
        public string CodigoEtiqueta { get; set; }
        public System.DateTime FechaCambio { get; set; }
        public string AgrupacionAnterior { get; set; }
        public string AgrupacionNueva { get; set; }
        public string CodUbicacionAnterior { get; set; }
        public string CodUbicacionNueva { get; set; }
    }
}
