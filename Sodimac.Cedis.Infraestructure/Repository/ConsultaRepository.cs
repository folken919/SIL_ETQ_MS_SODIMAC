using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Sodimac.Cedis.Core.Interfaces.Repository;
using Sodimac.Cedis.Infraestructure.Data;
using Sodimac.Cedis.Core.Utils;

namespace Sodimac.Cedis.Infraestructure.Repository
{
    public class ConsultaRepository : IConsultaRepository
    {
        #region instanciar inyeccion de dependencias
        private readonly ICedisRepository cedisRepository;
        #endregion

        public ConsultaRepository(ICedisRepository cedisRepository)
        {
            this.cedisRepository = cedisRepository;
        }

        #region metodos de la clase
        public long InsertXml(string xmlString, string tag)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(xmlString);
            string cadena = this.cedisRepository.GetConexion(tag);
            using (OracleConnection oracleConnection = new OracleConnection(cadena))
            {
                try
                {
                    OracleCommand command = new OracleCommand();

                    //Obtener Secuencia
                    command.Connection = oracleConnection;
                    oracleConnection.Open();
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT SEQ_ETQ_TRANSAC_XML.NEXTVAL FROM DUAL";
                    long id_transaccion = Convert.ToInt64(command.ExecuteScalar());

                    //Insert
                    command.CommandText = "INSERT INTO TBL_ETQ_TRANSAC_XML (ID_TRANSACCIONES, XML_PARAM, USUARIO_CREACION, FECHA_CREACION) VALUES (:id_transaccion, :xml, :usuario, :fecha)";
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(":id_transaccion", OracleDbType.Long, id_transaccion, ParameterDirection.Input);
                    command.Parameters.Add(":xml", OracleDbType.XmlType, xml.InnerXml, ParameterDirection.Input);
                    command.Parameters.Add(":usuario", OracleDbType.Varchar2, "API", ParameterDirection.Input);
                    command.Parameters.Add(":fecha", OracleDbType.Date, DateTime.Now, ParameterDirection.Input);
                    command.ExecuteNonQuery();
                    return id_transaccion;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    throw ex;
                }
                finally
                {
                    oracleConnection.Close();
                }

            }
        }
        #endregion
    }
}
