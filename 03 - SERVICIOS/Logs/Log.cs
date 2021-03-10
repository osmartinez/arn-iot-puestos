using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logs
{
    public class Log
    {
        private const string RUTA = "errores.log";

        public static void Write(Exception ex)
        {
            try
            {
                File.AppendAllText(RUTA, string.Format("\n\n[ERROR {0}] {1} - {2}", DateTime.Now.ToString(), ex.Message, ex.StackTrace));
            }
            catch (Exception)
            {

            }
        }
    }
}
