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
    
    public partial class SP_MaquinaVerColaTrabajoPorCodigo_Result
    {
        public string CodigoOrden { get; set; }
        public string NombreCliente { get; set; }
        public int Agrupacion { get; set; }
        public int Id { get; set; }
        public int Posicion { get; set; }
        public bool Ejecucion { get; set; }
        public Nullable<double> CantidadFabricar { get; set; }
        public int IdMaquina { get; set; }
        public int IdTarea { get; set; }
        public Nullable<int> IdOperarioEjecuta { get; set; }
        public Nullable<int> IdOperarioPrograma { get; set; }
        public string TallaUtillaje { get; set; }
        public string Utillaje { get; set; }
        public string Modelo { get; set; }
    }
}
