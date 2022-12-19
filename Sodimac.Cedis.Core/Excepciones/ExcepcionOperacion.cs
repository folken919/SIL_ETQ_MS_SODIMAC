using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Sodimac.Cedis.Core.Excepciones
{
    public class ExcepcionOperacion: ExcepcionBase
    {
        #region Variables

        private string _operacion;
        private int _codigo;

        #endregion

        #region Encapsulamiento

        /// <summary>
        /// Propiedad que permite obtener/establecer la operación realizada que lanzó la excepción
        /// </summary>
        public string Operacion
        {
            get { return _operacion; }
            set { _operacion = value; }
        }
        /// <summary>
        /// Propiedad que permite obtener/establecer el código de la excepción que se generó
        /// </summary>
        public int Codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }

        #endregion

        #region Constructor


        public ExcepcionOperacion(string message, int codigo, object data, Exception innerException = null)
            : base(message, data, innerException)
        {
            this._codigo = codigo;
            this._operacion = ObtenerMetodo();


        }

        #endregion

        #region Métodos

        public string ObtenerMetodo()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(2);

            return sf.GetMethod().Name;
        }

        #endregion
    }
}
