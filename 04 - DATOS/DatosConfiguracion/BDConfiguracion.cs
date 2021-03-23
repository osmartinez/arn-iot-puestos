using Entidades.EntidadesConfiguracion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatosConfiguracion
{
    public static class BDConfiguracion
    {
        private static string RUTA = Path.Combine(Environment.GetFolderPath(
    Environment.SpecialFolder.ApplicationData), "settings.json");

        public static void Guardar(Configuracion config)
        {
            string json = JsonConvert.SerializeObject(config);
            File.WriteAllText(RUTA, json);
        }

        public static Configuracion Leer()
        {
            Configuracion config = null;
            if (!File.Exists(RUTA))
            {
                config = new Configuracion { IdBancada = 29 };
                Guardar(config);

            }
            else
            {
                try
                {
                    string texto = File.ReadAllText(RUTA);
                    config = JsonConvert.DeserializeObject<Configuracion>(texto);
                }
                catch(Exception ex)
                {
                    config = new Configuracion { IdBancada = 29 };
                    Guardar(config);

                }
            }


            return config;
             
        }
    }
}
