using Sodimac.Cedis.Core.DTOs;
using Sodimac.Cedis.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Sodimac.Cedis.Core.Interfaces.Repository
{
    public interface IImpresionEtiquetaRepository
    {
        public DataTable ObtenerZplsEtiquetasImprimirMasivo(RequestImp imprimir, string etiquetas);

        public DataTable ObtenerZplsEtiquetasImprimirMasivoXml(XmlEtq imprimir, string tag);

        public int ConsultarTope(string idEtiqueta);
    }
}
