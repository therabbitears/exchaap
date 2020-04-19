using System;
using System.Threading.Tasks;
using System.Web.Http;
using loffers.api.Controllers;
using loffers.api.Models.Generator;
using Loffers.Server.Errors;
using Loffers.Server.Services;
using Loffers.Server.ViewModels;

namespace Loffers.Server.Controllers
{
    [Authorize]
    [RoutePrefix("api/publishers")]
    public class PublisherController : ParentController
    {
        private readonly ILogger<PublisherController> _logger;
        private readonly PublisherService service;

        public PublisherController()
        {
            _logger = new Logger<PublisherController>(); 
            service = new PublisherService();

        }

        [Route("create")]
        [HttpPost]
        public async Task<IHttpActionResult> Save(PublisherViewModel publisher)
        {
            try
            {
                var result = await service.Save(new Publishers()
                {
                    Name = publisher.Name,
                    Description = publisher.Description,
                    Image = publisher.Image,
                    Active = true,
                    CreatedBy = UserId,
                    CreatedOn = DateTime.Now,
                    LastEditedBy = UserId,
                    LastEditedOn = DateTime.Now,
                    Id = Guid.NewGuid().ToString()
                });

                return Ok(new HttpResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occurred while saving publisher information.")));
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<IHttpActionResult> Update(PublisherViewModel publisher)
        {
            try
            {
                var result = await service.Save(publisher, UserId);
                return Ok(new HttpResult(result));
            }
            catch (PublisherNotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.NOTFOUNDERROR, "Publisher not found or doesn't belong to this account.")));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occurred while saving publisher information.")));
            }
        }

        [Route("locations")]
        [HttpGet]
        public async Task<IHttpActionResult> Locations()
        {
            try
            {
                var publisherForUser = await service.GetPublisherForUser(UserId);
                if (publisherForUser != null)
                {
                    var result = await service.GetAllLocationsForPublisher(publisherForUser.PublisherID);
                    return Ok(new HttpResult(result));
                }

                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.NOTFOUNDERROR, "Publisher not found or doesn't belong to this account.")));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occurred while fetching locations for the publisher.")));
            }
        }

        [Route("locations/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> Locations(string id)
        {
            try
            {
                var publisherForUser = await service.GetPublisherForUser(UserId);
                if (publisherForUser != null)
                {
                    var result = await service.GetSingleLocationForPublisher(publisherForUser.PublisherID, id);
                    return Ok(new HttpResult(result));
                }

                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.NOTFOUNDERROR, "Publisher not found or doesn't belong to this account.")));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occurred while fetching locations for the publisher.")));
            }
        }


        [Route("my")]
        [HttpGet]
        public async Task<IHttpActionResult> Publishers()
        {
            try
            {
                var publisherForUser = await service.GetPublishersForUser(UserId);
                return Ok(new HttpResult(publisherForUser));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occurred while fetching publishers for the user.")));
            }
        }

        [Route("one/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> OnePublisher(string Id)
        {
            try
            {
                var publisherForUser = await service.GetOnePublisher(Id);
                return Ok(new HttpResult(publisherForUser));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occurred while fetching publisher.")));
            }
        }


        [Route("location")]
        [HttpPost]
        public async Task<IHttpActionResult> Location(PublisherLocationViewModel publisher)
        {
            try
            {
                var result = await service.SaveLocation(publisher, UserId);
                return Ok(new HttpResult(result));
            }
            catch (PublisherNotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.NOTFOUNDERROR, "Publisher or location not found or doesn't belong to this account.")));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occurred while saving the location for the publisher.")));
            }
        }
    }
}
