using Oracle.ManagedDataAccess.Client;
using System.Data;
using Sodimac.Cedis.Core.Entities;
using Sodimac.Cedis.Core.Interfaces.Repository;
using System;

namespace Sodimac.Cedis.Infraestructure.Repository
{
    public class EjecucionesRepository : IEjecucionesRepository
    {
        public DataTable getResultado(ScriptInfo request)
        {
            //Console.WriteLine("Tag: " + request.Tag);
            //Console.WriteLine("sentencia: " + request.Sentencia);
            //Console.WriteLine("parametros: " + request.Parametros);
            //Console.WriteLine("conex: " + request.Conexion);
            using (OracleConnection oracle = new OracleConnection(request.Conexion))
            {
                try
                {
                    oracle.Open();
                    OracleCommand cmd = new OracleCommand(request.Sentencia, oracle);
                    DataTable dt = new DataTable("resultado");
                    dt.Load(cmd.ExecuteReader());
                    return dt;
                }
                catch (OracleException)
                {
                    return null;
                }
                finally
                {
                    oracle.Close();
                }
            }
        }
        public Response Ejecutar(ScriptInfo request)
        {
            Response response = new Response { ok = false, mensaje = string.Empty, resultado = null };
            using (OracleConnection oracle = new OracleConnection(request.Conexion))
            {
                try
                {
                    oracle.Open();
                    OracleCommand cmd = new OracleCommand(request.Sentencia, oracle);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                    response.ok = true;
                }
                catch (OracleException oEx)
                {
                    response.mensaje = oEx.Message;
                }
                finally
                {
                    oracle.Close();
                }
            }
            return response;
        }
    }
}
