using System;
using System.Collections.Generic;
using System.Text;

namespace Sodimac.Cedis.Core.DTOs
{

    //TODO: este request se puede modificar de acuerdo a los parametros de entrada
    //TODO: no olvidar modificar la entity
    public class ImpresoraDTO
    {
        public string ip { get; set; }
        public string nombre { get; set; }
        public string status { get; set; }
    }
}