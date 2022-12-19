using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Sodimac.Cedis.Infraestructure.Models
{
    public partial class TblEtqOla
    {
        public TblEtqOla()
        {
            TblEtqEtiqueta = new HashSet<TblEtqEtiqueta>();
        }

        public string WaveNbr { get; set; }
        public string TipoOla { get; set; }
        public string TipoEtiqueta { get; set; }
        public DateTime FechaWaveNbr { get; set; }
        public byte IdEstado { get; set; }
        public string UsrHostCreacion { get; set; }
        public string UsrDbCreacion { get; set; }
        public string UsrSoCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsrHostModificacion { get; set; }
        public string UsrDbModificacion { get; set; }
        public string UsrSoModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public virtual ICollection<TblEtqEtiqueta> TblEtqEtiqueta { get; set; }
    }
}
