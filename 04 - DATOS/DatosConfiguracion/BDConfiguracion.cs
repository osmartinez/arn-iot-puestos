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
        private const string RUTA = "settings.json";

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
                config = new Configuracion();
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
                    config = new Configuracion();
                }
            }

            Guardar(config);

            return config;
             
        }
    }
}
