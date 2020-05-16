using Loffers.Server.Services;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace loffers.api.Services
{
    public class LocationService : BaseService
    {
        public async Task<object> Search(string keyword)
        {
            return await context.Locations.Where(c => c.Explorable && c.Name.StartsWith(keyword)).Select(c => new { c.Name, Landmark = c.DisplayAddress, c.Lat, c.Long }).Take(15).ToListAsync();
        }
    }
}