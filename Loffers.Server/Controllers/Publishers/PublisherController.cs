using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Loffers.Server.Errors;
using Loffers.Server.Services;
using Loffers.Server.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Loffers.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/publishers")]
    public class PublisherController : ParentController
    {
        private readonly ILogger<PublisherController> _logger;
        private readonly PublisherService service;

        public PublisherController(ILogger<PublisherController> logger)
        {
            _logger = logger;
            service = new PublisherService();
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Save(PublisherViewModel publisher)
        {
            var token = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;

            try
            {
                var result = await service.Save(new Models.Publishers()
                {
                    Name = publisher.Name,
                    Description = publisher.Description,
                    Image = publisher.Image,
                    Active = true,
                    CreatedBy = token,
                    CreatedOn = DateTime.Now,
                    LastEditedBy = token,
                    LastEditedOn = DateTime.Now,
                    Id = Guid.NewGuid().ToString()
                });

                return Ok(new HttpResult(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occured while saving publisher information.")));
            }
        }

        [Route("update")]
        [HttpPut]
        public async Task<IActionResult> Update(PublisherViewModel publisher)
        {
            try
            {
                var token = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                var result = await service.Save(publisher, token);
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
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occured while saving publisher information.")));
            }
        }

        [Route("locations")]
        public async Task<IActionResult> Locations()
        {
            try
            {
                var token = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                var publisherForUser = await service.GetPublisherForUser(token);
                if (publisherForUser != null)
                {
                    var result = await service.GetAllLocationsForPublisher(publisherForUser.PublisherId);
                    return Ok(new HttpResult(result));
                }

                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.NOTFOUNDERROR, "Publisher not found or doesn't belong to this account.")));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occured while fetching locations for the publisher.")));
            }
        }


        [Route("my")]
        public async Task<IActionResult> Publishers()
        {
            try
            {
                var token = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                var publisherForUser = await service.GetPublishersForUser(token);
                return Ok(new HttpResult(publisherForUser));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occured while fetching publishers for the user.")));
            }
        }

        [Route("one/{id}")]
        public async Task<IActionResult> OnePublisher(string Id)
        {
            try
            {
                var publisherForUser = await service.GetOnePublisher(Id);
                return Ok(new HttpResult(publisherForUser));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occured while fetching publisher.")));
            }
        }


        [Route("location")]
        [HttpPost]
        public async Task<IActionResult> Location(PublisherLocationViewModel publisher)
        {
            try
            {
                var token = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
                var result = await service.SaveLocation(publisher, token);
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
                return Ok(new HttpResult(null, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occured while saving the location for the publisher.")));
            }
        }
    }
}
