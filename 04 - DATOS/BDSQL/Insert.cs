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
        public static void InsertarConsumo(int idTarea, int pares, int idOperario, int idMaquina)
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
                db.SaveChangesAsync();
            }
        }

        private static MaquinasRegistrosDatos TranformarTareaAMaquinaRegistroDato(Tarea tarea, int idOperario, int pares, bool piezaIntroducida, int productividad)
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
                TallaUtillaje = tarea.TallaUtillaje
            };
        }

        public static void InsertarPulso(Tarea tarea, int idOperario, int pares)
        {
            using (SistemaGlobalPREEntities db = new SistemaGlobalPREEntities())
            {
                db.MaquinasRegistrosDatos.Add(TranformarTareaAMaquinaRegistroDato(tarea, idOperario, pares, true, 1));
                db.SaveChangesAsync();

            }
        }

        public static void InsertarSaldos(Tarea tarea, int idOperario, int pares)
        {
            using (SistemaGlobalPREEntities db = new SistemaGlobalPREEntities())
            {
                db.MaquinasRegistrosDatos.Add(TranformarTareaAMaquinaRegistroDato(tarea, idOperario, pares, false, -1));
                db.SaveChangesAsync();

            }
        }

        public static void InsertarCorreccion(Tarea tarea, int idOperario, int pares)
        {
            using (SistemaGlobalPREEntities db = new SistemaGlobalPREEntities())
            {
                db.MaquinasRegistrosDatos.Add(TranformarTareaAMaquinaRegistroDato(tarea, idOperario, pares, false, 1));
                db.SaveChangesAsync();

            }
        }
    }
}
