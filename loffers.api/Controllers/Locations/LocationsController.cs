using loffers.api.Services;
using Loffers.Server.Controllers;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace loffers.api.Controllers.Locations
{
    [RoutePrefix("api/locations")]
    public class LocationsController : ParentController
    {
        private LocationService service;
        public LocationsController()
        {
            service = new LocationService();
        }

        [AllowAnonymous]
        [Route("search")]
        [HttpGet]
        public async Task<IHttpActionResult> Search(string keyword)
        {
            try
            {
                var chatResult = await service.Search(keyword);
                return Ok(new HttpResult(chatResult));
            }
            catch (Exception ex)
            {
                return Ok(new HttpResult(new { }, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occurred while fetching the locations.")));
            }
        }
    }
}