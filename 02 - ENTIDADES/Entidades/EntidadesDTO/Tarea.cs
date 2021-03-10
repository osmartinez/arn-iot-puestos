using Entidades.EntidadesBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.EntidadesDTO
{
    public class Tarea
    {
        public string CodigoOrden { get; set; }
        public int IdOrden { get; set; }
        public string CodigoArticulo { get; set; }
        public string Modelo { get; set; }
        public string NombreCliente { get; set; }
        public string CodigoEtiqueta { get; set; }
        public string TallaEtiqueta { get; set; }
        public double CantidadEtiqueta { get; set; }
        public string CodigoUtillaje { get; set; }
        public string DescripcionOperacion { get; set; }
        public string TallaUtillaje { get; set; }
        public string TallasArticulo { get; set; }
        public double CantidadFabricar { get; set; }
        public double CantidadFabricada { get; set; }
        public string PedidoLinea { get; set; }
        public int IdOperacion { get; set; }
        public int IdTarea { get; set; }
        public double Productividad { get; set; }
        public int Agrupacion { get; set; }
        public List<Maquinas> MaquinasEjecucion { get; private set; } = new List<Maquinas>();

        public List<PulsoMaquina> Pulsos { get; private set; } = new List<PulsoMaquina>();
        public List<PulsoMaquina> Saldos { get; private set; } = new List<PulsoMaquina>();
        public List<PulsoMaquina> Correcciones { get; private set; } = new List<PulsoMaquina>();


        public Tarea(SP_BarquillaBuscarInformacionEnSeccion_Result sp)
        {
            this.CodigoOrden = sp.Codigo;
            this.IdOrden = sp.IdOrden;
            this.CodigoArticulo = sp.CodigoArticulo;
            this.Modelo = sp.DESCRIPCIONARTICULO;
            this.NombreCliente = sp.NOMBRECLI;
            this.CodigoEtiqueta = sp.CodigoEtiqueta;
            this.TallaEtiqueta = sp.Talla;
            this.CantidadEtiqueta = sp.Cantidad;
            this.CodigoUtillaje = sp.CodUtillaje;
            this.DescripcionOperacion = sp.Descripcion;
            this.TallaUtillaje = sp.IdUtillajeTalla;
            this.TallasArticulo = sp.Tallas;
            this.CantidadFabricada = sp.CantidadFabricada??0;
            this.CantidadFabricar = sp.CantidadFabricar??0;
            this.PedidoLinea = sp.PedidoLinea;
            this.IdOperacion = sp.IdOperacion;
            this.IdTarea = sp.IdTarea ?? 0;
            this.Productividad = sp.Productividad;
            this.Agrupacion = sp.Agrupacion??0;
            this.MaquinasEjecucion = sp.MaquinasEjecucion.ToList();
        }

        public Tarea(string codigoOrden, int idOrden, string codigoArticulo, string modelo, string nombreCliente, string codigoEtiqueta, string tallaEtiqueta, double cantidadEtiqueta, string codigoUtillaje, string descripcionOperacion, string tallaUtillaje, string tallasArticulo, double cantidadFabricar, double cantidadFabricada, string pedidoLinea, int idOperacion, int idTarea, double productividad, int agrupacion)
        {
            CodigoOrden = codigoOrden;
            IdOrden = idOrden;
            CodigoArticulo = codigoArticulo;
            Modelo = modelo;
            NombreCliente = nombreCliente;
            CodigoEtiqueta = codigoEtiqueta;
            TallaEtiqueta = tallaEtiqueta;
            CantidadEtiqueta = cantidadEtiqueta;
            CodigoUtillaje = codigoUtillaje;
            DescripcionOperacion = descripcionOperacion;
            TallaUtillaje = tallaUtillaje;
            TallasArticulo = tallasArticulo;
            CantidadFabricar = cantidadFabricar;
            CantidadFabricada = cantidadFabricada;
            PedidoLinea = pedidoLinea;
            IdOperacion = idOperacion;
            IdTarea = idTarea;
            Productividad = productividad;
            Agrupacion = agrupacion;
        }
    }
}
