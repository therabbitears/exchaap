using loffers.api;
using loffers.api.Data;
using loffers.api.Services;
using Loffers.Server.Data;
using Loffers.Server.Services;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace Loffers.Server.Controllers.Offers
{
    [Authorize]
    [RoutePrefix("api/users")]
    public class UsersController : ParentController
    {
        UsersService service;
        private readonly PublisherService publisherService;
        private readonly CategoriesService categoryService;
        private ApplicationUserManager _userManager;

        public UsersController()
        {
            service = new UsersService();
            publisherService = new PublisherService();
            categoryService = new CategoriesService();
        }

        public UsersController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
            service = new UsersService();
            publisherService = new PublisherService();
            categoryService = new CategoriesService();
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpPut]
        [Route("updateconfiguration")]
        public async Task<IHttpActionResult> Update([FromBody] ConfugurationModel model)
        {
            try
            {
                string nameIdentifier = ClaimTypes.NameIdentifier;
                var claimUsers = User as ClaimsPrincipal;
                var userId = claimUsers.Claims.FirstOrDefault(c => c.Type == nameIdentifier);
                if (userId != null)
                {
                    var offerResult = await service.Update(model, userId.Value);
                    return Ok(new HttpResult(offerResult));
                }

                return Ok(new HttpResult(1));
            }
            catch (Exception ex)
            {
                return Ok(new HttpResult(new { }, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occurred while updating the configurations.")));
            }
        }

        [HttpPut]
        [Route("updatesnapshot")]
        public async Task<IHttpActionResult> UpdateSnapshot([FromBody] SnapshotModel model)
        {
            try
            {
                var offerResult = await service.UpdateSnapshot(model, UserId);
                return Ok(new HttpResult(offerResult));
            }
            catch (Exception ex)
            {
                return Ok(new HttpResult(new { }, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occurred while updating the snapshot.")));
            }
        }

        //[HttpGet]
        //[Route("load")]
        //public async Task<IHttpActionResult> Load()
        //{
        //    try
        //    {
        //        string nameIdentifier = ClaimTypes.NameIdentifier;
        //        var claimUsers = User as ClaimsPrincipal;
        //        var userId = claimUsers.Claims.FirstOrDefault(c => c.Type == nameIdentifier);
        //        if (userId != null)
        //        {
        //            var offerResult = await service.Get<ConfugurationModel>(userId.Value);
        //            return Ok(new HttpResult(offerResult));
        //        }

        //        return Ok(new HttpResult(1));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new HttpResult(new { }, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occurred while updating the configurations.")));
        //    }
        //}


        [HttpGet]
        [Route("settings")]
        public async Task<IHttpActionResult> Settings()
        {
            var userName = User.Identity.Name;
            var user = await UserManager.FindByNameAsync(userName);
            if (user != null)
            {
                return Ok(new HttpResult(new
                {
                    user.Name,
                    user.Email,
                    user.PhoneNumber,
                    Publishers = await publisherService.GetPublishersForUser(UserId),
                    Categories = await categoryService.UserCategories(UserId),
                    Configuration = await service.Get(UserId)
                }));
            }

            var notFoundResult = new
            {
                status = 404,
                userName = string.Empty
            };

            return Ok(new HttpResult(notFoundResult, true, HttpResult.SingleError(HttpResult.ErrorCodes.USERDOESNTEXIST, "User doesn't exist.")));

        }
    }
}