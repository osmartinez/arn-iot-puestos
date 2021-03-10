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

        public static Operarios BuscarOperarioPorCodigo(string cod)
        {
            using (SistemaGlobalPREEntities db = new SistemaGlobalPREEntities())
            {
                return db.Operarios.FirstOrDefault(x => x.CodigoObrero.Contains(cod));
            }
        }
    }
}
