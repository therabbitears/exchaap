using loffers.api.Hubs;
using loffers.api.Models.Generator;
using Loffers.Server.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace loffers.api.Services
{
    public class ChatService : BaseService
    {
        internal async Task<object> Start(string offerId, string groupName, string userId)
        {
            ChatGroups existingGroup = null;
            OfferLocations offerLocations = null;

            if (string.IsNullOrEmpty(groupName))
            {
                offerLocations = await context.OfferLocations.Include("Offers").FirstOrDefaultAsync(c => c.Offers.Id == offerId);
                groupName = string.Format("{0}_{1}_{2}", offerLocations.PublisherLocationID, offerLocations.Offers.Id, userId);
            }

            existingGroup = await context.ChatGroups.FirstOrDefaultAsync(c => c.Name == groupName);
            if (existingGroup == null)
            {
                existingGroup = new ChatGroups()
                {
                    CreatedBy = userId,
                    CreatedOn = DateTime.Now,
                    Name = groupName,
                    OfferID = offerLocations.Offers.OfferID,
                    OfferLocationID = offerLocations.OfferLocationID,
                    Status = 1
                };

                context.ChatGroups.Add(existingGroup);

                var user1 = new ChatGroupUsers()
                {
                    Active = true,
                    UserId = userId,
                    ChatGroups = existingGroup,
                };
                context.ChatGroupUsers.Add(user1);


                var user2 = new ChatGroupUsers()
                {
                    Active = true,
                    UserId = offerLocations.Offers.CreatedBy,
                    ChatGroups = existingGroup
                };
                context.ChatGroupUsers.Add(user2);

                await context.SaveChangesAsync();

                return new
                {
                    Group = new
                    {
                        existingGroup.Name,
                        Owner = userId
                    },
                    Users = new List<object>() { new
                    {
                        user1.UserId,
                        IsSelf = user1.UserId == userId,
                        DisplayName = context.UserProfileSnapshots.FirstOrDefault(c => c.UserId == userId).FullName },
                        new
                        {
                            user2.UserId,
                            IsSelf = user2.UserId == userId,
                            DisplayName = context.UserProfileSnapshots.FirstOrDefault(c => c.UserId == user2.UserId).FullName
                        }
                    }
                };
            }

            return new
            {
                Group = new
                {
                    existingGroup.Name,
                    Owner = existingGroup.CreatedBy
                },
                Users = existingGroup.ChatGroupUsers.Select(c => new
                {
                    c.UserId,
                    IsSelf = c.UserId == userId,
                    DisplayName = context.UserProfileSnapshots.FirstOrDefault(d => d.UserId == d.UserId).FullName
                }).ToList()
            };
        }

        public async Task<object> MarkAsRead(string groupName, string userId)
        {
            var message = await context.ChatGroupMessages.Include("ChatGroupUsers").Include("ChatGroupUsers.ChatGroups").OrderByDescending(c => c.CreatedOn).FirstOrDefaultAsync(c => c.ChatGroupUsers.ChatGroups.Name == groupName);
            if (message != null && message.ChatGroupUsers.UserId != userId)
            {
                message.Status = 1;
            }

            return context.SaveChangesAsync();
        }

        public async Task<object> LoadChat(string name, string userId)
        {
            var isParticipant = await context.ChatGroupUsers.Include("ChatGroups").AnyAsync(c => c.UserId == userId && c.ChatGroups.Name == name);
            if (isParticipant)
            {
                return await context.ChatGroupMessages.Include("ChatGroupUsers").Include("ChatGroupUsers.ChatGroups").Where(c => c.ChatGroupUsers.ChatGroups.Name == name)
                    .Select(c => new
                    {
                        c.Message,
                        c.ChatGroupUsers.UserId,
                        Stamp = c.CreatedOn,
                        IsSelf = c.ChatGroupUsers.UserId == userId,
                        DisplayName = context.UserProfileSnapshots.FirstOrDefault(d => d.UserId == c.ChatGroupUsers.UserId).FullName,
                        c.Status
                    }).OrderBy(c => c.Stamp)
                    .ToListAsync();
            }
            else
            {
                return new List<object>();
            }
        }


        public async Task<object> Chats(string userID)
        {
            var list = context.ChatGroupUsers.Include("ChatGroups").Include("ChatGroups.Offers").Include("ChatGroups.Offers.Categories").Where(c => c.UserId == userID).Join
                    (context.ChatGroupMessages.Include("ChatGroupUsers").GroupBy(x => x.ChatGroupUsers.GroupID, (key, g) => g.OrderByDescending(e => e.CreatedOn).FirstOrDefault()), users => users.GroupID, chatGroupMessage => chatGroupMessage.ChatGroupUsers.GroupID, (users, chatGroupMessage) => new
                    {
                        OfferId = users.ChatGroups.Offers.Id,
                        Stamp = chatGroupMessage.CreatedOn,
                        Sender = chatGroupMessage.ChatGroupUsers.UserId,
                        users.UserId,
                        users.ChatGroups.Name,
                        chatGroupMessage.Message,
                        users.ChatGroups.Offers.OfferHeadline,
                        users.ChatGroups.Offers.Image,
                        users.ChatGroups.Offers.OriginalImage,
                        IsSelf = chatGroupMessage.ChatGroupUsers.UserId == userID,
                        DisplayName = context.UserProfileSnapshots.FirstOrDefault(d => d.UserId == chatGroupMessage.ChatGroupUsers.UserId).FullName,
                        chatGroupMessage.Status,
                        Category = new { users.ChatGroups.Offers.Categories.Id, users.ChatGroups.Offers.Categories.Name, users.ChatGroups.Offers.Categories.Image },
                    }).Where(c => c.UserId == userID);

            return await list.OrderByDescending(c => c.Stamp).ToListAsync();
        }

        public async Task InsertMessageToGroup(string groupName, string message, string userId, ChatHub.MessageType messageType)
        {
            var group = await context.ChatGroups.FirstOrDefaultAsync(c => c.Name == groupName);
            var chatGroupCurrentUser = await context.ChatGroupUsers.FirstOrDefaultAsync(c => c.UserId == userId && c.GroupID == group.GroupID);
            var chatMessage = new ChatGroupMessages()
            {
                Message = message,
                Status = 0,
                CreatedOn = DateTime.UtcNow,
                ChatGroupUsers = chatGroupCurrentUser
            };

            context.ChatGroupMessages.Add(chatMessage);
            context.Entry<ChatGroupUsers>(chatGroupCurrentUser).State = EntityState.Unchanged;
            await context.SaveChangesAsync();
        }
    }
}
