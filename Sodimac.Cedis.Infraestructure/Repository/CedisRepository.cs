using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using Sodimac.Cedis.Core.Entities;
using Sodimac.Cedis.Core.Interfaces.Repository;
using Sodimac.Cedis.Core.Utils;
using Sodimac.Cedis.Infraestructure.Data;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using ola_automatica.worker.core.Interfaces;

namespace Sodimac.Cedis.Infraestructure.Repository
{
    public class CedisRepository : ICedisRepository
    {
        private const string sqlSentencia = "select pkg_sgl_sql.fnc_get_sentencia('{0}','{1}','#') from dual";
        private const string sqlDatabase = "select pkg_sgl_sql.fnc_get_conexion_tag('{0}') from dual";
        private readonly IConfiguration _configuration;
        private readonly IConsolaSodimac _logger;
        string connection;

        public CedisRepository(IConfiguration configuration , IConsolaSodimac consola)
        {
            _configuration = configuration;
            connection = _configuration.GetConnectionString("sgl_prod");
            _logger = consola;
        }

        /// <summary>
        /// Metodo encargado de ejecutar una sentencia de sql
        /// </summary>
        /// <param name="Tag"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        private string GetSentencia(string Tag, string Parameters)
        {
            string sentencia = string.Empty;

            OracleConnection oracleConnection = new OracleConnection(connection.Desencriptar());
            try
            {
                OracleConnection.ClearPool(oracleConnection);
                if (SGLConnection.AbrirConexion(oracleConnection))
                {
                    string sqlConsulta = string.Format(sqlSentencia, Tag, Parameters);
                    OracleCommand cmd = new OracleCommand(sqlConsulta, oracleConnection);
                    sentencia = cmd.ExecuteScalar().ToString();
                }
                return sentencia;
            }
            catch(Exception ex)
            {
                _logger.LogCritical("GetSentencia",ex);
                throw ex;
            }
            finally
            {
                oracleConnection.Close();
            }          
        }

        /// <summary>
        /// Metodo encargado de ejecutar una sentencia para obtener la conexion de un tag de BD
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public string GetConexion(string tag)
        {
            string sentencia = string.Empty;
            OracleConnection oracleConnection = new OracleConnection(connection.Desencriptar());
            try
            {
                OracleConnection.ClearPool(oracleConnection);
                if (SGLConnection.AbrirConexion(oracleConnection))
                {
                    string sqlConsulta = string.Format(sqlDatabase, tag);
                    OracleCommand cmd = new OracleCommand(sqlConsulta, oracleConnection);
                    sentencia = cmd.ExecuteScalar().ToString();
                }

                if (sentencia == "DEFAULT")
                    sentencia = connection;

                return sentencia.Desencriptar();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Getconexion tag: {tag}");
                _logger.LogCritical("GetConexion", ex);
                throw ex;
            }
            finally
            {
                oracleConnection.Close();
            }
           
        }

        /// <summary>
        /// Metodo encargado de ejecutar una sentencia de bd obteniendo la conexion correspondiente
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public ScriptInfo getScriptInfo(string tag, List<string> parametros)
        {
            try
            {
                ScriptInfo item = new ScriptInfo { Conexion = string.Empty, Parametros = parametros.ToDynamicParams(), Sentencia = string.Empty, Tag = tag };
                item.Sentencia = GetSentencia(item.Tag, item.Parametros);
                if (!string.IsNullOrWhiteSpace(item.Sentencia))
                    item.Conexion = GetConexion(item.Tag);

                return item;
            }
            catch(Exception ex)
            {
                _logger.LogError($"getScriptInfo tag: {tag}");
                _logger.LogCritical("getScriptInfo", ex);
                throw ex;
            }
            
        }
    }
}
