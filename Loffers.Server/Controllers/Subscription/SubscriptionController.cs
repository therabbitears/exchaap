using Loffers.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Loffers.Server.Controllers.Subscription
{
    [ApiController]
    [Route("api/subscription")]
    public class SubscriptionController : ParentController
    {
        private readonly ILogger<PublisherController> _logger;
        private readonly PublisherService service;
        private readonly CategoriesService categoryService;

        public SubscriptionController(ILogger<PublisherController> logger)
        {
            _logger = logger;
            service = new PublisherService();
            categoryService = new CategoriesService();
        }

        [Route("detail")]
        public async Task<IActionResult> Details()
        {
            try
            {
                var token = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                var result = new
                {
                    Publishers = await service.GetPublishersForUser(token),
                    Categories = await categoryService.UserCategories(token)
                };

                return Ok(new HttpResult(result));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occured while fetching information for the user.")));
            }
        }
    }
}
