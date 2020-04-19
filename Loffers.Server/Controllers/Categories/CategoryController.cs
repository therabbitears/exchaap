using System.Threading.Tasks;
using Loffers.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;
using Loffers.Server.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Loffers.Server.Controllers.Categories
{
    //
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ParentController
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly CategoriesService service;
        public CategoryController(ILogger<CategoryController> logger)
        {
            _logger = logger;
            service = new CategoriesService();
        }

        [Route("list")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await service.GetAll();
                return Ok(new HttpResult(result));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occured while fetching categories.")));
            }
        }

        [Route("offer")]
        [HttpGet]
        public async Task<IActionResult> GetOffer()
        {
            try
            {
                var result = await service.GetAll();
                return Ok(new HttpResult(result));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occured while fetching categories.")));
            }
        }

        [Authorize]
        [Route("subscribed")]
        public async Task<IActionResult> MyCategories()
        {
            try
            {
                var token = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                var result = await service.UserCategories(token);
                return Ok(new HttpResult(result));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occured while fetching subscribed categories.")));
            }
        }

        [Authorize]
        [HttpPut]
        [Route("save")]
        public async Task<IActionResult> SaveCategories([FromBody] List<UserCategoryModel> model)
        {
            try
            {
                var token = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                var result = await service.SaveUserCategories(model, token);
                return Ok(new HttpResult(result));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occured while saving subscribed categories.")));
            }
        }
    }
}
