using System;
using System.Collections.Generic;
using System.Text;

namespace Sodimac.Cedis.Core.Excepciones
{
    public class ExcepcionBase: Exception
    {
        #region Variables

        private readonly object _referencia;

        #endregion

        #region Encapsulamiento


        public object Referencia { get { return _referencia; } }

        #endregion

        #region Constructor


        protected ExcepcionBase(string message, object referencia, Exception innerException = null)
            : base(message, innerException)
        {
            _referencia = referencia;
        }

        protected ExcepcionBase(string message)
            : base(message)
        {

        }

        #endregion
    }
}
