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
    
    public partial class SP_ListarCambiosMolde_Result
    {
        public string CodUtillaje { get; set; }
        public string IdUtillajeTalla { get; set; }
        public string Codigo { get; set; }
        public string NombreCliente { get; set; }
        public string Descripcion { get; set; }
        public string Tallas { get; set; }
        public int ID { get; set; }
        public Nullable<double> CantidadFabricar { get; set; }
        public int Posicion { get; set; }
        public Nullable<int> IdMaquina { get; set; }
        public int IdMaquinaEjecucion { get; set; }
        public string MaquinaPlan { get; set; }
        public string MaquinaEjec { get; set; }
        public Nullable<double> MaquinaPlanCiclo { get; set; }
        public double MaquinaEjecCiclo { get; set; }
        public Nullable<double> CantidadFabricada { get; set; }
    }
}
