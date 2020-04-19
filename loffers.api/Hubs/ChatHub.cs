using loffers.api.Services;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace loffers.api.Hubs
{
    public class ChatHub : Hub
    {
        public enum MessageType
        {
            Mesaage = 1,
            Notification = 2,
            CurrentTypingStatus = 3,
            ChatClosed = 4,
            FileSending = 5
        }

        #region Data Members

        static List<UserDetail> ConnectedUsers = new List<UserDetail>();
        static List<MessageDetail> CurrentMessage = new List<MessageDetail>();

        #endregion

        #region Methods

        public override Task OnConnected()
        {
            SendConnectedMessage(DateTime.UtcNow);
            return base.OnConnected();
        }

        public Task SendConnectedMessage(DateTime utcNow)
        {
            return Task.Run(async () =>
            {
                await Clients.Caller.onConnected(utcNow);
            });
        }

        public void Connect(string userName)
        {
            var id = Context.ConnectionId;
            if (ConnectedUsers.Count(x => x.ConnectionId == id) == 0)
            {
                ConnectedUsers.Add(new UserDetail { ConnectionId = id, UserName = userName });

                // send to caller
                Clients.Caller.onConnected(id, userName, ConnectedUsers, CurrentMessage);

                // send to all except caller client
                Clients.AllExcept(id).onNewUserConnected(id, userName);
            }
            else { OnReconnected(); }
        }

        [ValidateInput(true)]
        public async Task JoinToGroup(string groupName)
        {
            await Groups.Add(Context.ConnectionId, groupName);
        }

        [ValidateInput(true)]
        public Task SendToGroup(string message, int messageType, string groupName)
        {
            return Task.Run(async () =>
            {
                //await Clients.All.messageReceived(currentUserName, message, messageType, currentUserID, groupID);
                // Broad cast message
                await Clients.Group(groupName).messageReceived(message, messageType, groupName, CurrentUserId);
                await InsertMessage(groupName, message, CurrentUserId, (MessageType)messageType);
            });
        }


        [ValidateInput(true)]
        public Task SendToUser(string toUser, string message, int messageType, string groupName)
        {
            return Task.Run(async () =>
            {
                //await Clients.All.messageReceived(currentUserName, message, messageType, currentUserID, groupID);
                // Broad cast message
                foreach (var item in toUser.Split('_'))
                {
                    await Clients.User(item).messageReceived(message, messageType, groupName, CurrentUserId == item);
                }

                //await Clients.Caller.messageReceived(message, messageType, groupName, true);
                //await Clients.Caller.____callack((int)MessageCallbackFor.ReceivedToServer, messageID);
                await InsertMessage(groupName, message, CurrentUserId, (MessageType)messageType);
            });
        }

        public Task sendReadReceipt(long groupID)
        {
            return Task.Run(async () =>
            {
                await UpdateReadStatusForReceiver(groupID);
            });
        }

        private async Task UpdateReadStatusForReceiver(long groupID)
        {
            using (var repository = new ChatService())
            {
                //await repository.UpdateReadStatus(groupID, (int)MessageReadingStatus.ReadByReceiver);
            }
        }

        public Task SendTypingStatus(string userName, bool isTyping)
        {
            return Task.Run(async () =>
            {
                // Broad cast message
                await Clients.User(userName).setTypingStatus(isTyping);
            });
        }

        private async Task InsertMessage(string groupName, string message, string userId, MessageType messageType)
        {
            using (var service = new ChatService())
            {
                await service.InsertMessageToGroup(groupName, message, userId, messageType);
            }
        }

        #endregion

        string _currentUserId;
        public string CurrentUserId
        {
            get
            {
                if (!string.IsNullOrEmpty(_currentUserId))
                {
                    return _currentUserId;
                }

                if (Context.User.Identity is ClaimsIdentity claimUsers)
                {
                    string nameIdentifier = ClaimTypes.NameIdentifier;
                    var userId = claimUsers.Claims.FirstOrDefault(c => c.Type == nameIdentifier);
                    if (userId != null)
                    {
                        _currentUserId = userId.Value;
                    }
                }

                return _currentUserId;
            }
        }

    }

    public enum MessageType
    { }

    public enum MessageCallbackFor
    {
        ReceivedToServer
    }

    public enum MessageReadingStatus
    {
        Pending,
        ReadByReceiver
    }

    public class MyConnectionFactory : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            if (request.User.Identity is ClaimsIdentity claimUsers)
            {
                string nameIdentifier = ClaimTypes.NameIdentifier;
                var userId = claimUsers.Claims.FirstOrDefault(c => c.Type == nameIdentifier);
                if (userId != null)
                {
                    return userId.Value;
                }
            }

            return request.User.Identity.Name;
        }
    }
}
