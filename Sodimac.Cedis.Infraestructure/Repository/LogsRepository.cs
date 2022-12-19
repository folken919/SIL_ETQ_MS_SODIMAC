using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using Sodimac.Cedis.Core.Entities;
using Sodimac.Cedis.Core.Interfaces.Repository;
using Sodimac.Cedis.Core.Utils;
using Sodimac.Cedis.Infraestructure.Data;
using System.Data;

namespace Sodimac.Cedis.Infraestructure.Repository
{
    public class LogsRepository : ILogsRepository
    {
        private readonly IConfiguration _configuration;
        private const string sqlsentencia = "select pkg_log_excepciones.prc_captura_error('{0}','{1}') from dual";
        string connection;

        public LogsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connection = _configuration.GetConnectionString("sgl_prod");
        }

        /// <summary>
        /// Metodo encargado de registrar los errores de la aplicación
        /// </summary>
        /// Autor: Ricardo Vivas
        /// Fecha: 05/05/2022
        /// <param name="item"></param>
        /// <returns></returns>
        public Response RegistrarLog(LogInfo item)
        {
            Response response = new Response { ok = false, mensaje = string.Empty, resultado = null };

            OracleConnection oracleConnection = new OracleConnection(connection.Desencriptar());
            try
            {
                if (SGLConnection.AbrirConexion(oracleConnection))
                {
                    OracleCommand cmd = new OracleCommand(string.Format(sqlsentencia, item.errNbr, item.errMsg), oracleConnection)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.ExecuteNonQuery();
                    response.ok = true;
                }
               
            }
            catch (OracleException oEx)
            {
                response.mensaje = oEx.Message;
            }
            finally
            {
                oracleConnection.Close();
            }

            return response;
        }
    }
}
