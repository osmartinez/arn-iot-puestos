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
    
    public partial class SP_BancadaActualizar_Result
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public Nullable<bool> Activa { get; set; }
        public Nullable<decimal> TiempoDesplazamiento { get; set; }
        public Nullable<decimal> TiempoObjetivo { get; set; }
        public Nullable<decimal> PorcentajeDesviacion { get; set; }
        public string CICLO { get; set; }
        public Nullable<decimal> CorrectorBancada { get; set; }
        public Nullable<decimal> TiempoMaquina { get; set; }
        public Nullable<decimal> TiempoOperario { get; set; }
        public Nullable<double> Ritmo { get; set; }
        public Nullable<double> CicloSegundos { get; set; }
        public string Observaciones { get; set; }
        public string Descripcion { get; set; }
        public bool EsManual { get; set; }
        public string CodigoEtiqueta { get; set; }
        public int IdBancada { get; set; }
        public string PinBuzzer { get; set; }
        public string PinLed { get; set; }
        public double ContadorPaquetes { get; set; }
        public bool EsContadorPaquetesAutomatico { get; set; }
        public bool AvisarFinPaquete { get; set; }
        public bool AvisarFinTarea { get; set; }
    }
}
