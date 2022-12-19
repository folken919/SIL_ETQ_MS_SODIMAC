using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Sodimac.Cedis.Core.Entities
{
    public class HiloImpresion
    {
        public string ipImpresora { get; set; }
        public string nombreImpresora { get; set; }
        public DataTable etiquetas { get; set; }
        public int cantidadPaquetes { get; set; }
        public int restantesUltimoPaquete { get; set; }
    }
}
