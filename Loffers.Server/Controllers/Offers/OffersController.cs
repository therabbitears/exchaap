using Loffers.Server.Data;
using Loffers.Server.Errors;
using Loffers.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Loffers.Server.Controllers.Offers
{
    // [Authorize]
    [ApiController]
    [Route("api/offers")]
    public class OffersController : ParentController
    {
        OffersService service;
        PublisherService publisherService;

        public OffersController()
        {
            service = new OffersService();
            publisherService = new PublisherService();
        }

        [Route("my")]
        public async Task<IActionResult> MyOffers()
        {
            try
            {
                var token = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                var offerResult = await service.ForUser(token);
                return Ok(new HttpResult(offerResult));
            }
            catch (Exception ex)
            {
                return Ok(new HttpResult(new { }, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occured while fetching the offers for user.")));
            }
        }

        [Route("edit/{id}")]
        public async Task<IActionResult> MyOffers(string id)
        {
            try
            {
                var token = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                var offerResult = await service.GetOne(id, token, true);
                return Ok(new HttpResult(offerResult));
            }
            catch (Exception ex)
            {
                return Ok(new HttpResult(new { }, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occured while fetching the offer."), id));
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] OfferModel model)
        {
            try
            {
                var token = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                var publisherForUser = await publisherService.GetPublisherForUser(token);

                var offerResult = await service.Create(new Models.Offers()
                {
                    Active = true,
                    CreatedBy = token,
                    CreatedOn = DateTime.Now,
                    LastEditedBy = token,
                    LastEditedOn = DateTime.Now,
                    OfferDescription = model.Detail,
                    OfferHeadline = model.Heading,
                    TermsAndConditions = model.Terms,
                    ValidFrom = model.ValidFrom.Value,
                    ValidTill = model.ValidTo.Value,
                    Image = model.Image,
                    Publisher = publisherForUser,
                    Id = Guid.NewGuid().ToString(),
                    OfferCategories = model.Categories.Select(c => new Models.OfferCategories() { Id = c.Id }).ToList(),
                    OfferLocations = model.OfferLocations.Select(c => new Models.OfferLocations() { PublisherLocation = publisherService.GetOneLocation(c.Id).Result }).ToList(),
                });

                return Ok(new HttpResult(offerResult));
            }
            catch (Exception ex)
            {
                return Ok(new HttpResult(new { }, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occured while creating the offer.")));
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] OfferModel model)
        {
            try
            {
                var token = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                var offerResult = await service.Update(model, token);

                return Ok(new HttpResult(offerResult));
            }
            catch (Exception ex)
            {
                return Ok(new HttpResult(new { }, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occured while updating the offer.")));
            }
        }

        [AllowAnonymous]
        [Route("search")]
        public async Task<IActionResult> Search(double currentLat, double currentLong, int maximumDistanceInMeters, [FromQuery] string[] categories, string distanceUnit = "km")
        {
            try
            {
                var token = string.Empty;
                if (User != null && User.Claims.Any())
                    token = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;

                var offerResult = await service.FindOffersAround(currentLat, currentLong, maximumDistanceInMeters, categories, distanceUnit, token);
                return Ok(new HttpResult(offerResult));
            }
            catch (Exception ex)
            {
                return Ok(new HttpResult(new { }, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occured while searching into offers.")));
            }
        }

        [AllowAnonymous]
        [Route("details")]
        public async Task<IActionResult> Detail(string Id, double currentLat, double currentLong, string unit)
        {
            try
            {
                var offerResult = await service.Details(Id, currentLat, currentLong, unit);
                return Ok(new HttpResult(offerResult));
            }
            catch (Exception ex)
            {
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occured while fetching details for the offer.")));
            }
        }

        [HttpPost]
        [Route("star")]
        public async Task<IActionResult> Star([FromBody]StarModel model)
        {
            try
            {
                var user = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                var offerResult = await service.Star(model.token, model.locationToken, user);
                return Ok(new HttpResult(offerResult));
            }
            catch (OfferNotFoundException ex)
            {
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.NOTFOUNDERROR, "Offer or location not found.")));
            }
            catch (Exception ex)
            {
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occured while fetching details for the offer.")));
            }
        }

        [Route("starred")]
        public async Task<IActionResult> MyStarred(double currentLat, double currentLong, string distanceUnit = "km")
        {
            try
            {
                var token = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                var offerResult = await service.Starred(currentLat, currentLong, token, distanceUnit);
                return Ok(new HttpResult(offerResult));
            }
            catch (Exception ex)
            {
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occured while fetching offers for the user.")));
            }
        }

        [Route("report")]
        [HttpPost]
        public async Task<IActionResult> Report([FromBody]OfferReportModel model)
        {
            try
            {
                var token = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                var offerResult = await service.Report(model, token);
                return Ok(new HttpResult(offerResult));
            }
            catch (OfferNotFoundException ex)
            {
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.NOTFOUNDERROR, "Offer not found.")));
            }
            catch (Exception ex)
            {
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occured while reporting the offer.")));
            }
        }
    }
}
