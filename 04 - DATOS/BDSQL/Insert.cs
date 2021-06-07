using Entidades.EntidadesBD;
using Entidades.EntidadesDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDSQL
{
    public static class Insert
    {
        public async static Task<int> InsertarConsumo(int idTarea, int pares, int idOperario, int idMaquina, bool finalizar)
        {
            using (SistemaGlobalPREEntities db = new SistemaGlobalPREEntities())
            {
                db.OrdenesFabricacionProductos.Add(new OrdenesFabricacionProductos
                {
                    Cantidad = (double)pares,
                    FechaCreacion = DateTime.Now,
                    IdMaquina = idMaquina,
                    IdOperario = idOperario,
                    Tipo = "PUESTO",
                    IdOrdenFabricacionOperacionTallaCantidad = idTarea,

                });

                if (finalizar)
                {
                    var tareadb = db.OrdenesFabricacionOperacionesTallasCantidad.Find(idTarea);
                    bool finalizado = tareadb.Finalizado ?? false;
                    if (!finalizado)
                    {
                        tareadb.Finalizado = true;
                        tareadb.IdEstado = 5;
                    }
                }

                return await db.SaveChangesAsync();
            }
        }

        private static MaquinasRegistrosDatos TranformarTareaAMaquinaRegistroDato(Tarea tarea, int idOperario, int idPuesto, int pares, bool piezaIntroducida, int productividad)
        {
            return new MaquinasRegistrosDatos
            {
                IdTarea = tarea.IdTarea,
                Fecha = DateTime.Now.ToUniversalTime(),
                Ciclo = 0,
                IpAutomata = "0",
                IdOperario = idOperario,
                Pares = pares,
                PiezaIntroducida = piezaIntroducida,
                CodigoEtiqueta = tarea.CodigoEtiqueta,
                CodigoOrden = tarea.CodigoOrden,
                IdOperacion = tarea.IdOperacion,
                IdOrden = tarea.IdOrden,
                PosicionMaquina = 0,
                Productividad = productividad,
                Talla = tarea.TallaEtiqueta,
                TallaUtillaje = tarea.TallaUtillaje,
                IdPuesto = idPuesto,
            };
        }

        public static List<MaquinasColasTrabajo> ActualizarColaTrabajo(string codigoBarquilla, List<int> idsTareas, int? agrupacion, int idMaquina, int idOperario, double cantidad,string talla)
        {
            List<MaquinasColasTrabajo> trabajosInsertar = new List<MaquinasColasTrabajo>();

            using (SistemaGlobalPREEntities db = new SistemaGlobalPREEntities())
            {
                if (idsTareas.Any())
                {
                    // recupero la cola de la maquina
                    var trabajos = db.MaquinasColasTrabajo
                        .Include("OrdenesFabricacionOperacionesTallasCantidad.OrdenesFabricacionOperacionesTallas.OrdenesFabricacionOperaciones.OrdenesFabricacion.Campos_ERP")
                        .Include("OrdenesFabricacionOperacionesTallasCantidad.OrdenesFabricacionProductos")
                        .Where(x => x.IdMaquina == idMaquina).ToList();

                    // elimino todos los trabajos que tengan que ver con la tarea que quiero ejecutar
                    trabajos.RemoveAll(x => idsTareas.Contains(x.IdTarea));

                    // elimino los trabajos que se encuentren en ejecución actualmente
                    trabajos.RemoveAll(x => x.Ejecucion);

                    // los ordeno por posicion
                    var trabajosOrdenados = trabajos.OrderBy(x => x.Posicion).ToList();

                    // actualizo sus posiciones en +1
                    int i = 1;
                    int anterior = 0;
                    foreach (var trabajo in trabajosOrdenados)
                    {
                        if (anterior == 0)
                        {
                            anterior = trabajo.Posicion;
                            trabajo.Posicion = i;
                        }
                        else
                        {
                            if (anterior == trabajo.Posicion)
                            {
                                trabajo.Posicion = i;
                            }
                            else
                            {
                                i++;
                                anterior = trabajo.Posicion;
                                trabajo.Posicion = i;
                            }
                        }
                    }

                    // busco el trabajo en ejecucion actual de la cola
                    MaquinasColasTrabajo trabajoEjecucionActual = db.MaquinasColasTrabajo.FirstOrDefault(x => x.IdMaquina == idMaquina && x.Ejecucion);
                    // si tiene trabajo en ejecución
                    if (trabajoEjecucionActual != null)
                    {
                        // lo desubico
                        var barquilla = db.Barquillas.FirstOrDefault(x => x.CodigoEtiqueta == trabajoEjecucionActual.CodigoEtiquetaFichada);
                        barquilla.CodUbicacion = null;
                    }

                    // elimino toda la cola
                    db.MaquinasColasTrabajo.RemoveRange(db.MaquinasColasTrabajo.Where(x => x.IdMaquina == idMaquina).ToList());


                    // inserto los trabajos antiguos con las posiciones actualizadas en +1
                    foreach (var trabajo in trabajosOrdenados)
                    {
                        trabajosInsertar.Add(new MaquinasColasTrabajo
                        {
                            TallaEtiquetaFichada = trabajo.TallaEtiquetaFichada,
                            CantidadEtiquetaFichada = trabajo.CantidadEtiquetaFichada,
                            Ejecucion = false,
                            Posicion = trabajo.Posicion + 1,
                            IdTarea = trabajo.IdTarea,
                            Agrupacion = trabajo.Agrupacion,
                            FechaProgramado = trabajo.FechaProgramado,
                            IdMaquina = idMaquina,
                            IdOperarioEjecuta = trabajo.IdOperarioEjecuta,
                            IdOperarioPrograma = trabajo.IdOperarioPrograma,
                            CodigoEtiquetaFichada = trabajo.CodigoEtiquetaFichada,
                            OrdenesFabricacionOperacionesTallasCantidad = trabajo.OrdenesFabricacionOperacionesTallasCantidad
                        });

                    }

                    // inserto la nueva tarea en ejecución
                    foreach (var id in idsTareas)
                    {
                        var tarea = db.OrdenesFabricacionOperacionesTallasCantidad
                            .Include("OrdenesFabricacionOperacionesTallas.OrdenesFabricacionOperaciones.OrdenesFabricacion.Campos_ERP")
                            .Include("OrdenesFabricacionProductos")
                            .FirstOrDefault(x => x.ID == id);
                        trabajosInsertar.Add(new MaquinasColasTrabajo
                        {
                            TallaEtiquetaFichada = talla,
                            CantidadEtiquetaFichada = cantidad,
                            IdMaquina = idMaquina,
                            IdOperarioEjecuta = idOperario,
                            IdTarea = id,
                            Posicion = 1,
                            Agrupacion = agrupacion ?? 0,
                            FechaProgramado = DateTime.Now,
                            Ejecucion = true,
                            CodigoEtiquetaFichada = codigoBarquilla,
                            OrdenesFabricacionOperacionesTallasCantidad = tarea,
                        });
                    }

                    db.MaquinasColasTrabajo.AddRange(trabajosInsertar);
                    db.SaveChanges();
                }

            }

            return trabajosInsertar;
        }

        public static List<MaquinasColasTrabajo> EliminarDeColaTrabajo(string codigoBarquilla, int idMaquina)
        {
            List<MaquinasColasTrabajo> trabajosInsertar = new List<MaquinasColasTrabajo>();

            using (SistemaGlobalPREEntities db = new SistemaGlobalPREEntities())
            {

                // recupero la cola de la maquina
                var trabajos = db.MaquinasColasTrabajo
                    .Include("OrdenesFabricacionOperacionesTallasCantidad.OrdenesFabricacionOperacionesTallas.OrdenesFabricacionOperaciones.OrdenesFabricacion.Campos_ERP")
                    .Include("OrdenesFabricacionOperacionesTallasCantidad.OrdenesFabricacionProductos")
                    .Where(x => x.IdMaquina == idMaquina).ToList();

                // elimino todos los trabajos que tengan que ver con la tarea que quiero ejecutar
                trabajos.RemoveAll(x =>x.CodigoEtiquetaFichada == codigoBarquilla);

                // elimino los trabajos que se encuentren en ejecución actualmente
                trabajos.RemoveAll(x => x.Ejecucion);

                // los ordeno por posicion
                var trabajosOrdenados = trabajos.OrderBy(x => x.Posicion).ToList();

                // actualizo sus posiciones en +1
                int i = 1;
                int anterior = 0;
                foreach (var trabajo in trabajosOrdenados)
                {
                    if (anterior == 0)
                    {
                        anterior = trabajo.Posicion;
                        trabajo.Posicion = i;
                    }
                    else
                    {
                        if (anterior == trabajo.Posicion)
                        {
                            trabajo.Posicion = i;
                        }
                        else
                        {
                            i++;
                            anterior = trabajo.Posicion;
                            trabajo.Posicion = i;
                        }
                    }
                }

                // busco el trabajo en ejecucion actual de la cola
                MaquinasColasTrabajo trabajoEjecucionActual = db.MaquinasColasTrabajo.FirstOrDefault(x => x.IdMaquina == idMaquina && x.Ejecucion);
                // si tiene trabajo en ejecución
                if (trabajoEjecucionActual != null)
                {
                    // lo desubico
                    var barquilla = db.Barquillas.FirstOrDefault(x => x.CodigoEtiqueta == trabajoEjecucionActual.CodigoEtiquetaFichada);
                    barquilla.CodUbicacion = null;
                }

                // elimino toda la cola
                db.MaquinasColasTrabajo.RemoveRange(db.MaquinasColasTrabajo.Where(x => x.IdMaquina == idMaquina).ToList());


                // inserto los trabajos antiguos con las posiciones actualizadas en +1
                foreach (var trabajo in trabajosOrdenados)
                {
                    trabajosInsertar.Add(new MaquinasColasTrabajo
                    {
                        CantidadEtiquetaFichada = trabajo.CantidadEtiquetaFichada,
                        Ejecucion = false,
                        Posicion = trabajo.Posicion,
                        IdTarea = trabajo.IdTarea,
                        Agrupacion = trabajo.Agrupacion,
                        FechaProgramado = trabajo.FechaProgramado,
                        IdMaquina = idMaquina,
                        IdOperarioEjecuta = trabajo.IdOperarioEjecuta,
                        IdOperarioPrograma = trabajo.IdOperarioPrograma,
                        CodigoEtiquetaFichada = trabajo.CodigoEtiquetaFichada,
                        OrdenesFabricacionOperacionesTallasCantidad = trabajo.OrdenesFabricacionOperacionesTallasCantidad
                    });

                }

                db.MaquinasColasTrabajo.AddRange(trabajosInsertar);
                db.SaveChanges();
            }

            return trabajosInsertar;
        }


        public static void InsertarPulso(Tarea tarea, int idOperario, int idPuesto, int pares)
        {
            using (SistemaGlobalPREEntities db = new SistemaGlobalPREEntities())
            {
                db.MaquinasRegistrosDatos.Add(TranformarTareaAMaquinaRegistroDato(tarea, idOperario, idPuesto, pares, true, 1));
                db.SaveChangesAsync();

            }
        }

        public static void InsertarSaldos(Tarea tarea, int idOperario, int idPuesto, int pares)
        {
            using (SistemaGlobalPREEntities db = new SistemaGlobalPREEntities())
            {
                db.MaquinasRegistrosDatos.Add(TranformarTareaAMaquinaRegistroDato(tarea, idOperario, idPuesto, pares, false, -1));
                db.SaveChangesAsync();

            }
        }

        public static void InsertarCorreccion(Tarea tarea, int idOperario, int idPuesto, int pares)
        {
            using (SistemaGlobalPREEntities db = new SistemaGlobalPREEntities())
            {
                db.MaquinasRegistrosDatos.Add(TranformarTareaAMaquinaRegistroDato(tarea, idOperario, idPuesto, pares, false, 1));
                db.SaveChangesAsync();

            }
        }

        public static void InsertarStocks(List<StockArticulos> stocks)
        {
            using (SistemaGlobalPREEntities db = new SistemaGlobalPREEntities())
            {
                db.StockArticulos.AddRange(stocks);
                db.SaveChanges();
            }
        }

        public static void ConsumirOperacionEnvasado(List<StockArticulos> stocks)
        {
            DateTime ahora = DateTime.Now;
            using (SistemaGlobalPREEntities db = new SistemaGlobalPREEntities())
            {
                var agrupadosPorOrden = stocks.GroupBy(x => x.IdOrdenFabricacion);
                foreach (var grupo in agrupadosPorOrden)
                {
                    foreach (var stock in grupo)
                    {
                        var orden = db.OrdenesFabricacion.FirstOrDefault(x => x.ID == stock.IdOrdenFabricacion);
                        if (orden != null)
                        {
                            var operacion = orden.OrdenesFabricacionOperaciones.FirstOrDefault(x => x.CodSeccion == "300");
                            if (operacion != null)
                            {
                                foreach (var sat in stock.StockArticulosTallas.Where(x => x.Cantidad > 0))
                                {
                                    var ofot = operacion.OrdenesFabricacionOperacionesTallas.FirstOrDefault(x => x.Tallas.Split(',').Contains(sat.Talla));
                                    if (ofot != null)
                                    {
                                        var tarea = ofot.OrdenesFabricacionOperacionesTallasCantidad.First();
                                        tarea.OrdenesFabricacionProductos.Add(new OrdenesFabricacionProductos
                                        {
                                            FechaCreacion = ahora,
                                            Cantidad = sat.Cantidad ?? 0,
                                            IdOperario = stock.IdOperarioCreacion,
                                            IdOrdenFabricacionOperacionTallaCantidad = tarea.ID,
                                        });
                                        if (tarea.CantidadFabricar <= tarea.OrdenesFabricacionProductos.Sum(x => x.Cantidad))
                                        {
                                            // finalizar
                                            tarea.Finalizado = true;
                                            tarea.IdEstadoAnterior = tarea.IdEstado;
                                            tarea.IdEstado = 5;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                db.SaveChanges();
            }
        }
    }
}
