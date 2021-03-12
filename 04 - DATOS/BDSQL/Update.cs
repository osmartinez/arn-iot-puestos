using Entidades.EntidadesBD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDSQL
{
    public static class Update
    {
        public static void UpdateContadorPaquetesBancada (Bancadas b,int contador)
        {
            using (SistemaGlobalPREEntities db = new SistemaGlobalPREEntities())
            {
                Bancadas bancadaBD = db.Bancadas.Find(b.ID);
                if (bancadaBD != null)
                {
                    bancadaBD.BancadasConfiguracionesPins.ContadorPaquetes = contador;
                    db.SaveChanges();
                    b.BancadasConfiguracionesPins.ContadorPaquetes = contador;
                }
            }
        }
    }
}
