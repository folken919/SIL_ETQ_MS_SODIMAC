using Sodimac.Cedis.Core.DTOs;
using Sodimac.Cedis.Core.Entities;

namespace Sodimac.Cedis.Core.Interfaces.Services
{
    //TODO: no olvidar implementar metodos en las interfaces
    public interface ICedisService
    {
        Response Consultar(Request item);
    }
}