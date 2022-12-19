using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Sodimac.Cedis.Infraestructure.Data
{
    public class SGLConnection : IDisposable
    {
        private static OracleConnection oracleConnection;

        private SGLConnection() { }

        public static OracleConnection GetConnection(string ConnectionString)
        {
            if (oracleConnection == null)
                oracleConnection = new OracleConnection(ConnectionString);

            return oracleConnection;
        }

        /// <summary>
        /// Metodo encargado de abrir la conexión a la base de datos y generar n reintentos ante algun fallo
        /// </summary>
        /// <param name="objConexion"></param>
        /// Autor: Ricardo Vivas
        /// Fecha: 05/01/2022
        /// Empresa: Asesoftware
        /// <returns></returns>
        public static bool AbrirConexion(OracleConnection objConexion)
        {
            int reintentos = 3;
            int tiempoReintento = 10000;
            try
            {
                int num = 0;
                bool flag = false;
                while (num < reintentos && !flag)
                {
                    try
                    {
                        if (objConexion.State != ConnectionState.Open)
                        {
                            objConexion.Open();
                            flag = true;
                        }

                        num++;
                    }
                    catch (Exception)
                    {
                        num++;
                        if (num < reintentos)
                        {
                            Thread.Sleep(tiempoReintento);
                        }
                    }
                }

                return flag;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Dispose()
        {
            oracleConnection.Dispose();
            oracleConnection = null;
        }
    }
}
