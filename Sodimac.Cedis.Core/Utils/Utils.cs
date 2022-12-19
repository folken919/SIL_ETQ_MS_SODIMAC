
using Sodimac.Cedis.Core.DTOs;
using Sodimac.Cedis.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Sodimac.Cedis.Core.Utils
{
    public static class Utils
    {
        public static string ToDynamicParams(this List<string> items)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("#");
            foreach (string item in items)
            {
                sb.Append($"{item}#");
            }
            return sb.ToString();
        }
        public static string ToStringParams(this List<string> items)
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append("");
            foreach (string item in items)
            {
                sb.Append($"{item},");
            }
            return sb.ToString();
        }
        public static string ToString(this DataRow dr, string fieldName)
        {
            return dr[fieldName].ToString();
        }

        public static Int64 ToNumber(this DataRow dr, string fieldName)
        {
            return Convert.ToInt64(dr[fieldName].ToString());
        }

        public static bool IsEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace (value);
        }

        /// <summary>
        /// Metodo encargado de separar por comas las impresoras que vienen en un listado
        /// </summary>
        /// <param name="impresoras"></param>
        /// <returns></returns>
        public static string ObtenerStringIps(List<Impresora> impresoras)
        {
            var ips = string.Empty;
            foreach (Impresora imp in impresoras)
            {
                ips = string.Concat(ips, imp.ip, ",");
            }
            return ips.Substring(0, ips.Length - 1);
        }

        /// <summary>
        /// Método recursivo que valida que las etiquetas sean suficientes para distribuirlas entre las impresoras definidas.
        /// Si no hay suficientes etiquetas (se podría generar un error en la distribución), se eliminan impresoras de la lista para evitar error de datos nulos.
        /// </summary>
        /// Autor: jvivas
        /// Fecha: 28/04/2022
        /// <param name="impresoras"></param>
        /// <param name="cantidadEtiquetas"></param>
        /// <returns></returns>
        public static List<Impresora> ValidarCantidadImpresoras(List<Impresora> impresoras, int cantidadEtiquetas)
        {
            if (Convert.ToInt32(Math.Ceiling(Convert.ToDouble(cantidadEtiquetas) / impresoras.Count)) < impresoras.Count)
            {
                impresoras.RemoveAt(impresoras.Count - 1);
                impresoras = ValidarCantidadImpresoras(impresoras, cantidadEtiquetas);
            }
            return impresoras;
        }

        /// <summary>
        /// Metodo encargado de desencriptar la cadena de conexion
        /// </summary>
        /// <param name="Cadena"></param>
        /// <returns></returns>
        public static string Desencriptar(this string Cadena)
        {
            string s = "Sodimac_SGL*";
            try
            {
                if (Cadena == null || string.IsNullOrEmpty(Cadena))
                {
                    return null;
                }

                MemoryStream memoryStream = new MemoryStream();
                RC2CryptoServiceProvider rC2CryptoServiceProvider = new RC2CryptoServiceProvider();
                byte[] bytes = Encoding.ASCII.GetBytes(s);
                byte[] array = Convert.FromBase64String(Cadena);
                ((SymmetricAlgorithm)(object)rC2CryptoServiceProvider).Mode = CipherMode.CBC;
                ICryptoTransform transform = ((SymmetricAlgorithm)(object)rC2CryptoServiceProvider).CreateDecryptor(bytes, Encoding.ASCII.GetBytes(s));
                CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
                cryptoStream.Write(array, 0, array.Length);
                cryptoStream.FlushFinalBlock();
                cryptoStream.Close();
                return Encoding.ASCII.GetString(memoryStream.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
