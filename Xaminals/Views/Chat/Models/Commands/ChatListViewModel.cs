using Loffers.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xaminals.Infra.Results;
using Xaminals.Services.HttpServices;

namespace Loffers.Views.Chat.Models
{
    public partial class ChatListViewModel
    {
        public ICommand LoadItemsCommand { get; set; }
        public ICommand TappedItemChangedCommand { get; set; }

        protected override void IntializeCommands()
        {
            base.IntializeCommands();
            LoadItemsCommand = new Command(async (object sender) => await ExecuteLoadItemsCommand(sender));
            TappedItemChangedCommand = new Command(async (object sender) => await ExecuteSelectedItemChangedCommand(sender));
            LoadItemsCommand.Execute(null);
        }

        protected override void OnLoginStateChanged(bool isLoggedIn)
        {
            base.OnLoginStateChanged(isLoggedIn);
            if (isLoggedIn)
                LoadItemsCommand.Execute(null);
        }

        protected override void OnUserLogsOut()
        {
            base.OnUserLogsOut();
            this.Chats.Clear();
        }

        protected override void AddListeners()
        {
            base.AddListeners();
            this.ApplicationModel.OnMessageArrived += OnNewMessage;
        }

        private void OnNewMessage(MessageViewModel message)
        {
            if (Chats != null && Chats.Any(c => c.Name == message.GroupName))
            {
                var singleChat = Chats.FirstOrDefault(c => c.Name == message.GroupName);
                singleChat.IsSelf = message.IsSelf;
                singleChat.Message = message.Message;
                singleChat.Stamp = message.Stamp;
            }
            else
            {
                LoadItemsCommand.Execute(null);
            }
        }

        async Task ExecuteSelectedItemChangedCommand(object sender)
        {
            if (IsLoggedIn)
            {
                if (sender is ChatListItemModel SelectedChat)
                    await Shell.Current.GoToAsync($"chat?offerid=" + SelectedChat.OfferId + "&groupname=" + SelectedChat.Name, true);
            }
            else
            {
                await ExecutePopupLoginCommand();
            }
        }

        async Task ExecuteLoadItemsCommand(object sender)
        {
            if (IsLoggedIn)
            {
                HasItems = IsBusy = true;

                try
                {
                    var result = await new RestService().LoadChats<HttpResult<List<ChatListItemModel>>>();
                    if (!result.IsError)
                    {
                        foreach (var item in result.Result.OrderByDescending(c => c.Stamp))
                        {
                            if (!Chats.Any(c => c.Name == item.Name))
                                this.Chats.Add(item);
                            else
                            {
                                var existingChat = Chats.FirstOrDefault(c => c.Name == item.Name);
                                existingChat.IsSelf = item.IsSelf;
                                existingChat.Message = item.Message;
                                existingChat.Name = item.Name;
                                existingChat.OfferHeading = item.OfferHeading;
                                existingChat.Sender = item.Sender;
                                existingChat.Stamp = item.Stamp;
                                existingChat.UserId = item.UserId;
                                existingChat.Status = item.Status;
                            }
                        }
                    }
                    else
                    {
                        await RaiseError(result.Errors.First().Description);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    await RaiseError("An error occurred while loading chats.");
                }
                finally
                {
                    IsBusy = false;
                    HasItems = Chats.Any();
                }
            }
        }
    }
}