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
    
    public partial class SP_BuscarImpresionMarcajePorUtillajeTalla_Result
    {
        public int ID { get; set; }
        public string CodUtillaje { get; set; }
        public string TallaUtillaje { get; set; }
        public Nullable<double> Productividad { get; set; }
        public Nullable<int> DimensionX { get; set; }
        public Nullable<int> DimensionY { get; set; }
        public Nullable<int> Posicion { get; set; }
        public Nullable<int> TotalComponentes { get; set; }
        public string ImpresionMarcaje1 { get; set; }
        public string ImpresionMarcaje2 { get; set; }
        public string ImpresionMarcaje3 { get; set; }
        public string FicheroImpresionMarcaje { get; set; }
    }
}