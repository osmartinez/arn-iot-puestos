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
                    .Include("Maquinas.MaquinasColasTrabajo.OrdenesFabricacionOperacionesTallasCantidad.OrdenesFabricacionOperacionesTallas.OrdenesFabricacionOperaciones.OrdenesFabricacion.Campos_ERP")
                    .Include("Maquinas.MaquinasColasTrabajo.OrdenesFabricacionOperacionesTallasCantidad.OrdenesFabricacionProductos")
                    .FirstOrDefault(x => x.ID == id);
            }
        }
        public static List<MaquinasRegistrosDatos> HistoricoPaquetesOperario(string ipAutomata, int pos,DateTime fechaInicio, DateTime fechaFin)
        {
            fechaInicio = fechaInicio.ToUniversalTime();
            fechaFin = fechaFin.ToUniversalTime();
            using (SistemaGlobalPREEntities db = new SistemaGlobalPREEntities())
            {
                var registros =
                db.MaquinasRegistrosDatos
                    .Where(x => x.IpAutomata == ipAutomata && x.PosicionMaquina == pos &&
                (fechaInicio <= x.Fecha && x.Fecha <= fechaFin)).ToList();

                foreach (var registro in registros)
                {
                    if (registro.IdTarea != 0)
                    {
                        registro.OrdenesFabricacionOperacionesTallasCantidad = db.OrdenesFabricacionOperacionesTallasCantidad
                            .Include("OrdenesFabricacionOperacionesTallas.OrdenesFabricacionOperaciones.OrdenesFabricacion")
                            .FirstOrDefault(x => x.ID == registro.IdTarea);
                    }
                }

                return registros;
            }
        }
        public static List<SP_BarquillaBuscarInformacionEnSeccion_Result> BuscarTareasPorCodigoBarquilla(string codigo, List<Maquinas> maquinas)
        {
            using (SistemaGlobalPREEntities db = new SistemaGlobalPREEntities())
            {
                List<SP_BarquillaBuscarInformacionEnSeccion_Result> info = new List<SP_BarquillaBuscarInformacionEnSeccion_Result>();
                foreach (var maquina in maquinas)
                {
                    var _infos = db.SP_BarquillaBuscarInformacionEnSeccion(codigo, maquina.CodSeccion).ToList();
                    foreach (var _info in _infos)
                    {
                        _info.MaquinasEjecucion.Add(maquina);
                    }
                    info.AddRange(_infos);
                }
                return info;
            }
        }

        public static List<SP_BarquillaBuscarInformacionEnSeccion_Result> BuscarTareasPorCodigoBarquilla(string codEtiqueta, string codSeccion)
        {
            using (SistemaGlobalPREEntities db = new SistemaGlobalPREEntities())
            {
                return db.SP_BarquillaBuscarInformacionEnSeccion(codEtiqueta, codSeccion).ToList();
            }
        }

        public static List<SP_BarquillaBuscarInformacionEnSeccion_Result> BuscarTareasPorOfot(int idOfot,string talla)
        {
            using (SistemaGlobalPREEntities db = new SistemaGlobalPREEntities())
            {
                var ofot = db.OrdenesFabricacionOperacionesTallas.FirstOrDefault(x => x.ID == idOfot);
                var ofotc = ofot.OrdenesFabricacionOperacionesTallasCantidad.First();
                var campos =ofot.OrdenesFabricacionOperaciones.OrdenesFabricacion.Campos_ERP;
                return new List<SP_BarquillaBuscarInformacionEnSeccion_Result>() { new SP_BarquillaBuscarInformacionEnSeccion_Result
                {
                     CantidadFabricar = ofotc.CantidadFabricar.Value + ofotc.CantidadSaldos.Value,
                     Cantidad = ofotc.CantidadFabricar.Value + ofotc.CantidadSaldos.Value,
                     Agrupacion = 0,
                     CantidadFabricada = ofotc.OrdenesFabricacionProductos.Sum(x=>x.Cantidad),
                     Codigo = ofot.OrdenesFabricacionOperaciones.OrdenesFabricacion.Codigo,
                     CodigoAgrupacion = ofot.OrdenesFabricacionOperaciones.OrdenesFabricacion.Agrupacion,
                     CodigoArticulo = ofot.OrdenesFabricacionOperaciones.OrdenesFabricacion.CodigoArticulo,
                     CodigoEtiqueta = "",
                     CodUtillaje = ofot.OrdenesFabricacionOperaciones.CodUtillaje,
                     Descripcion = ofot.OrdenesFabricacionOperaciones.Descripcion,
                     DESCRIPCIONARTICULO = campos==null?"SIN DESC":campos.DESCRIPCIONARTICULO,
                     NOMBRECLI=campos==null?"ARNEPLANT S.L.":campos.NOMBRECLI,
                     IdOperacion = ofot.IdOrdenFabricacionOperacion,
                     IdOrden = ofot.OrdenesFabricacionOperaciones.IdOrdenFabricacion,
                     IdTarea = ofotc.ID,
                     IdUtillajeTalla = ofot.IdUtillajeTalla,
                     PedidoLinea = (campos==null?"0":campos.PEDIDO.ToString())+  "/"+(campos==null?"0":campos.LINEAPEDIDO.ToString()),
                     Talla = talla,
                     Tallas = ofot.Tallas,
                     Productividad = 1,
                }
                };
            }
        }

        /// <summary>
        /// busca el control de un tipo de maquina a partir de una operación
        /// </summary>
        /// <param name="idOfo"></param>
        /// <param name="idTipoMaquina"></param>
        /// <returns></returns>
        public static OperacionesControles BuscarControlOperacion(int idOfo, int idTipoMaquina)
        {
            using (SistemaGlobalPREEntities db = new SistemaGlobalPREEntities())
            {
                var ofo = db.OrdenesFabricacionOperaciones.Find(idOfo);
                if (ofo.IdOperacionMaestra == null)
                {
                    return OperacionesControles.Default;
                }
                var control = db.OperacionesControles.FirstOrDefault(x => x.IdOperacion == ofo.IdOperacionMaestra && x.IdTipoMaquina == idTipoMaquina);
                if (control == null)
                {
                    return OperacionesControles.Default;

                }
                else
                {
                    return control;
                }
            }
        }

        /// <summary>
        /// busca la lista de tareas en formato SP_BarquillaBuscarInformacionEnSeccion a partir de una ofot
        /// </summary>
        /// <param name="idOfot"></param>
        /// <param name="maquinas"></param>
        /// <returns></returns>
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
                    CodigoEtiqueta = tarea.OrdenesFabricacionOperacionesTallas.OrdenesFabricacionOperaciones.ID.ToString().PadLeft(13, '0'),
                    Talla = tarea.OrdenesFabricacionOperacionesTallas.Tallas,
                    Cantidad = tarea.CantidadFabricar.Value + tarea.CantidadSaldos.Value,
                    CodUtillaje = tarea.OrdenesFabricacionOperacionesTallas.OrdenesFabricacionOperaciones.CodUtillaje,
                    Descripcion = tarea.OrdenesFabricacionOperacionesTallas.OrdenesFabricacionOperaciones.Descripcion,
                    IdUtillajeTalla = tarea.OrdenesFabricacionOperacionesTallas.IdUtillajeTalla,
                    Tallas = tarea.OrdenesFabricacionOperacionesTallas.Tallas,
                    CantidadFabricar = tarea.CantidadFabricar.Value + tarea.CantidadSaldos.Value,
                    CantidadFabricada = tarea.OrdenesFabricacionProductos.Sum(x => x.Cantidad),
                    PedidoLinea = orden.Campos_ERP.PEDIDO + "/" + orden.Campos_ERP.LINEAPEDIDO,
                    IdOperacion = tarea.OrdenesFabricacionOperacionesTallas.OrdenesFabricacionOperaciones.ID,
                    IdTarea = tarea.ID,
                    Productividad = 1,
                    Agrupacion = orden.Agrupacion,
                    MaquinasEjecucion = maquinas,
                });
                return info;
            }
        }

        /// <summary>
        /// a partir de una id de operacion, encuentra las ofot
        /// </summary>
        /// <param name="idOperacion"></param>
        /// <returns></returns>
        public static List<OrdenesFabricacionOperacionesTallas> ObtenerOperacionesTallasOperacion(int idOperacion)
        {
            using (SistemaGlobalPREEntities db = new SistemaGlobalPREEntities())
            {
                return db.OrdenesFabricacionOperacionesTallas.Where(x => x.IdOrdenFabricacionOperacion == idOperacion).ToList();
            }
        }

        /// <summary>
        /// busca un operario por código de obrero
        /// </summary>
        /// <param name="cod"></param>
        /// <returns></returns>
        public static Operarios BuscarOperarioPorCodigo(string cod)
        {
            using (SistemaGlobalPREEntities db = new SistemaGlobalPREEntities())
            {
                return db.Operarios.FirstOrDefault(x => x.CodigoObrero.Contains(cod));
            }
        }
    }
}
