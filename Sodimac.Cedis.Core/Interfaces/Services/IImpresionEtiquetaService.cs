using Sodimac.Cedis.Core.DTOs;
using Sodimac.Cedis.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sodimac.Cedis.Core.Interfaces.Services
{
    public interface IImpresionEtiquetaService
    {
        int ImprimirEtiquetasMasivo(RequestImp imprimirDTO);
        int ImprimirEtiquetas(Request imprimirDTO);
        Response ConsultaLoteEtq();
        Response CancelarImprimirEtq(LoteImpresion lote);
        bool ValidaOlasHabilitadas(string ola);
        int ImprimirEtqMasivoXml(XmlEtq imprimir);
        int ImprimirEtqMasivoPrincipal(XmlEtq imprimir);
        Response ValidaOlasHabilitadasMasivas(string ola);

        //Response BloquearOla(string olas, string estado);
    }
}
