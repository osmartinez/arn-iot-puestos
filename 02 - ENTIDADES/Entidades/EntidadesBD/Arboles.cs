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
    
    public partial class Arboles
    {
        public int ID { get; set; }
        public string PrefijoID { get; set; }
        public int CodArbol { get; set; }
        public string Padre { get; set; }
        public string Text { get; set; }
        public Nullable<int> ImageIndex { get; set; }
        public Nullable<int> SelectedImageIndex { get; set; }
        public string Tag { get; set; }
    }
}