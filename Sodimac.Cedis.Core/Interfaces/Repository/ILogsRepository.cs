using Sodimac.Cedis.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sodimac.Cedis.Core.Interfaces.Repository
{
    public interface ILogsRepository
    {
        public Response RegistrarLog(LogInfo item);
    }
}
