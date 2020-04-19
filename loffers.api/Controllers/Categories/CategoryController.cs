using System.Threading.Tasks;
using Loffers.Server.Services;
using Loffers.Server.Data;
using System.Collections.Generic;
using System.Web.Http;
using loffers.api.Controllers;

namespace Loffers.Server.Controllers.Categories
{
    [RoutePrefix("api/categories")]
    public class CategoryController : ParentController
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly CategoriesService service;

        public CategoryController()
        {
            _logger = new Logger<CategoryController>();
            service = new CategoriesService();
        }

        [Route("list")]
        [HttpGet]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var result = await service.GetAll();
                return Ok(new HttpResult(result));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occurred while fetching categories.")));
            }
        }

        [Route("offer")]
        [HttpGet]
        public async Task<IHttpActionResult> GetOffer()
        {
            try
            {
                var result = await service.GetAll();
                return Ok(new HttpResult(result));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occurred while fetching categories.")));
            }
        }

        [HttpGet]
        [Route("subscribed")]
        [Authorize]        
        public async Task<IHttpActionResult> MyCategories()
        {
            try
            {
                var result = await service.UserCategories(UserId);
                return Ok(new HttpResult(result));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occurred while fetching subscribed categories.")));
            }
        }

        [HttpPut]
        [Route("save")]
        [Authorize]
        public async Task<IHttpActionResult> SaveCategories([FromBody] List<UserCategoryModel> model)
        {
            try
            {
                var result = await service.SaveUserCategories(model, UserId);
                return Ok(new HttpResult(result));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occurred while saving subscribed categories.")));
            }
        }
    }
}
