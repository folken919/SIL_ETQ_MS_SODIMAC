using System;
using System.Collections.Generic;
using System.Text;

namespace Sodimac.Cedis.Core.DTOs
{
    public class ResponseDTO
    {
        public string mensaje { get; set; }
        public bool ok { get; set; }
        public object resultado { get; set; }
    }
}
