using loffers.api.Data;
using loffers.api.Services;
using Loffers.Server.Controllers;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace loffers.api.Controllers.Chats
{
    [RoutePrefix("api/conversations")]
    public class ChatsController : ParentController
    {
        private ChatService service;
        public ChatsController()
        {
            service = new ChatService();
        }

        [Route("start/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> Start(string id, string groupName = null)
        {
            try
            {
                var chatResult = await service.Start(id, groupName, UserId);
                return Ok(new HttpResult(chatResult));
            }
            catch (Exception ex)
            {
                return Ok(new HttpResult(new { }, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occurred while fetching the offer."), id));
            }
        }

        [Route("loadchat/{name}")]
        [HttpGet]
        public async Task<IHttpActionResult> LoadChat(string name)
        {
            try
            {
                var chatResult = await service.LoadChat(name, UserId);
                return Ok(new HttpResult(chatResult));
            }
            catch (Exception ex)
            {
                return Ok(new HttpResult(new { }, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occurred while fetching the offer."), name));
            }
        }


        [Route("all")]
        [HttpGet]
        public async Task<IHttpActionResult> LoadChats()
        {
            try
            {
                var chatResult = await service.Chats(UserId);
                return Ok(new HttpResult(chatResult));
            }
            catch (Exception ex)
            {
                return Ok(new HttpResult(new { }, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occurred while fetching the offer.")));
            }
        }

        [Route("markread")]
        [HttpPost]
        public async Task<IHttpActionResult> MarkAsRead(ChatGroupModel model)
        {
            try
            {
                var chatResult = await service.MarkAsRead(model.GroupName, UserId);
                return Ok(new HttpResult(chatResult));
            }
            catch (Exception ex)
            {
                return Ok(new HttpResult(new { }, true, HttpResult.SingleError(HttpResult.ErrorCodes.GENERALERROR, "An error occurred while fetching the offer.")));
            }
        }
    }
}