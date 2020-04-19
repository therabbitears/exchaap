using loffers.api.Controllers;
using Loffers.Server.Services;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace Loffers.Server.Controllers.Subscription
{

    [RoutePrefix("api/subscription")]
    public class SubscriptionController : ParentController
    {
        private readonly ILogger<SubscriptionController> _logger;
        private readonly PublisherService service;
        private readonly CategoriesService categoryService;

        public SubscriptionController()
        {
            _logger = new Logger<SubscriptionController>();
            service = new PublisherService();
            categoryService = new CategoriesService();
        }

        [HttpGet]
        [Route("detail")]
        public async Task<IHttpActionResult> Details()
        {
            try
            {
                var result = new
                {
                    Publishers = await service.GetPublishersForUser(UserId),
                    Categories = await categoryService.UserCategories(UserId)
                };

                return Ok(new HttpResult(result));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occurred while fetching information for the user.")));
            }
        }
    }
}
