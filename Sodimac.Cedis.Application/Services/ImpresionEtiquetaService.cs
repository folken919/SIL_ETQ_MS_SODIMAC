using ApesPrinterTools;
using ola_automatica.worker.core.Interfaces;
using Sodimac.Cedis.Core.DTOs;
using Sodimac.Cedis.Core.Entities;
using Sodimac.Cedis.Core.Excepciones;
using Sodimac.Cedis.Core.Interfaces.Repository;
using Sodimac.Cedis.Core.Interfaces.Services;
using Sodimac.Cedis.Core.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Sodimac.Cedis.Application.Services
{
    public class ImpresionEtiquetaService : IImpresionEtiquetaService
    {
        #region variables
        private readonly IImpresionEtiquetaRepository _impresionEtiquetaRepository;
        private readonly ICedisRepository _cedisRepository;
        private readonly IEjecucionesRepository _ejecucionesRepository;
        private readonly ILogsRepository _logsRepository;
        private readonly IConsultaRepository consultaRepository;
        private readonly IConsolaSodimac _logger;

        int respuesta;
        #endregion

        #region Metodos
        public ImpresionEtiquetaService(IImpresionEtiquetaRepository impresionEtiquetaRepository,
                                        ICedisRepository cedisRepository,
                                        IEjecucionesRepository ejecucionesRepository,
                                        ILogsRepository logsRepository,
                                        IConsultaRepository consultaRepository,
                                        IConsolaSodimac consola)
        {
            _impresionEtiquetaRepository = impresionEtiquetaRepository;
            _cedisRepository = cedisRepository;
            _ejecucionesRepository = ejecucionesRepository;
            _logsRepository = logsRepository;
            this.consultaRepository = consultaRepository;
            this._logger = consola;
        }

        /// <summary>
        /// Metodo encargado de gestionar de forma asincrona la impresión de las etiquetas para las diferentes olas
        /// </summary>
        /// Autor: Ricardo Vivas
        /// Fecha: 04/05/2022
        /// <param name="imprimir"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int ImprimirEtiquetasMasivo(RequestImp imprimir)
        {

            try
            {//Se valida que todas las olas se encuentren habilitadas para impresion
                bool olaHabilitada = ValidaOlasHabilitadas(imprimir.ola);
                if (olaHabilitada)
                {
                    BloqueaOla(Constantes.Estados.estadoBloquearOla, imprimir.ola);
                    var respuestaImpresion = ObtenerImprimirEtiquetas(imprimir);
                }
                else
                {
                    _logger.LogError(Constantes.MensajesError.errorOlasImpresion);
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                _logger.LogCritical("ImprimirEtiquetasMasivo", ex);
                _logsRepository.RegistrarLog(new LogInfo { errMsg = ex.Message, errNbr = Constantes.CodigosRespuesta.respuestaError });
                throw ex;
            }
            finally
            {
                //Se desbloquean todas las olas
                BloqueaOla(Constantes.Estados.estadoDesbloquearOla, imprimir.ola);
            }
        }

        public int ImprimirEtiquetas(Request imprimir)
        {

            try
            {//Se valida que todas las olas se encuentren habilitadas para impresion
                bool olaHabilitada = ValidaOlasHabilitadas(imprimir.ola);
                if (olaHabilitada)
                {
                    BloqueaOla(Constantes.Estados.estadoBloquearOla, imprimir.ola);
                    var respuestaImpresion = ObtenerImprimirEtiqueta(imprimir);
                }
                else
                {
                    _logger.LogError(Constantes.MensajesError.errorOlasImpresion);
                    throw new Exception(Constantes.MensajesError.errorOlasImpresion);
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                _logger.LogCritical("ImprimirEtiquetas", ex);
                _logsRepository.RegistrarLog(new LogInfo { errMsg = ex.Message, errNbr = Constantes.CodigosRespuesta.respuestaError });
                throw ex;
            }
            finally
            {
                //Se desbloquean todas las olas
                BloqueaOla(Constantes.Estados.estadoDesbloquearOla, imprimir.ola);
            }
        }

        public int ObtenerImprimirEtiqueta(Request imprimir)
        {
            try
            {
                imprimir.impresoras = ValidarIpsHabilitadas(imprimir);
                imprimir.impresoras = ValidarConexionImpresoras(imprimir.impresoras);

                if (imprimir.impresoras.Count > 0)
                {
                    string ips = Utils.ObtenerStringIps(imprimir.impresoras);

                    try
                    {
                        ScriptInfo script = _cedisRepository.getScriptInfo("ETQGETIMP", new List<string> { imprimir.cantidad, imprimir.ola, imprimir.olpn, imprimir.etiqueta, imprimir.lote, imprimir.area, imprimir.zona, imprimir.usuario, imprimir.flagReimpresion.ToString(), imprimir.flagAsignacion.ToString().ToUpper(), imprimir.transportadora });
                        DataTable tblEtiquetas = _ejecucionesRepository.getResultado(script);

                        tblEtiquetas.Columns["ID_ETIQUETA"].ColumnName = "TAGREFERENCE";
                        tblEtiquetas.Columns["CODE_IMPRESION"].ColumnName = "ZPLCODE";
                        if (tblEtiquetas != null && tblEtiquetas.Rows.Count > 0)
                        {
                            int tope = _impresionEtiquetaRepository.ConsultarTope(imprimir.ola);
                            CrearLote(imprimir.nuevoLote, tblEtiquetas.Rows.Count.ToString(), tope == 0 ? Constantes.CodigosGenericos.tope.ToString() : tope.ToString());
                            BloquearOlaEImpresora(ips, imprimir.ola, imprimir.nuevoLote, Constantes.Estados.estadoBloquearOla, imprimir.etiqueta);
                            respuesta = DistribuirEnImpresoras(Utils.ValidarCantidadImpresoras(imprimir.impresoras, tblEtiquetas.Rows.Count), tblEtiquetas, tope == 0 ? Constantes.CodigosGenericos.tope : tope, imprimir.nuevoLote, "-1");

                        }
                        else
                        {
                            if (!imprimir.flagAsignacion)
                            {
                                throw new Exception(Constantes.MensajesError.errorOlaSinAsignar);
                            }
                            else
                            {
                                throw new Exception(Constantes.MensajesExito.exitoEtiquetasAsignadas);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogCritical("ObtenerImprimirEtiqueta", ex);
                        _logsRepository.RegistrarLog(new LogInfo { errMsg = "ObtenerImprimirEtiqueta: " + ex.Message + " " + imprimir.impresoras, errNbr = -1 });
                        throw ex;
                    }
                    finally
                    {
                        BloquearOlaEImpresora(ips, imprimir.ola, imprimir.nuevoLote, Constantes.Estados.estadoDesbloquearOla, imprimir.etiqueta);
                    }
                }
                else
                {
                    _logger.LogError(Constantes.MensajesError.errorSinImpresorasDisponibles);
                    throw new Exception(Constantes.MensajesError.errorSinImpresorasDisponibles);
                }
                return respuesta;
            }
            catch (Exception ex)
            {
                _logger.LogCritical("ObtenerImprimirEtiqueta", ex);
                _logsRepository.RegistrarLog(new LogInfo { errMsg = ex.Message, errNbr = Constantes.CodigosRespuesta.respuestaError });
                throw new Exception(String.Format(Constantes.MensajesError.errorGenerandoImprimiendoEtq, ex.Message, imprimir.ola));
            }
        }
        /// <summary>
        /// Metodo encargado de realizar la logica de impresión para una única ola
        /// </summary>
        /// Autor: Ricardo Vivas
        /// Fecha: 04/05/2022
        /// <param name="imprimir"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int ObtenerImprimirEtiquetas(RequestImp imprimir)
        {
            try
            {
                imprimir.impresoras = ValidarIpsHabilitadasImp(imprimir);
                imprimir.impresoras = ValidarConexionImpresoras(imprimir.impresoras);

                if (imprimir.impresoras.Count > 0)
                {
                    string ips = Utils.ObtenerStringIps(imprimir.impresoras);

                    try
                    {
                        List<string> listaOlpn = new List<string>(imprimir.etiqueta.Split(','));
                        XElement xmlOlpns = new XElement("ETIQUETA", listaOlpn.Select(prod => new XElement("ID", prod)));

                        DataTable etiquetas = _impresionEtiquetaRepository.ObtenerZplsEtiquetasImprimirMasivo(imprimir, xmlOlpns.ToString());

                        if (etiquetas != null && etiquetas.Rows.Count > 0)
                        {
                            int tope = _impresionEtiquetaRepository.ConsultarTope(imprimir.ola);
                            CrearLote(imprimir.nuevoLote, etiquetas.Rows.Count.ToString(), tope == 0 ? Constantes.CodigosGenericos.tope.ToString() : tope.ToString());
                            BloquearOlaEImpresora(ips, imprimir.ola, imprimir.nuevoLote, Constantes.Estados.estadoBloquearOla, imprimir.etiqueta);
                            respuesta = DistribuirEnImpresoras(Utils.ValidarCantidadImpresoras(imprimir.impresoras, etiquetas.Rows.Count), etiquetas, tope == 0 ? Constantes.CodigosGenericos.tope : tope, imprimir.nuevoLote, "-1");
                        }
                        else
                        {
                            if (!imprimir.flagAsignacion)
                            {
                                _logger.LogError(Constantes.MensajesError.errorOlaSinAsignar);
                                throw new Exception(Constantes.MensajesError.errorOlaSinAsignar);
                            }
                            else
                            {
                                _logger.LogInfo(Constantes.MensajesExito.exitoEtiquetasAsignadas);
                                throw new Exception(Constantes.MensajesExito.exitoEtiquetasAsignadas);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogCritical("ObtenerImprimirEtiquetas 230", ex);
                        _logsRepository.RegistrarLog(new LogInfo { errMsg = "ObtenerImprimirEtiqueta: " + ex.Message + " " + imprimir.impresoras, errNbr = -1 });

                        throw ex;
                    }
                    finally
                    {
                        BloquearImpresora(ips, Constantes.Estados.estadoDesbloquearOla);
                        BloqueaOla( Constantes.Estados.estadoDesbloquearOla, imprimir.ola);
                        //BloquearOlaEImpresora(ips, imprimir.ola, imprimir.nuevoLote, Constantes.Estados.estadoDesbloquearOla, imprimir.etiqueta);
                    }
                }
                else
                {
                    _logger.LogError(Constantes.MensajesError.errorSinImpresorasDisponibles);
                    throw new Exception(Constantes.MensajesError.errorSinImpresorasDisponibles);
                }
                return respuesta;

            }
            catch (Exception ex)
            {
                _logger.LogCritical("ObtenerImprimirEtiquetas 248", ex);
                _logsRepository.RegistrarLog(new LogInfo { errMsg = ex.Message, errNbr = Constantes.CodigosRespuesta.respuestaError });
                throw new Exception(String.Format(Constantes.MensajesError.errorGenerandoImprimiendoEtq, ex.Message, imprimir.ola));
            }
        }

        /// <summary>
        /// Metodo encargado de validar si la ola de la etiqueta se encuentra habilitada
        /// </summary>
        /// Autor: Jvivas
        /// Fecha: 27/04/2022
        /// <param name="idEtiqueta"></param>
        /// <returns></returns>
        public bool ValidaOlasHabilitadas(string ola)
        {
            try
            {
                ScriptInfo scriptInfo = _cedisRepository.getScriptInfo(Constantes.Dinamicos.validaOlaHabilitada, new List<string> { ola });
                DataTable resultado = _ejecucionesRepository.getResultado(scriptInfo);

                if (resultado != null && resultado.Rows.Count > 0)
                {
                    string salida = resultado.Rows[0][0].ToString();
                    return Convert.ToBoolean(salida);
                }
                else
                {
                    _logger.LogError(Constantes.Dinamicos.validaOlaHabilitada);
                    throw new ExcepcionOperacion(Constantes.Dinamicos.validaOlaHabilitada, Constantes.CodigosRespuesta.respuestaError, null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical("ValidaOlasHabilitadas",ex);
                _logsRepository.RegistrarLog(new LogInfo { errMsg = "ValidaOlasHabilitadas: " + ex.Message + " " + ola, errNbr = -1 });
                throw ex;
            }

        }

        /// <summary>
        /// Metodo encargado de bloquear una Ola
        /// </summary>
        /// Autor: Jvivas
        /// Fecha: 27/04/2022
        /// <param name="estado"></param>
        /// <param name="olas"></param>
        /// <returns></returns>
        private Response BloqueaOla(string estado, string olas)
        {
            try
            {
                ScriptInfo scriptInfo = _cedisRepository.getScriptInfo(Constantes.Dinamicos.bloqueaOla, new List<string> { estado, olas });
                Response resultado = _ejecucionesRepository.Ejecutar(scriptInfo);

                if (!resultado.ok)
                {
                    _logger.LogError(Constantes.MensajesError.errorBloquearOla);
                    throw new ExcepcionOperacion(String.Format(Constantes.MensajesError.errorBloquearOla, resultado.mensaje), Constantes.CodigosRespuesta.respuestaError, null);
                }
                return resultado;

            }
            catch (Exception ex)
            {
                _logger.LogCritical("BloqueaOla", ex);
                _logsRepository.RegistrarLog(new LogInfo { errMsg = "BloqueaOla: " + ex.Message + " " + olas, errNbr = -1 });
                throw ex;
            }

        }

        /// <summary>
        /// Metodo encargado de bloquear una ola e impresora cuando se está realizando la impresión
        /// </summary>
        /// Autor: Ricardo Vivas
        /// Fecha: 05/05/2022
        /// <param name="ips"></param>
        /// <param name="olas"></param>
        /// <param name="lote"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        private Response BloquearOlaEImpresora(string ips, string olas, string lote, string estado, string idEtqs)
        {
            try
            {
                ScriptInfo scriptInfo = _cedisRepository.getScriptInfo(Constantes.Dinamicos.bloquearOlaImpresora, new List<string> { estado, ips, olas, lote, idEtqs });
                Response resultado = _ejecucionesRepository.Ejecutar(scriptInfo);

                if (!resultado.ok)
                {
                    _logger.LogError(Constantes.MensajesError.errorBloquearOla);
                    throw new ExcepcionOperacion(String.Format(Constantes.MensajesError.errorBloquearOla, resultado.mensaje), Constantes.CodigosRespuesta.respuestaError, null);
                }
                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogCritical("BloquearOlaEImpresora", ex);
                _logsRepository.RegistrarLog(new LogInfo { errMsg = "BloquearOlaEImpresora: " + ex.Message + " " + olas, errNbr = -1 });
                throw ex;
            }
        }

        /// <summary>
        /// Metodo encargado de crear un lote
        /// </summary>
        /// <param name="lote"></param>
        /// <param name="cantidadEtq"></param>
        /// <param name="largoBloque"></param>
        /// <returns></returns>
        private Response CrearLote(string lote, string cantidadEtq, string largoBloque)
        {
            try
            {
                ScriptInfo scriptInfo = _cedisRepository.getScriptInfo(Constantes.Dinamicos.crearLote, new List<string> { lote, cantidadEtq, largoBloque });
                Response resultado = _ejecucionesRepository.Ejecutar(scriptInfo);

                if (!resultado.ok)
                {
                    _logger.LogError(Constantes.MensajesError.errorCrearLoteImpresion);
                    throw new ExcepcionOperacion(Constantes.MensajesError.errorCrearLoteImpresion, Constantes.CodigosRespuesta.respuestaError, null);
                }
                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogCritical("CrearLote", ex);
                _logsRepository.RegistrarLog(new LogInfo { errMsg = "CrearLote: " + ex.Message + " " + lote, errNbr = -1 });
                throw ex;
            }
        }

        /// <summary>
        /// Metodo encargado de validar que las IPS estén habilitadas
        /// </summary>
        /// <param name="imprimirDTO"></param>
        /// <returns></returns>
        private List<Impresora> ValidarIpsHabilitadas(Request imprimir)
        {
            try
            {
                string ips = Utils.ObtenerStringIps(imprimir.impresoras);
                ScriptInfo scriptInfo = _cedisRepository.getScriptInfo(Constantes.Dinamicos.impresorasDisponibles, new List<string> { imprimir.area, imprimir.zona,  ips, imprimir.ola, imprimir.cedis });
                DataTable resultado = _ejecucionesRepository.getResultado(scriptInfo);

                if (resultado != null && resultado.Rows.Count > 0)
                {
                    List<Impresora> listIps = new List<Impresora>();
                    foreach (DataRow row in resultado.Rows)
                    {
                        Impresora impTemp = new Impresora();
                        impTemp.ip = row[0].ToString();
                        impTemp.nombre = row[1].ToString();
                        listIps.Add(impTemp);
                    }
                    return listIps;
                }
                else
                {
                    _logger.LogError(Constantes.MensajesError.errorConsultandoImpresorasDisp);
                    throw new ExcepcionOperacion(Constantes.MensajesError.errorConsultandoImpresorasDisp, Constantes.CodigosRespuesta.respuestaError, null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical("ValidarIpsHabilitadas", ex);
                _logsRepository.RegistrarLog(new LogInfo { errMsg = "ValidarIpsHabilitadas: " + ex.Message + " " + imprimir.impresoras, errNbr = -1 });
                throw ex;
            }
        }

        /// <summary>
        /// Metodo encargado de validar que las IPS estén habilitadas
        /// </summary>
        /// <param name="imprimirImpDTO"></param>
        /// <returns></returns>
        private List<Impresora> ValidarIpsHabilitadasImp(RequestImp imprimir)
        {
            try
            {
                string ips = Utils.ObtenerStringIps(imprimir.impresoras);
                ScriptInfo scriptInfo = _cedisRepository.getScriptInfo(Constantes.Dinamicos.impresorasDisponibles, new List<string> { imprimir.area, imprimir.zona, ips, imprimir.ola, imprimir.cedis });
                DataTable resultado = _ejecucionesRepository.getResultado(scriptInfo);

                if (resultado != null && resultado.Rows.Count > 0)
                {
                    List<Impresora> listIps = new List<Impresora>();
                    foreach (DataRow row in resultado.Rows)
                    {
                        Impresora impTemp = new Impresora();
                        impTemp.ip = row[0].ToString();
                        impTemp.nombre = row[1].ToString();
                        listIps.Add(impTemp);
                    }
                    return listIps;
                }
                else
                {
                    _logger.LogError(Constantes.MensajesError.errorConsultandoImpresorasDisp);
                    throw new ExcepcionOperacion(Constantes.MensajesError.errorConsultandoImpresorasDisp, Constantes.CodigosRespuesta.respuestaError, null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical("ValidarIpsHabilitadasImp", ex);
                _logsRepository.RegistrarLog(new LogInfo { errMsg = "ValidarIpsHabilitadas: " + ex.Message + " " + imprimir.impresoras, errNbr = -1 });
                throw ex;
            }
        }

        /// <summary>
        /// Método que valida la conexión con las impresoras seleccionadas.
        /// </summary>
        /// Autor: Jvivas
        /// Fecha: 27/04/2022
        /// <param name="impresoras"></param>
        /// <returns></returns>
        private List<Impresora> ValidarConexionImpresoras(List<Impresora> impresoras)
        {
            PrinterTools printer;
            List<Impresora> nuevaLista = new List<Impresora>();
            foreach (Impresora imp in impresoras)
            {
                printer = new PrinterTools(imp.ip);
                if (printer.Error == null || printer.Error.Rows.Count == 0)
                {
                    nuevaLista.Add(imp);
                }
                else
                {
                    _logger.LogError($"Se generó un error al intentar conectar con la impresora {imp.ip}:");
                    foreach(DataRow e in printer.Error.Rows)
                        _logger.LogError($"{e[0]}");
                }
            }
            //TODO: Descomentar para debug
            //nuevaLista.Add(new Impresora { ip = "10.23.188.50",nombre = "IMPRESORA 4 TENJO",status = "1" });
            return nuevaLista;
        }

        /// <summary>
        /// Metodo encargado de distribuir las impresiones en la n cantidad de impresoras
        /// </summary>
        /// <param name="listImpresoras"></param>
        /// <param name="etiquetas"></param>
        /// <param name="packageLenght"></param>
        /// <param name="nuevoLote"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        /// <exception cref="ExcepcionOperacion"></exception>
        /// <exception cref="Exception"></exception>
        private int DistribuirEnImpresoras(List<Impresora> listImpresoras, DataTable etiquetas, int packageLenght, string nuevoLote, string usuario)
        {
            int respuesta = 0;
            List<HiloImpresion> listaHilos = new List<HiloImpresion>();
            int cantidadPorImpresora = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(etiquetas.Rows.Count) / listImpresoras.Count));
            int ultimoPaquete = etiquetas.Rows.Count % cantidadPorImpresora;

            try
            {
                HiloImpresion hiloTemp;
                for (int i = 0; i < listImpresoras.Count; i++)
                {
                    hiloTemp = new HiloImpresion();
                    hiloTemp.etiquetas = new DataTable();
                    hiloTemp.etiquetas = etiquetas.Clone();
                    hiloTemp.etiquetas.Clear();
                    if (i == listImpresoras.Count - 1 && ultimoPaquete != 0)
                    {
                        for (int j = 0; j < ultimoPaquete; j++)
                        {
                            hiloTemp.etiquetas.Rows.Add(etiquetas.Rows[j + cantidadPorImpresora * i].ItemArray);
                        }
                    }
                    else
                    {
                        for (int j = 0; j < cantidadPorImpresora; j++)
                        {
                            hiloTemp.etiquetas.Rows.Add(etiquetas.Rows[j + cantidadPorImpresora * i].ItemArray);
                        }
                    }

                    hiloTemp.cantidadPaquetes = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(hiloTemp.etiquetas.Rows.Count) / packageLenght));
                    hiloTemp.restantesUltimoPaquete = hiloTemp.etiquetas.Rows.Count % packageLenght;
                    hiloTemp.ipImpresora = listImpresoras.ElementAt(i).ip;
                    hiloTemp.nombreImpresora = listImpresoras.ElementAt(i).nombre;

                    listaHilos.Add(hiloTemp);
                }
                var tareas = listaHilos.Select(hilo => Task<int>.Factory.StartNew(() => InsertarEImprimirZpls(packageLenght, hilo.cantidadPaquetes, hilo.restantesUltimoPaquete, hilo.etiquetas, hilo.ipImpresora, hilo.nombreImpresora, nuevoLote, usuario))).ToArray();

                Task.WaitAll(tareas);

                var resTareas = tareas.Select(t => t.Result).ToList();

                foreach (int res in resTareas)
                {
                    respuesta += res;
                }
            }
            catch (ExcepcionOperacion exo)
            {
                _logger.LogCritical("DistribuirEnImpresoras", exo);
                throw new ExcepcionOperacion(Constantes.MensajesError.errorDistribuirImpresoras + " " + exo.Message, exo.Codigo, exo.Data, exo.InnerException);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("DistribuirEnImpresoras", ex);
                throw new Exception(Constantes.MensajesError.errorDistribuirImpresoras + ex.Message, ex.InnerException);
            }

            return respuesta;
        }

        /// <summary>
        /// Metodo encargado de imprimir los zpl
        /// </summary>
        /// <param name="packageLenght"></param>
        /// <param name="packCount"></param>
        /// <param name="restantes"></param>
        /// <param name="dt"></param>
        /// <param name="ipPrinter"></param>
        /// <param name="printerName"></param>
        /// <param name="lote"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        /// <exception cref="ExcepcionOperacion"></exception>
        /// <exception cref="Exception"></exception>
        private int InsertarEImprimirZpls(int packageLenght, int packCount, int restantes, DataTable dt, string ipPrinter, string printerName, string lote, string usuario)
        {
            List<string> zpls;
            int respuesta = 0;

            try
            {
                for (int i = 0; i < packCount; i++)
                {
                    if (!ContinuarImprimiendo(lote))
                    {
                        break;
                    }

                    zpls = new List<string>();

                    if (i == packCount - 1 && restantes != 0)
                    {
                        for (int j = 0; j < restantes; j++)
                        {
                            zpls.Add(dt.Rows[i * packageLenght + j]["ZPLCODE"].ToString());
                        }
                    }
                    else
                    {
                        for (int j = 0; j < packageLenght; j++)
                        {
                            zpls.Add(dt.Rows[i * packageLenght + j]["ZPLCODE"].ToString());
                        }
                    }

                    PrinterTools printer = new PrinterTools(ipPrinter, printerName, zpls);

                    //TODO: Descomentar para debug
                    //printer.Result.Add(true);

                    // error 
                    if (printer.Result.Count == 0)
                    {
                        _logger.LogError("Error de impresion (614): " + printer.Error.Rows[0]["Message"].ToString());
                        throw new Exception("Error de impresion: " + printer.Error.Rows[0]["Message"].ToString());
                    }
                    string etiquetas = string.Empty;
                    string listaErrores = string.Empty;
                    for (int k = 0; k < printer.Result.Count; k++)
                    {
                        if (printer.Result[k])
                        {
                            respuesta += 1;
                            etiquetas = string.Concat(etiquetas, dt.Rows[i * packageLenght + k]["TAGREFERENCE"].ToString(), ",");
                        }
                        else
                        {
                            listaErrores = string.Concat(listaErrores, dt.Rows[i * packageLenght + k]["TAGREFERENCE"].ToString(), ";");
                        }
                    }
                    if (!string.IsNullOrEmpty(listaErrores))
                    {
                        _logsRepository.RegistrarLog(new LogInfo { errMsg = "PrinterTools: Etiquetas no impresas. Ciclo: " + (i + 1).ToString(), errNbr = -1 });
                    }
                    etiquetas = etiquetas.Substring(0, etiquetas.Length - 1);
                    RegistraEtiquetasImpresasExito(etiquetas, lote, usuario);
                }
                return respuesta;
            }
            catch (ExcepcionOperacion exo)
            {
                _logger.LogCritical("InsertarEImprimirZpls", exo);
                _logsRepository.RegistrarLog(new LogInfo { errMsg = "InsertarEImprimirZpls: " + exo.Message, errNbr = -1 });

                throw new ExcepcionOperacion(ipPrinter + " " + exo.Message, exo.Codigo, exo.Data, exo.InnerException);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("InsertarEImprimirZpls", ex);
                _logsRepository.RegistrarLog(new LogInfo { errMsg = "InsertarEImprimirZpls: " + ex.Message, errNbr = -1 });

                throw new Exception(ipPrinter + " " + ex.Message, ex.InnerException);
            }
        }

        /// <summary>
        /// Metodo encargado de validar la continuidad de la impresion
        /// </summary>
        /// <param name="lote"></param>
        /// <returns></returns>
        private bool ContinuarImprimiendo(string lote)
        {
            try
            {
                Response response = new Response { ok = false, mensaje = String.Empty, resultado = null };
                ScriptInfo scriptInfo = _cedisRepository.getScriptInfo(Constantes.Dinamicos.continuarImprimiendo, new List<string> { lote });
                DataTable resultado = _ejecucionesRepository.getResultado(scriptInfo);

                if (resultado != null && resultado.Rows.Count > 0)
                {
                    string salida = resultado.Rows[0][0].ToString();
                    response.ok = Convert.ToBoolean(Convert.ToInt32(salida));
                    return response.ok;
                }
                else
                {
                    _logger.LogError(Constantes.MensajesError.errorParametroImpresion);
                    throw new ExcepcionOperacion(Constantes.MensajesError.errorParametroImpresion, Constantes.CodigosRespuesta.respuestaError, null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical("ContinuarImprimiendo", ex);
                _logsRepository.RegistrarLog(new LogInfo { errMsg = "ContinuarImprimiendo: " + lote + " " + ex.Message, errNbr = -1 });
                throw ex;
            }
        }

        /// <summary>
        /// Metodo encargado de registar las etiquetas que fueron impresas
        /// </summary>
        /// <param name="etiquetas"></param>
        /// <param name="lote"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public Response RegistraEtiquetasImpresasExito(string etiquetas, string lote, string usuario)
        {
            try
            {
                Response response = new Response { ok = false, mensaje = String.Empty, resultado = null };
                ScriptInfo scriptInfo = _cedisRepository.getScriptInfo(Constantes.Dinamicos.etiquetasImpresasOk, new List<string> { etiquetas, lote, usuario });
                Response resultado = _ejecucionesRepository.Ejecutar(scriptInfo);

                if (!resultado.ok)
                {
                    _logger.LogError(Constantes.MensajesError.errorRegistrarEtiquetasImp);
                    throw new ExcepcionOperacion(Constantes.MensajesError.errorRegistrarEtiquetasImp, Constantes.CodigosRespuesta.respuestaError, null);
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogCritical("RegistraEtiquetasImpresasExito", ex);
                _logsRepository.RegistrarLog(new LogInfo { errMsg = "RegistraEtiquetasImpresasExito: " + lote + " " + ex.Message, errNbr = -1 });
                throw ex;
            }
        }

        public Response ConsultaLoteEtq()
        {
            try
            {
                Response response = new Response { ok = false, mensaje = String.Empty, resultado = null };
                ScriptInfo scriptInfo = _cedisRepository.getScriptInfo(Constantes.Dinamicos.consultaLote, new List<string> { "#" });
                DataTable resultado = _ejecucionesRepository.getResultado(scriptInfo);

                if (resultado != null && resultado.Rows.Count > 0)
                {
                    List<LoteDto> listLote = new List<LoteDto>();
                    foreach (DataRow dr in resultado.Rows)
                    {
                        LoteDto loteTemp = new LoteDto();
                        loteTemp = new LoteDto(dr);
                        listLote.Add(loteTemp);
                    }
                    response.resultado = listLote;
                    response.ok = true;
                }
                else
                {
                    response.mensaje = Constantes.MensajesError.errorConsultandoLote;
                    throw new ExcepcionOperacion(Constantes.MensajesError.errorConsultandoLote, Constantes.CodigosRespuesta.respuestaError, null);
                }
                return response;

            }
            catch (Exception ex)
            {
                _logsRepository.RegistrarLog(new LogInfo { errMsg = "ConsultaLoteEtq: " + ex.Message, errNbr = -1 });
                throw ex;
            }
        }
        public Response CancelarImprimirEtq(LoteImpresion lote)
        {
            try
            {
                Response response = new Response { ok = false, mensaje = String.Empty, resultado = null };
                ScriptInfo scriptInfo = _cedisRepository.getScriptInfo(Constantes.Dinamicos.cancelarImpresion, new List<string> { "0", lote.loteImpresion });
                Response resultado = _ejecucionesRepository.Ejecutar(scriptInfo);

                if (!resultado.ok)
                {
                    response.mensaje = Constantes.MensajesError.errorCancelarImpresionEtq;
                    throw new ExcepcionOperacion(Constantes.MensajesError.errorCancelarImpresionEtq, Constantes.CodigosRespuesta.respuestaError, null);
                }
                else
                {
                    response.ok = true;
                    response.mensaje = Constantes.MensajesExito.cancelacionExitosa;
                }
                return response;
            }
            catch (Exception ex)
            {
                _logsRepository.RegistrarLog(new LogInfo { errMsg = "CancelarImprimirEtq: " + lote + " " + ex.Message, errNbr = -1 });
                throw ex;
            }
        }

        public int ImprimirEtqMasivoXml(XmlEtq imprimir)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(imprimir.xml);

            XmlNodeList elemList = doc.GetElementsByTagName("OLA");
            List<string> listOlas = new List<string>();
            for (int i = 0; i < elemList.Count; i++)
            {
                listOlas.Add(elemList[i].InnerText);
            }
            string olas = listOlas.ToStringParams();
            olas = olas.TrimEnd(',');

            try
            {
                List<OlaPorEstadoDto> olaHabilitada = ValidaOlasHabilitadasMasivasXml(olas);

                if (Convert.ToBoolean(olaHabilitada[0].estado))
                {
                    BloqueaOla(Constantes.Estados.estadoBloquearOla, olaHabilitada[0].ola);
                    var respuestaImpresion = ObtenerImprimirEtiquetasXml(imprimir, Constantes.Dinamicos.getImpresoraMasiva);
                }
                else
                {
                    throw new Exception(Constantes.MensajesError.errorOlasImpresion);
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                _logsRepository.RegistrarLog(new LogInfo { errMsg = ex.Message, errNbr = Constantes.CodigosRespuesta.respuestaError });
                throw ex;
            }
            finally
            {
                //Se desbloquean todas las olas
                BloqueaOla(Constantes.Estados.estadoDesbloquearOla, olas);
            }
        }

        public int ImprimirEtqMasivoPrincipal(XmlEtq imprimir)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(imprimir.xml);

            XmlNodeList elemList = doc.GetElementsByTagName("OLA");
            List<string> listOlas = new List<string>();
            for (int i = 0; i < elemList.Count; i++)
            {
                listOlas.Add(elemList[i].InnerText);
            }
            string olas = listOlas.ToStringParams();
            olas = olas.TrimEnd(',');
            try
            {
                List<OlaPorEstadoDto> olaHabilitada = ValidaOlasHabilitadasMasivasXml(olas);

                if (Convert.ToBoolean(olaHabilitada[0].estado))
                {
                    BloqueaOla(Constantes.Estados.estadoBloquearOla, olaHabilitada[0].ola);
                    var respuestaImpresion = ObtenerImprimirEtiquetasXml(imprimir, Constantes.Dinamicos.getImpPrincipal);
                }
                else
                {
                    throw new Exception(Constantes.MensajesError.errorOlasImpresion);
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                _logsRepository.RegistrarLog(new LogInfo { errMsg = ex.Message, errNbr = Constantes.CodigosRespuesta.respuestaError });
                throw ex;
            }
            finally
            {
                //Se desbloquean todas las olas
                BloqueaOla(Constantes.Estados.estadoDesbloquearOla, olas);
            }
        }

        public Response ValidaOlasHabilitadasMasivas(string ola)
        {
            try
            {
                Response response = new Response { ok = false, mensaje = string.Empty, resultado = null };
                ScriptInfo scriptInfo = _cedisRepository.getScriptInfo(Constantes.Dinamicos.validaOlaHabilitadaMasiva, new List<string> { ola });
                DataTable resultado = _ejecucionesRepository.getResultado(scriptInfo);
                if (resultado.Rows.Count == 0)
                {
                    response.mensaje = "La consulta no obtuvo resultados. Tag:ETQMOLAEST;";
                    return response;
                }
                List<OlaPorEstadoDto> listOlasPorEstado = new List<OlaPorEstadoDto>();
                OlaPorEstadoDto plantillaTemp;
                foreach (DataRow dr in resultado.Rows)
                {
                    plantillaTemp = new OlaPorEstadoDto(dr);
                    listOlasPorEstado.Add(plantillaTemp);
                }
                response.resultado = listOlasPorEstado;
                response.ok = true;
                return response;
            }
            catch (Exception ex)
            {
                _logsRepository.RegistrarLog(new LogInfo { errMsg = "ValidaOlasHabilitadas: " + ex.Message + " " + ola, errNbr = -1 });
                throw ex;
            }

        }

        public List<OlaPorEstadoDto> ValidaOlasHabilitadasMasivasXml(string ola)
        {
            try
            {
                ScriptInfo scriptInfo = _cedisRepository.getScriptInfo(Constantes.Dinamicos.validaOlaHabilitadaMasiva, new List<string> { ola });
                DataTable resultado = _ejecucionesRepository.getResultado(scriptInfo);
                List<OlaPorEstadoDto> listOlasPorEstado = new List<OlaPorEstadoDto>();
                OlaPorEstadoDto plantillaTemp;
                foreach (DataRow dr in resultado.Rows)
                {
                    plantillaTemp = new OlaPorEstadoDto(dr);
                    listOlasPorEstado.Add(plantillaTemp);
                }
                return listOlasPorEstado;
            }
            catch (Exception ex)
            {
                _logsRepository.RegistrarLog(new LogInfo { errMsg = "ValidaOlasHabilitadas: " + ex.Message + " " + ola, errNbr = -1 });
                throw ex;
            }
        }

        public int ObtenerImprimirEtiquetasXml(XmlEtq imprimir, string tag)
        {
            List<Impresora> listImpre = new List<Impresora>();

            var doc = XDocument.Parse(imprimir.xml);
            var impresora = from impresoras in doc.Descendants("IMPRESORAS")
                            select new
                            {
                                ip = impresoras.Element("IP").Value,
                                nombre = impresoras.Element("NOMBRE").Value,
                                status = impresoras.Element("STATUS").Value,
                            };
            var result = from xml in doc.Descendants("IMPRIMIRMULTIPLE")
                         select new
                         {
                             nuevoLote = xml.Element("NUEVOLOTE").Value,
                             flagAsignacion = xml.Element("FLAGASIGNACION").Value,

                         };
            foreach (var item in impresora)
            {
                Impresora impresoras = new Impresora(item.ip, item.nombre, item.status);
                listImpre.Add(impresoras);
            }
            var nuevoLote = "";
            bool flagAsignacion = false;
            foreach (var item in result)
            {
                nuevoLote = item.nuevoLote;
                flagAsignacion = Convert.ToBoolean(item.flagAsignacion);
            }
            XmlDocument olaXml = new XmlDocument();
            olaXml.LoadXml(imprimir.xml);

            XmlNodeList elemList = olaXml.GetElementsByTagName("OLA");
            List<string> listOlas = new List<string>();
            for (int i = 0; i < elemList.Count; i++)
            {
                listOlas.Add(elemList[i].InnerText);
            }
            string olas = listOlas.ToStringParams();
            olas = olas.TrimEnd(',');

            try
            {
                listImpre = ValidarIpsHabilitadasXml(imprimir, listImpre);
                listImpre = ValidarConexionImpresoras(listImpre);

                if (listImpre.Count > 0)
                {
                    string ips = Utils.ObtenerStringIps(listImpre);
                    try
                    {
                        DataTable etiquetas = _impresionEtiquetaRepository.ObtenerZplsEtiquetasImprimirMasivoXml(imprimir, tag);

                        if (etiquetas != null && etiquetas.Rows.Count > 0)
                        {
                            int tope = _impresionEtiquetaRepository.ConsultarTope(olas);
                            CrearLote(nuevoLote, etiquetas.Rows.Count.ToString(), tope == 0 ? Constantes.CodigosGenericos.tope.ToString() : tope.ToString());
                            BloquearOlaEImpresora(ips, olas, nuevoLote, Constantes.Estados.estadoBloquearOla, "");
                            respuesta = DistribuirEnImpresoras(Utils.ValidarCantidadImpresoras(listImpre, etiquetas.Rows.Count), etiquetas, tope == 0 ? Constantes.CodigosGenericos.tope : tope, nuevoLote, "-1");
                        }
                        else
                        {
                            if (!flagAsignacion)
                            {
                                throw new Exception(Constantes.MensajesError.errorOlaSinAsignar);
                            }
                            else
                            {
                                throw new Exception(Constantes.MensajesExito.exitoEtiquetasAsignadas);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logsRepository.RegistrarLog(new LogInfo { errMsg = "ObtenerImprimirEtiqueta: " + ex.Message + " " + listImpre, errNbr = -1 });

                        throw ex;
                    }
                    finally
                    {
                        BloquearOlaEImpresora(ips, olas, nuevoLote, Constantes.Estados.estadoDesbloquearOla, "");
                    }
                }
                else
                {
                    throw new Exception(Constantes.MensajesError.errorSinImpresorasDisponibles);
                }
                return respuesta;

            }
            catch (Exception ex)
            {
                _logger.LogCritical("ObtenerImprimirEtiquetasXml", ex);
                _logsRepository.RegistrarLog(new LogInfo { errMsg = ex.Message, errNbr = Constantes.CodigosRespuesta.respuestaError });
                throw new Exception(String.Format(Constantes.MensajesError.errorGenerandoImprimiendoEtq, ex.Message, olas));
            }
        }

        private List<Impresora> ValidarIpsHabilitadasXml(XmlEtq imprimir, List<Impresora> listImpre)
        {
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(imprimir.xml);

                //ZONAS
                XmlNodeList elemZonas = xml.GetElementsByTagName("ID_ZONA");
                List<string> listZonas = new List<string>();
                for (int i = 0; i < elemZonas.Count; i++)
                {
                    listZonas.Add(elemZonas[i].InnerText);
                }
                string zonas = listZonas.ToStringParams();
                zonas = zonas.TrimEnd(',');

                //AREAS
                XmlNodeList elemAreas = xml.GetElementsByTagName("AREA");
                List<string> listAreas = new List<string>();
                for (int i = 0; i < elemAreas.Count; i++)
                {
                    listAreas.Add(elemAreas[i].InnerText);
                }
                string areas = listAreas.ToStringParams();
                areas = areas.TrimEnd(',');

                //OLA
                XmlNodeList elemOlas = xml.GetElementsByTagName("OLA");
                List<string> listOlas = new List<string>();
                for (int i = 0; i < elemOlas.Count; i++)
                {
                    listOlas.Add(elemOlas[i].InnerText);
                }
                string olas = listOlas.ToStringParams();
                olas = olas.TrimEnd(',');

                //CEDIS
                var doc = XDocument.Parse(imprimir.xml);
                var result = from impMul in doc.Descendants("IMPRIMIRMULTIPLE")
                             select new
                             {
                                 cedis = impMul.Element("CEDIS").Value,
                             };
                string cedis = "";
                foreach (var item in result)
                {
                    cedis = item.cedis;
                }



                string ips = Utils.ObtenerStringIps(listImpre);
                ScriptInfo scriptInfo = _cedisRepository.getScriptInfo(Constantes.Dinamicos.impresorasDisponibles, new List<string> { areas, "-1",  ips, olas, cedis });
                DataTable resultado = _ejecucionesRepository.getResultado(scriptInfo);

                if (resultado != null && resultado.Rows.Count > 0)
                {
                    List<Impresora> listIps = new List<Impresora>();
                    foreach (DataRow row in resultado.Rows)
                    {
                        Impresora impTemp = new Impresora();
                        impTemp.ip = row[0].ToString();
                        impTemp.nombre = row[1].ToString();
                        listIps.Add(impTemp);
                    }
                    return listIps;
                }
                else
                {
                    throw new ExcepcionOperacion(Constantes.MensajesError.errorConsultandoImpresorasDisp, Constantes.CodigosRespuesta.respuestaError, null);
                }
            }
            catch (Exception ex)
            {
                _logsRepository.RegistrarLog(new LogInfo { errMsg = "ValidarIpsHabilitadas: " + ex.Message + " " + listImpre, errNbr = -1 });
                throw ex;
            }
        }

        private Response BloquearImpresora(string ips, string estado)
        {
            try
            {
                ScriptInfo scriptInfo = _cedisRepository.getScriptInfo(Constantes.Dinamicos.bloquearImpresora, new List<string> { estado, ips });
                Response resultado = _ejecucionesRepository.Ejecutar(scriptInfo);

                if (!resultado.ok)
                {
                    _logger.LogError(Constantes.MensajesError.errorBloquearOla);
                    throw new ExcepcionOperacion(String.Format(Constantes.MensajesError.errorBloquearOla, resultado.mensaje), Constantes.CodigosRespuesta.respuestaError, null);
                }
                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogCritical("BloquearImpresora", ex);
                _logsRepository.RegistrarLog(new LogInfo { errMsg = "BloquearImpresora: " + ex.Message + " " + ips, errNbr = -1 });
                throw ex;
            }
        }

        private Response BloquearOla(string olas, string estado)
        {
            try
            {
                ScriptInfo scriptInfo = _cedisRepository.getScriptInfo(Constantes.Dinamicos.bloquearOla, new List<string> { estado,  olas });
                Response resultado = _ejecucionesRepository.Ejecutar(scriptInfo);

                if (!resultado.ok)
                {
                    _logger.LogError(Constantes.MensajesError.errorBloquearOla);
                    throw new ExcepcionOperacion(String.Format(Constantes.MensajesError.errorBloquearOla, resultado.mensaje), Constantes.CodigosRespuesta.respuestaError, null);
                }
                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogCritical("BloquearOla", ex);
                _logsRepository.RegistrarLog(new LogInfo { errMsg = "BloquearOla: " + ex.Message + " " + olas, errNbr = -1 });
                throw ex;
            }
        }
        #endregion

    }
}
