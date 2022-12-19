using Sodimac.Cedis.Core.Entities;
using System.Data;

namespace Sodimac.Cedis.Core.Interfaces.Repository
{
    //TODO: no olvidar implementar metodos en las interfaces
    public interface IEjecucionesRepository
    {
        DataTable getResultado(ScriptInfo request);
        Response Ejecutar(ScriptInfo request);
    }
}