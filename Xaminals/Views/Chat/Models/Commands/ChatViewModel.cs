using Loffers.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xaminals;
using Xaminals.Infra.Mappers;
using Xaminals.Infra.Results;
using Xaminals.Services.HttpServices;
using Xaminals.Views.Offers.ViewModels;

namespace Loffers.Views.Chat.Models
{
    public partial class ChatViewModel
    {
        public ICommand SendMessageCommand { get; set; }
        public ICommand LoadOfferCommand { get; set; }
        public ICommand StartChatCommand { get; set; }
        public ICommand LoadChatCommand { get; set; }
        public ICommand GoToOfferCommand { get; set; }


        protected override void IntializeCommands()
        {
            base.IntializeCommands();
            SendMessageCommand = new Command(ExecuteSendCommand);
            LoadOfferCommand = new Command(async (object obj) => await ExecuteLoadOfferCommand(obj));
            StartChatCommand = new Command(async (object obj) => await ExecuteStartChatCommand(obj));
            LoadChatCommand = new Command(async (object obj) => await ExecuteLoadChatCommand(obj));
            GoToOfferCommand = new Command(async (object obj) => await ExecuteGoToOfferCommand(obj));
        }      

        protected override void AddListeners()
        {
            base.AddListeners();
            this.PropertyChanged += OnPropertyChanged;
            ApplicationModel.OnMessageArrived += OnMessageArrived;
            //GroupName = "72d90861-4203-430a-8530-e7989953b4b6-fcad56d0-a46c-42e3-8d64-dc4cb3826267";
            ////LocationId = "53074ac1-95dc-409a-9f72-ba6d4b92ea94";
            //Id = "72d90861-4203-430a-8530-e7989953b4b6";
        }

        private void OnMessageArrived(MessageViewModel newMessage)
        {
            if (newMessage.GroupName == this.Group.Name)
            {
                Messages.Add(newMessage);
            }
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Id")
            {
                LoadOfferCommand.Execute(null);
                StartChatCommand.Execute(null);
            }
        }

        async void ExecuteSendCommand(object value)
        {
            if (!string.IsNullOrEmpty(Message))
            {
                if (ApplicationModel.hubConnection.State == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
                {
                    await ApplicationModel.chatHubProxy.Invoke("SendToUser", string.Join("_", Users.Select(c => c.UserId).ToArray()), Message, 1, this.Group.Name);
                    Message = string.Empty;
                }
            }
        }

        async Task ExecuteGoToOfferCommand(object obj)
        {
            ShellNavigationState state = Shell.Current.CurrentState;
            await Shell.Current.GoToAsync($"offerdetails?offerid=" + this.Offer.Id + "&locationid=" + this.Offer.LocationId, true);
        }

        async Task ExecuteStartChatCommand(object obj)
        {
            IsBusy = true;

            try
            {
                var result = await new RestService().StartChat<HttpResult<ChatModel>>(Id, LocationId, GroupName);
                if (!result.IsError)
                {
                    this.Users.Clear();
                    this.Group.Name = result.Result.Group.Name;
                    this.Group.Owner = result.Result.Group.Owner;
                    foreach (var item in result.Result.Users)
                    {
                        this.Users.Add(item);
                    }

                    await ExecuteLoadChatCommand(this.Offer);
                    await ExecuteMarkAdRead(this.Group.Name);
                    //await (Application.Current as App).ApplicationModel.chatHubProxy.Invoke("JoinToGroup", this.Group.Name);
                }
                else
                {
                    await RaiseError(result.Errors.First().Description);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await RaiseError("An error occurred while loading offer details.");
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task ExecuteMarkAdRead(string name)
        {
            IsBusy = true;

            try
            {
                var location = await GetCurrentLocation(true);
                var result = await new RestService().MarkMessageAsRead<HttpResult<object>>(new { GroupName = name });
                if (!result.IsError)
                {
                    // Do nothing
                }
                else
                {
                    await RaiseError("An error occurred while marking the chat as read.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await RaiseError("An error occurred while marking the chat as read.");
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task ExecuteLoadOfferCommand(object obj)
        {
            IsBusy = true;

            try
            {
                var location = await GetCurrentLocation(true);
                var result = await new RestService().OfferDetail<HttpResult<OfferListItemViewModel>>(Id, LocationId, location.Latitude, location.Longitude, "M");
                if (!result.IsError)
                {
                    this.Title = result.Result.Title;
                    Mapper.Map(result.Result, this.Offer);
                }
                else
                {
                    await RaiseError(result.Errors.First().Description);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await RaiseError("An error occurred while loading offer details.");
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task ExecuteLoadChatCommand(object obj)
        {
            IsBusy = true;

            try
            {
                var result = await new RestService().LoadChat<HttpResult<List<MessageViewModel>>>(this.Group.Name);
                if (!result.IsError)
                    result.Result.ForEach(c => this.Messages.Add(c));
                else
                    await RaiseError(result.Errors.First().Description);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await RaiseError("An error occurred while loading offer details.");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
