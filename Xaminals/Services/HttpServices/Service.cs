using Xaminals.Infra.Context;

namespace Xaminals.Services.HttpServices
{
    public class Service
    {
        protected SingletonLoffersContext Context
        {
            get
            {
                return SingletonLoffersContext.Context;
            }
        }
    }
}