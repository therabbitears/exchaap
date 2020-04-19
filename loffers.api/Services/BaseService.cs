using loffers.api.Models.Generator;
using System;

namespace Loffers.Server.Services
{
    public class BaseService:IDisposable
    {
        protected LoffersContext context;

        public BaseService(LoffersContext existingContext = null)
        {
            context = existingContext ?? new LoffersContext();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
