using System;
using System.Collections.Generic;
using System.Text;

namespace Sodimac.Cedis.Core.Utils
{
    public static class Constantes
    {
        public struct Dinamicos
        {
            public const string validaOlaHabilitada = "ETQOLAESTA";
            public const string bloqueaOla = "ETQESTOLA";
            public const string impresorasDisponibles = "ETQIPDISPN";
            public const string crearLote = "ETQCREALOT";
            public const string bloquearOlaImpresora = "ETQIMPRESI";
            public const string bloquearImpresora = "ETQESTIMPR";
            public const string bloquearOla = "ETQESTOLA";
            public const string continuarImprimiendo = "ETQGETIMPR";
            public const string etiquetasImpresasOk = "ETQUPDETQ";
            public const string consultaLote = "ETQGETLOTE";
            public const string cancelarImpresion = "ETQSETIMPR";
            public const string validaOlaHabilitadaMasiva = "ETQMOLAEST";
            public const string impresorasDisponiblesMasiva = "ETQMIPDISP";
            public const string getImpresoraMasiva = "ETQMGETIMP";
            public const string getImpPrincipal = "ETGETIMPMA";
        }

        public struct MensajesError
        {
            public const string errorAbriendoConexion = "Se ha presentado un error intentando abri la conexión a la base de datos";
            public const string errorValidaOlaHabilitada = "Error al consultar el estado de la ola: ETQOLAESTA.";
            public const string errorBloquearOla = "Error al bloquear ola para impresión: ETQIMPRESI. Resultado operación {0}. ";
            public const string errorConsultandoImpresorasDisp = "ETQIPDISPN. error al consultar impresoras disponibles.";
            public const string errorConsultandoZpls = "Error consultando ZPLs: ETQGETIMP ";
            public const string errorConsultandoLote = "Error consultando Lote: ETQGETLOTE ";
            public const string errorOlaSinUsuarioAsignado = "La OLA a imprimir no posee usuario asignado ETQGETIMP ";
            public const string errorCrearLoteImpresion = "Error al crear el lote de impresión: ETQCREALOT ";
            public const string errorDistribuirImpresoras = "Se ha presentado un error al distribuir las impresoras";
            public const string errorParametroImpresion = "Error al consultar parámetro de impresión: ETQGETIMPR. ";
            public const string errorRegistrarEtiquetasImp = "Error al registrar etiquetas impresas: ETQUPDETQ ";
            public const string errorOlaSinAsignar = "La OLA a imprimir no posee usuario asignado. ETQGETIMP ";
            public const string errorSinImpresorasDisponibles = "No se encontraron impresoras disponibles. ";
            public const string errorOlasImpresion = "Una o más de las Olas enviadas se encuentra en impresión actualmente. ";
            public const string errorImprimirEtiquetas = "Se ha presentado un error imprimiendo las etiquetas. Error: {0} ";
            public const string errorGenerandoImprimiendoEtq = "Se ha presentado el siguiente error: {0} generado o imprimiendo etiquetas de la ola: {1} ";
            public const string errorCancelarImpresionEtq = "Se ha presentado el siguiente error: {0} Cancalación Impresión.";
            public const string errorConsultaVacioZpls = "Error consultando ZPLs: La consulta no obtuvo datos. ";
        }

        public struct MensajesExito
        {
            public const string exitoEtiquetasAsignadas = "Las etiquetas fueron asignadas a los usuarios correctamente";
            public const string cancelacionExitosa = "Se realizó cancelación de impresion correctamente";
        }

        public struct CodigosRespuesta
        {
            public const int respuestaError = -1;
            public const int respuestaExito = 1;
        }

        public struct Estados
        {
            public const string estadoBloquearOla = "27";
            public const string estadoDesbloquearOla = "1";
        }

        public struct CodigosGenericos
        {
            public const int tope = 100;
        }

        public struct ConsultaEtiquetasImp
        {
            public const string procedimiento = "PKG_ETIQUETA.PRC_CONSULTA_ETIQUETAS_IMP";
            public const string pTope = "P_TOPE";
            public const string pOla = "P_OLA";
            public const string pEtiqueta = "P_ETQ";
            public const string pLote = "P_LOTE";
            public const string pReimpresion = "P_REIMP";
            public const string pSalida = "P_SALIDA";
            public const string pCursor = "P_OUT_CUR";
        }
    }
}
