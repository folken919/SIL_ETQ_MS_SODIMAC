using System.Collections.Generic;
using Sodimac.Cedis.Core.Entities;

namespace Sodimac.Cedis.Core.Interfaces.Repository
{
    //TODO: no olvidar implementar metodos en las interfaces
    public interface ICedisRepository
    {
        public string GetConexion(string tag);
        ScriptInfo getScriptInfo(string tag, List<string> parametros);

    }
}