using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Sodimac.Cedis.Core.DTOs
{
    public class LoteDto
    {
        public string lote { get; set; }

        public LoteDto()
        {
        }
        public LoteDto(DataRow dr) 
        {
            lote = dr["ID_LOTE_IMPRESION"].ToString();
        }
    }

    
}
