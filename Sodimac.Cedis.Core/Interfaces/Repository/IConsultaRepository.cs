using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sodimac.Cedis.Core.Interfaces.Repository
{
    public interface IConsultaRepository
    {
        public long InsertXml(string xmlString, string tag);
    }
}
