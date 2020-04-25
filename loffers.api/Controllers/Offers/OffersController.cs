using loffers.api.Models.Generator;
using Loffers.Server.Data;
using Loffers.Server.Errors;
using Loffers.Server.Services;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Loffers.Server.Controllers.Offers
{
    [Authorize]
    [RoutePrefix("api/offers")]
    public class OffersController : ParentController
    {
        OffersService service;
        PublisherService publisherService;

        public OffersController()
        {
            service = new OffersService();
            publisherService = new PublisherService();
        }

        [Route("edit/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> MyOffers(string id)
        {
            try
            {
                var offerResult = await service.GetOne(id, UserId, true);
                return Ok(new HttpResult(offerResult));
            }
            catch (Exception ex)
            {
                return Ok(new HttpResult(new { }, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occurred while fetching the offer."), id));
            }
        }

        [Route("create")]
        [HttpPost]
        public async Task<IHttpActionResult> Create([FromBody] OfferModel model)
        {
            try
            {
                string fileName = HttpContext.Current.Server.MapPath("~/offers/" + DateTime.Now.Ticks + ".txt");
                File.WriteAllText(fileName, JsonConvert.SerializeObject(model));
                var offerResult = await service.Create(model, UserId);
                return Ok(new HttpResult(offerResult));
            }
            catch (Exception ex)
            {
                return Ok(new HttpResult(new { }, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, ex.Message)));
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<IHttpActionResult> Update([FromBody] OfferModel model)
        {
            try
            {
                var offerResult = await service.Update(model, UserId);

                return Ok(new HttpResult(offerResult));
            }
            catch (Exception ex)
            {
                return Ok(new HttpResult(new { }, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occurred while updating the offer.")));
            }
        }

        [Route("search")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<object> Search(double currentLat, double currentLong, int maximumDistanceInMeters, [FromUri] string[] categories, string distanceUnit = "km", int pageNumber = 0)
        {
            try
            {
                var token = string.Empty;
                if (User.Identity.IsAuthenticated)
                    token = UserId;

                var offerResult = await service.FindOffersAround(currentLat, currentLong, maximumDistanceInMeters, categories, distanceUnit, token, pageNumber);
                return Ok(new HttpResult(offerResult));
            }
            catch (Exception ex)
            {
                return Ok(new HttpResult(new { }, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occurred while searching into offers.")));
            }
        }

        [Route("details")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Detail(string Id = "", double currentLat = 0, double currentLong = 0, string unit = "m")
        {
            try
            {
                var offerResult = await service.Details(Id, currentLat, currentLong, unit);
                return Ok(new HttpResult(offerResult));
            }
            catch (Exception ex)
            {
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occurred while fetching details for the offer.")));
            }
        }

        [Route("star")]
        [HttpPost]
        public async Task<IHttpActionResult> Star([FromBody]StarModel model)
        {
            try
            {
                var offerResult = await service.Star(model.token, model.locationToken, UserId);
                return Ok(new HttpResult(offerResult));
            }
            catch (OfferNotFoundException ex)
            {
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.NOTFOUNDERROR, "Offer or location not found.")));
            }
            catch (Exception ex)
            {
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occurred while fetching details for the offer.")));
            }
        }

        [Route("my")]
        [HttpGet]
        public async Task<IHttpActionResult> MyOffers()
        {
            try
            {
                var offerResult = await service.ForUser(UserId);
                return Ok(new HttpResult(offerResult));
            }
            catch (Exception ex)
            {
                return Ok(new HttpResult(new { }, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occurred while fetching the offers for user.")));
            }
        }

        [Route("starred")]
        [HttpGet]
        public async Task<IHttpActionResult> MyStarred([FromUri] double currentLat, [FromUri] double currentLong, [FromUri] string distanceUnit = "km")
        {
            try
            {
                var offerResult = await service.Starred(currentLat, currentLong, UserId, distanceUnit);
                return Ok(new HttpResult(offerResult));
            }
            catch (Exception ex)
            {
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occurred while fetching offers for the user.")));
            }
        }

        [Route("report")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Report([FromBody]OfferReportModel model)
        {
            try
            {
                var offerResult = await service.Report(model, UserId);
                return Ok(new HttpResult(offerResult));
            }
            catch (OfferNotFoundException ex)
            {
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.NOTFOUNDERROR, "Offer not found.")));
            }
            catch (Exception ex)
            {
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occurred while reporting the offer.")));
            }
        }
    }
}
