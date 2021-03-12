using Entidades.EntidadesBD;
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
    }
}
