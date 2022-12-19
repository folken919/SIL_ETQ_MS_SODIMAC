using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Sodimac.Cedis.Core.DTOs
{
    public class OlaPorEstadoDto
    {
        public string ola { get; set; }
        public string estado { get; set; }

        public OlaPorEstadoDto() { }

        public OlaPorEstadoDto(DataRow dr)
        {
            ola = dr["OLA"].ToString();
            estado = dr["ESTADO"].ToString();
        }
    }
}
