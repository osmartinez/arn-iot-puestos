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
    
    public partial class SP_ObtenerTareasOrdenFabricacion_Result
    {
        public int ID { get; set; }
        public string OrdenFabricacion { get; set; }
        public Nullable<int> Prioridad { get; set; }
        public string Molde { get; set; }
        public string Seccion { get; set; }
        public string Talla { get; set; }
        public string Tallas { get; set; }
        public string Proceso { get; set; }
        public Nullable<double> Cantidad { get; set; }
        public string Maquina { get; set; }
        public string NumeroOperacion { get; set; }
        public Nullable<int> TipoProceso { get; set; }
        public string NumeroOperacionAnterior { get; set; }
        public string Estado { get; set; }
        public Nullable<System.DateTime> Inicio { get; set; }
    }
}
