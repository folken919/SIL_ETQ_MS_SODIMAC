using Sodimac.Cedis.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sodimac.Cedis.Core.Entities
{
    public class Request
    {
        public string cedis { get; set; }
        public string area { get; set; }
        public string transportadora { get; set; }
        public string zona { get; set; }
        public string cantidad { get; set; }
        public string etiqueta { get; set; }
        public List<Impresora> impresoras { get; set; }
        public string lote { get; set; }
        public string nuevoLote { get; set; }
        public string ola { get; set; }
        public string olpn { get; set; }
        public int flagReimpresion { get; set; }
        public bool flagAsignacion { get; set; }
        public string usuario { get; set; }
    }
}
