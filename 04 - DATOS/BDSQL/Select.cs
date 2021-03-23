using Entidades.EntidadesBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDSQL
{
    public static class Select
    {
        public static Bancadas ObtenerBancadaPorId(int id)
        {
            using (SistemaGlobalPREEntities db = new SistemaGlobalPREEntities())
            {
                return db.Bancadas
                    .Include("BancadasConfiguracionesIncidencias")
                    .Include("BancadasConfiguracionesPins")
                    .Include("Maquinas.MaquinasConfiguracionesPins")
                    .FirstOrDefault(x=>x.ID == id);
            }
        }

        public static List<SP_BarquillaBuscarInformacionEnSeccion_Result> BuscarTareasPorCodigoBarquilla(string codigo,List<Maquinas> maquinas)
        {
            using (SistemaGlobalPREEntities db = new SistemaGlobalPREEntities())
            {
                List<SP_BarquillaBuscarInformacionEnSeccion_Result> info = new List<SP_BarquillaBuscarInformacionEnSeccion_Result>();
                foreach(var maquina in maquinas)
                {
                    var _infos = db.SP_BarquillaBuscarInformacionEnSeccion(codigo, maquina.CodSeccion).ToList();
                    foreach(var _info in _infos)
                    {
                        _info.MaquinasEjecucion.Add(maquina);
                    }
                    info.AddRange(_infos);
                }
                return info;
            }
        }

        public static List<SP_BarquillaBuscarInformacionEnSeccion_Result> BuscarTareasPorOfot(int idOfot, List<Maquinas> maquinas)
        {
            using (SistemaGlobalPREEntities db = new SistemaGlobalPREEntities())
            {
                var tarea = db.OrdenesFabricacionOperacionesTallasCantidad.FirstOrDefault(x => x.IdOrdenFabricacionOperacionesTallas == idOfot);
                List<SP_BarquillaBuscarInformacionEnSeccion_Result> info = new List<SP_BarquillaBuscarInformacionEnSeccion_Result>();
                var orden = tarea.OrdenesFabricacionOperacionesTallas.OrdenesFabricacionOperaciones.OrdenesFabricacion;
                info.Add(new SP_BarquillaBuscarInformacionEnSeccion_Result
                {
                    Codigo = orden.Codigo,
                    IdOrden = orden.ID,
                    CodigoArticulo = orden.CodigoArticulo,
                    DESCRIPCIONARTICULO = orden.Campos_ERP.DESCRIPCIONARTICULO,
                    NOMBRECLI = orden.Campos_ERP.NOMBRECLI,
                    CodigoAgrupacion = orden.Agrupacion,
                    CodigoEtiqueta = tarea.OrdenesFabricacionOperacionesTallas.OrdenesFabricacionOperaciones.ID.ToString().PadLeft(13,'0'),
                    Talla = tarea.OrdenesFabricacionOperacionesTallas.Tallas,
                    Cantidad = tarea.CantidadFabricar.Value+tarea.CantidadSaldos.Value,
                    CodUtillaje = tarea.OrdenesFabricacionOperacionesTallas.OrdenesFabricacionOperaciones.CodUtillaje,
                    Descripcion = tarea.OrdenesFabricacionOperacionesTallas.OrdenesFabricacionOperaciones.Descripcion,
                    IdUtillajeTalla = tarea.OrdenesFabricacionOperacionesTallas.IdUtillajeTalla,
                    Tallas = tarea.OrdenesFabricacionOperacionesTallas.Tallas,
                    CantidadFabricar = tarea.CantidadFabricar.Value+tarea.CantidadSaldos.Value,
                    CantidadFabricada =  tarea.OrdenesFabricacionProductos.Sum(x=>x.Cantidad),
                    PedidoLinea = orden.Campos_ERP.PEDIDO+"/"+orden.Campos_ERP.LINEAPEDIDO,
                    IdOperacion = tarea.OrdenesFabricacionOperacionesTallas.OrdenesFabricacionOperaciones.ID,
                    IdTarea = tarea.ID,
                    Productividad = 1,
                    Agrupacion = orden.Agrupacion,
                    MaquinasEjecucion = maquinas,
                });
                return info;
            }
        }

        public static List<OrdenesFabricacionOperacionesTallas> ObtenerOperacionesTallasOperacion(int idOperacion)
        {
            using (SistemaGlobalPREEntities db = new SistemaGlobalPREEntities())
            {
                return db.OrdenesFabricacionOperacionesTallas.Where(x => x.IdOrdenFabricacionOperacion == idOperacion).ToList();
            }
        }

        public static Operarios BuscarOperarioPorCodigo(string cod)
        {
            using (SistemaGlobalPREEntities db = new SistemaGlobalPREEntities())
            {
                return db.Operarios.FirstOrDefault(x => x.CodigoObrero.Contains(cod));
            }
        }
    }
}
