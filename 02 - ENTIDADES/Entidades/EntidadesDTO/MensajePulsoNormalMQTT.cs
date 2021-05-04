using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.EntidadesDTO
{
    public class MensajePulsoNormalMQTT
    {
        public int Posicion { get; set; }
        public int Tipo { get; set; }
        public int IdTarea { get; set; }
        public int IdOF { get; set; }
        public int IdOperacion { get; set; }
        public string CodigoOF { get; set; }
        public string CodigoBarras { get; set; }
        public int ParesUtillaje { get; set; }
        public int NumUtillajes { get; set; }
        public int PiezaIntroducida { get; set; }
        public string Utillaje { get; set; }
        public string TallaUtillaje { get; set; }
        public string TallaArticulo { get; set; }
        public string NombreCliente { get; set; }
        public string CodigoArticulo { get; set; }
        public int ParesTarea { get; set; }
        public int IdObrero { get; set; }
        public string Hora { get; set; }

    }
}
