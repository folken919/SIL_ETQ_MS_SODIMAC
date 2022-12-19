using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ola_automatica.worker.core.Interfaces;
using Sodimac.Cedis.Core.DTOs;
using Sodimac.Cedis.Core.Entities;
using Sodimac.Cedis.Core.Excepciones;
using Sodimac.Cedis.Core.Interfaces.Services;
using Sodimac.Cedis.Core.Utils;
using System;

namespace Sodimac.Cedis.Api.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/ImprimeEtqCedis")]
    [ApiController]
    public class ImpresionEtiquetaController : ControllerBase
    {
        #region variables
        private readonly IMapper _mapper;
        private readonly IImpresionEtiquetaService _impresionEtiquetaService;
        private readonly IConsolaSodimac _logger;
        #endregion

        #region Metodos
        public ImpresionEtiquetaController(IMapper mapper, IImpresionEtiquetaService impresionEtiquetaService, IConsolaSodimac consola)
        {
            _mapper = mapper;
            _impresionEtiquetaService = impresionEtiquetaService;
            _logger = consola;
        }

        /// <summary>
        /// Controlador encargado de realizar la impresión masiva de etiquetas
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("ImprimirEtiquetasMasivo")]
        public IActionResult ImprimirEtiquetasMasivo([FromBody] RequestImpDTO request)
        {
            try
            {
                RequestImp imprimir = _mapper.Map<RequestImp>(request);
                int respuesta = _impresionEtiquetaService.ImprimirEtiquetasMasivo(imprimir);
                return Ok(new ResponseDTO { ok = true, mensaje = "Etiquetas impresas", resultado = respuesta });
            }
            catch (ExcepcionOperacion exOp)
            {
                _logger.LogWarning(exOp.Message);
                return Ok(new ResponseDTO { ok = false, mensaje = string.Format(Constantes.MensajesError.errorImprimirEtiquetas, exOp.Message) });
            }
            catch (Exception ex)
            {

                var innerException = ex.InnerException != null ? ex.InnerException.Message : "";

                _logger.LogCritical("ImprimirEtiquetasMasivo", ex);
                return Ok(new ResponseDTO { ok = false, mensaje = string.Format(Constantes.MensajesError.errorImprimirEtiquetas, ex.Message) });
            }
        }
        /// <summary>
        /// Controlador encargado de realizar la impresión masiva de etiquetas
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("ImprimirEtiquetas")]
        public IActionResult ImprimirEtiquetas([FromBody] RequestDTO request)
        {
            try
            {
                Request imprimir = _mapper.Map<Request>(request);
                int respuesta = _impresionEtiquetaService.ImprimirEtiquetas(imprimir);
                return Ok(new ResponseDTO { ok = true, mensaje = "Etiquetas impresas", resultado = respuesta });
            }
            catch (ExcepcionOperacion exOp)
            {
                return Ok(new ResponseDTO { ok = false, mensaje = string.Format(Constantes.MensajesError.errorImprimirEtiquetas, exOp.Message) });
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException != null ? ex.InnerException.Message : "";
                return Ok(new ResponseDTO { ok = false, mensaje = string.Format(Constantes.MensajesError.errorImprimirEtiquetas, ex.Message) });
            }
        }

        [HttpPost("ImprimirEtqMasivoXml")]
        public IActionResult ImprimirEtqMasivoXml([FromBody] XmlEtqDto request)
        {
            try
            {
                XmlEtq imprimir = _mapper.Map<XmlEtq>(request);
                int respuesta = _impresionEtiquetaService.ImprimirEtqMasivoXml(imprimir);
                return Ok(new ResponseDTO { ok = true, mensaje = "Etiquetas impresas", resultado = respuesta });
            }
            catch (ExcepcionOperacion exOp)
            {
                return Ok(new ResponseDTO { ok = false, mensaje = string.Format(Constantes.MensajesError.errorImprimirEtiquetas, exOp.Message) });
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException != null ? ex.InnerException.Message : "";
                return Ok(new ResponseDTO { ok = false, mensaje = string.Format(Constantes.MensajesError.errorImprimirEtiquetas, ex.Message) });
            }
        }

        [HttpPost("ImprimirEtqMasivoPrincipal")]
        public IActionResult ImprimirEtqMasivoPrincipal([FromBody] XmlEtqDto request)
        {
            try
            {
                XmlEtq imprimir = _mapper.Map<XmlEtq>(request);
                int respuesta = _impresionEtiquetaService.ImprimirEtqMasivoPrincipal(imprimir);
                return Ok(new ResponseDTO { ok = true, mensaje = "Etiquetas impresas", resultado = respuesta });
            }
            catch (ExcepcionOperacion exOp)
            {
                return Ok(new ResponseDTO { ok = false, mensaje = string.Format(Constantes.MensajesError.errorImprimirEtiquetas, exOp.Message) });
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException != null ? ex.InnerException.Message : "";
                return Ok(new ResponseDTO { ok = false, mensaje = string.Format(Constantes.MensajesError.errorImprimirEtiquetas, ex.Message) });
            }
        }

        [HttpPost("CancelarImprimirEtq")]
        public IActionResult CancelarImprimirEtq(LoteImpresionDto loteImpresion)
        {
            try
            {
                var lote = _mapper.Map<LoteImpresion>(loteImpresion);
                Response respuesta = _impresionEtiquetaService.CancelarImprimirEtq(lote);
                if (respuesta == null)
                    return BadRequest("No se encontró informacion");
                else
                    return Ok(respuesta);
            }
            catch (ExcepcionOperacion exOp)
            {
                return Ok(new ResponseDTO { ok = false, mensaje = string.Format(Constantes.MensajesError.errorCancelarImpresionEtq, exOp.Message) });
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException != null ? ex.InnerException.Message : "";
                return Ok(new ResponseDTO { ok = false, mensaje = string.Format(Constantes.MensajesError.errorCancelarImpresionEtq, ex.Message) });
            }
        }

        [HttpPost("ConsultaLoteEtq")]
        public IActionResult ConsultaLoteEtq()
        {
            try
            {
                Response respuesta = _impresionEtiquetaService.ConsultaLoteEtq();
                if (respuesta == null)
                    return BadRequest("No se encontró informacion");
                else
                    return Ok(respuesta);
            }
            catch (ExcepcionOperacion exOp)
            {
                return Ok(new ResponseDTO { ok = false, mensaje = string.Format(Constantes.MensajesError.errorConsultandoLote, exOp.Message) });
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException != null ? ex.InnerException.Message : "";
                return Ok(new ResponseDTO { ok = false, mensaje = string.Format(Constantes.MensajesError.errorConsultandoLote, ex.Message) });
            }
        }

        [HttpPost("ValidaOlasHabilitadas")]
        public IActionResult ValidaOlasHabilitadas(RqOlaDto request)
        {
            try
            {
                var Ola = _mapper.Map<RqOla>(request);
                var habilitada = _impresionEtiquetaService.ValidaOlasHabilitadas(Ola.ola);
                ResponseDTO respuesta = new ResponseDTO { ok = false, mensaje = null, resultado = null };
                if (habilitada == null)
                {
                    return BadRequest("No se encontró informacion");
                }
                else if (habilitada == true)
                {
                    respuesta.ok = true;
                    respuesta.mensaje = "Ola Habilitada";
                    respuesta.resultado = ("1");
                    return Ok(respuesta);
                }
                else
                {
                    respuesta.ok = false;
                    respuesta.mensaje = "Ola No Habilitada";
                    respuesta.resultado = "0";
                    return Ok(respuesta);
                }
            }
            catch (ExcepcionOperacion exOp)
            {
                return Ok(new ResponseDTO { ok = false, mensaje = string.Format(Constantes.MensajesError.errorConsultandoLote, exOp.Message) });
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException != null ? ex.InnerException.Message : "";
                return Ok(new ResponseDTO { ok = false, mensaje = string.Format(Constantes.MensajesError.errorConsultandoLote, ex.Message) });
            }
        }

        [HttpPost("ValidaOlasHabilitadasMasivas")]
        public IActionResult ValidaOlasHabilitadasMasivas(RqOlaDto request)
        {
            try
            {
                var Ola = _mapper.Map<RqOla>(request);
                var habilitada = _impresionEtiquetaService.ValidaOlasHabilitadasMasivas(Ola.ola);
                ResponseDTO respuesta = new ResponseDTO { ok = false, mensaje = null, resultado = null };
                if (habilitada == null)
                {
                    return BadRequest("No se encontró informacion");
                }
                else
                {
                    return Ok(habilitada);
                }
            }
            catch (ExcepcionOperacion exOp)
            {
                return Ok(new ResponseDTO { ok = false, mensaje = string.Format(Constantes.MensajesError.errorConsultandoLote, exOp.Message) });
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException != null ? ex.InnerException.Message : "";
                return Ok(new ResponseDTO { ok = false, mensaje = string.Format(Constantes.MensajesError.errorConsultandoLote, ex.Message) });
            }
        }

        #endregion
    }
}
