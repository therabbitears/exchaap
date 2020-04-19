using Loffers.Server.Models;

namespace Loffers.Server.Services
{
    public class BaseService
    {
        protected LoffersContext context;

        public BaseService()
        {
            context = new LoffersContext();
        }
    }
}
