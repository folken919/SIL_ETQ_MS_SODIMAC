using Microsoft.Extensions.Configuration;
using ola_automatica.worker.core.Interfaces;
using Oracle.ManagedDataAccess.Client;
using Sodimac.Cedis.Core.Entities;
using Sodimac.Cedis.Core.Interfaces.Repository;
using Sodimac.Cedis.Core.Utils;
using Sodimac.Cedis.Infraestructure.Data;
using Sodimac.Cedis.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sodimac.Cedis.Infraestructure.Repository
{
    public class ImpresionEtiquetaRepository : IImpresionEtiquetaRepository
    {
        #region Variables
        private readonly IConfiguration _configuration;
        private readonly ICedisRepository _cedisRepository;
        private readonly IEjecucionesRepository _ejecucionesRepository;
        private readonly IConsultaRepository _consultaRepository;
        private readonly IConsolaSodimac _logger;
        #endregion

        #region Metodos

        public ImpresionEtiquetaRepository(IConfiguration configuration, ICedisRepository cedisRepository, IEjecucionesRepository ejecucionesRepository, IConsultaRepository consultaRepository, IConsolaSodimac consola)
        {
            _configuration = configuration;
            _cedisRepository = cedisRepository;
            _ejecucionesRepository = ejecucionesRepository;
            _consultaRepository = consultaRepository;
            _logger = consola;
        }

        /// <summary>
        /// Metodo para consultar el tope de impresión de una etiqueta
        /// </summary>
        /// Autor: Jvivas
        /// Fecha: 28/04/2022
        /// <param name="ola"></param>
        /// <returns></returns>
        public int ConsultarTope(string ola)
        {
            using (ModelContext context = new ModelContext(_configuration))
            {
                try
                {
                    var tope = (from etz in context.TblEtqZona
                                join etq in context.TblEtqEtiqueta
                                on new { X1 = etz.Cedis, X2 = etz.Area, X3 = etz.Zona } equals new { X1 = etq.LocationCedis, X2 = etq.LocationArea, X3 = etq.LocationZona }
                                where etq.WaveNbr == (ola)
                                select etz.Tope).FirstOrDefault().ToString();

                    return Convert.ToInt32(tope);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical("ConsultarTope", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Metodo encargado de obtener los zpl de una etiqueta
        /// </summary>
        /// <param name="imprimir"></param>
        /// <param name="etiquetas"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public DataTable ObtenerZplsEtiquetasImprimirMasivo(RequestImp imprimir, string etiquetas)
        {
            using (OracleConnection oracleConexion = new OracleConnection(_configuration.GetConnectionString("sgl_prod").Desencriptar()))
            {
                try
                {
                    DataTable tblEtiquetas = new DataTable();
                    OracleCommand oracleCommand = new OracleCommand(Constantes.ConsultaEtiquetasImp.procedimiento, oracleConexion);
                    OracleConnection.ClearPool(oracleConexion);
                    oracleCommand.CommandType = CommandType.StoredProcedure;
                    oracleCommand.Parameters.Add(Constantes.ConsultaEtiquetasImp.pTope, OracleDbType.Varchar2).Value = imprimir.cantidad;
                    oracleCommand.Parameters.Add(Constantes.ConsultaEtiquetasImp.pOla, OracleDbType.Varchar2).Value = imprimir.ola;
                    oracleCommand.Parameters.Add(Constantes.ConsultaEtiquetasImp.pEtiqueta, OracleDbType.XmlType, etiquetas.Length).Value = etiquetas;
                    oracleCommand.Parameters.Add(Constantes.ConsultaEtiquetasImp.pLote, OracleDbType.Varchar2).Value = imprimir.lote;
                    oracleCommand.Parameters.Add(Constantes.ConsultaEtiquetasImp.pReimpresion, OracleDbType.Int32).Value = imprimir.flagReimpresion;
                    oracleCommand.Parameters.Add(Constantes.ConsultaEtiquetasImp.pSalida, OracleDbType.Varchar2).Direction = ParameterDirection.Output;
                    oracleCommand.Parameters.Add(Constantes.ConsultaEtiquetasImp.pCursor, OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    oracleCommand.Parameters[Constantes.ConsultaEtiquetasImp.pSalida].Size = 2000;
                    if (SGLConnection.AbrirConexion(oracleConexion))
                    {
                        oracleCommand.ExecuteNonQuery();
                        var result = oracleCommand.Parameters[Constantes.ConsultaEtiquetasImp.pSalida].Value.ToString();

                        OracleDataReader reader = oracleCommand.ExecuteReader();

                        if (reader.HasRows)
                        {
                            tblEtiquetas.Columns.Add("TAGREFERENCE", typeof(string));
                            tblEtiquetas.Columns.Add("ZPLCODE", typeof(string));
                            Console.WriteLine("reader" + reader);
                            while (reader.Read())
                            {
                                Console.WriteLine("Ingresa");
                                tblEtiquetas.Rows.Add(reader.GetInt32(0).ToString(), (string)reader.GetOracleClob(1).Value);
                            }
                        }
                        else
                        {
                            if (!imprimir.flagAsignacion)
                            {
                                _logger.LogError(Constantes.MensajesError.errorOlaSinUsuarioAsignado);
                                throw new Exception(Constantes.MensajesError.errorOlaSinUsuarioAsignado);
                            }
                        }

                        reader.Dispose();
                        oracleCommand.Dispose();
                    }
                    else
                    {
                        _logger.LogError(Constantes.MensajesError.errorAbriendoConexion);
                        throw new Exception(Constantes.MensajesError.errorAbriendoConexion);
                    }

                    return tblEtiquetas;

                }
                catch (Exception ex)
                {
                    _logger.LogCritical("ObtenerZplsEtiquetasImprimirMasivo", ex);
                    throw new Exception(Constantes.MensajesError.errorConsultandoZpls + ex.Message, ex.InnerException);
                }
                finally
                {
                    oracleConexion.Close();
                }
            }
        }

        public DataTable ObtenerZplsEtiquetasImprimirMasivoXml(XmlEtq imprimir, string tag)
        {
            try
            {
                long idTransaccion = this._consultaRepository.InsertXml(imprimir.xml, tag);
                ScriptInfo scriptInfo = _cedisRepository.getScriptInfo(tag, new List<string> { idTransaccion.ToString() });
                DataTable resultado = _ejecucionesRepository.getResultado(scriptInfo);

                if (resultado.Rows.Count == 0)
                {
                    _logger.LogError(Constantes.MensajesError.errorConsultaVacioZpls);
                    throw new Exception(Constantes.MensajesError.errorConsultaVacioZpls);
                }

                return resultado;
            }
            catch (Exception ex)
            {
                _logger.LogCritical("ObtenerZplsEtiquetasImprimirMasivoXml", ex);
                throw new Exception(Constantes.MensajesError.errorConsultandoZpls + ex.Message, ex.InnerException);
            }
        }

        #endregion
    }
}
