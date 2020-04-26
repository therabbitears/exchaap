using loffers.api.Models.Generator;
using System;
using System.Configuration;

namespace Loffers.Server.Services
{
    public class BaseService:IDisposable
    {
        protected LoffersContext context;

        public BaseService(LoffersContext existingContext = null)
        {
            context = existingContext ?? new LoffersContext();
        }

        public string BaseUrlToSaveImages
        {
            get
            {
                return ConfigurationManager.AppSettings["BaseUrlToSaveImages"];
            }
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
