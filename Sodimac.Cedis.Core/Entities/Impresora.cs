using System;
using System.Collections.Generic;
using System.Text;

namespace Sodimac.Cedis.Core.Entities
{
    public class Impresora
    {
        public string ip { get; set; }
        public string nombre { get; set; }
        public string status { get; set; }

        public Impresora() { }

        public Impresora(string ip, string nombre, string status)
        {
            this.ip = ip;
            this.nombre = nombre;
            this.status = status;
        }
    }

}
