using Loffers.Views.Chat.Models;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xaminals.Infra.Results;
using Xaminals.Services.HttpServices;

namespace Loffers.GlobalViewModel
{
    public partial class ApplicationViewModel
    {
        public delegate void MessageArrived(MessageViewModel message);
        public event MessageArrived OnMessageArrived;

        public ICommand ValidateTokenCommand { get; set; }
        public ICommand StartConnectionCommand { get; set; }
        public ICommand LoadStaticCommand { get; set; }

        protected override void IntializeCommands()
        {
            base.IntializeCommands();
            ValidateTokenCommand = new Command(async (object value) => await ExecuteValidateTokenCommand(value));
            StartConnectionCommand = new Command(async (object value) => await ExecuteStartConnectionCommand(value));
            LoadStaticCommand = new Command(async (object value) => await ExecuteLoadStaticCommand(value));
            ValidateTokenCommand.Execute(null);
            LoadStaticCommand.Execute(null);
        }

        async Task ExecuteLoadStaticCommand(object value)
        {
            try
            {
                var categories = await Context.Database.GetCategoriesAsync();
                if (categories?.Any() == true)
                {
                    foreach (var category in categories)
                    {
                        this.Context.SearchModel.Categories.Add(category);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }            
        }

        protected override void OnLoginStateChanged(bool isLoggedIn)
        {
            base.OnLoginStateChanged(isLoggedIn);
            StartConnectionCommand.Execute(null);
        }

        async Task ExecuteStartConnectionCommand(object value)
        {
            if (IsLoggedIn)
            {
                try
                {
                    hubConnection = new HubConnection(UrlConstants.baseUrl + "signalr");
                    AddListeners(hubConnection);

                    chatHubProxy = hubConnection.CreateHubProxy("ChatHub");
                    hubConnection.Headers.Add("Authorization", "bearer " + Context.SessionModel.Token.token);
                    chatHubProxy.On<string, int, string, bool>("messageReceived", (string message, int messageType, string groupName, bool self) =>
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            var newMessage = new MessageViewModel()
                            {
                                Message = message,
                                IsSelf = self,
                                Stamp = DateTime.UtcNow,
                                GroupName = groupName
                            };

                            OnMessageArrived?.Invoke(newMessage);
                        });
                    });

                    //chatHubProxy.On<string, int, string, string>("messageReceived", (string message, int messageType, string groupName, string sender) =>
                    //{
                    //    MainThread.BeginInvokeOnMainThread(() =>
                    //    {
                    //        var newMessage = new MessageViewModel()
                    //        {
                    //            Message = message,
                    //            IsSelf = Context.SessionModel.Token.token == sender,
                    //            Stamp = DateTime.Now,
                    //            GroupName = groupName
                    //        };

                    //        OnMessageArrived?.Invoke(newMessage);
                    //    });
                    //});

                    chatHubProxy.On<DateTime>("onConnected", (DateTime time) =>
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                        //  RaiseSuccess(time.ToString());
                    });
                    });

                    await StartConnection();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            else
            {
                try
                {
                    if (hubConnection != null)
                    {
                        //if (hubConnection.State == ConnectionState.Connected)
                        //    hubConnection.Stop();

                        hubConnection.Dispose();
                    }

                    if (chatHubProxy != null)
                    {
                        chatHubProxy = null;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                finally { hubConnection = null; chatHubProxy = null; }
            }
        }

        private void AddListeners(HubConnection hubConnection)
        {
            hubConnection.Closed += OnConnectionClosed;
            hubConnection.StateChanged += OnStateChanged;
            hubConnection.Reconnected += OnReconnected;
            hubConnection.Reconnecting += OnReconnecting;
        }

        async Task ExecuteValidateTokenCommand(object value)
        {
            try
            {
                var tokens = Database.AllTokens().Result;
                if (tokens != null)
                {
                    var token = tokens.FirstOrDefault();
                    if (token != null)
                    {
                        Service.Token = token.token;
                        var validToken = await Service.ValidateToken<HttpResult<bool>>();
                        if (validToken != null)
                        {
                            if (!validToken.Result)
                                await Database.DeleteTokenAsync();
                            else
                            {
                                Context.SessionModel.Token.expiration = token.expiration;
                                Context.SessionModel.Token.token = token.token;
                                Context.SessionModel.Token.vendor = token.vendor;
                                Context.SessionModel.IsLoggedIn = true;
                            }
                        }

                        Service.Token = null;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine("OnStart an error occurred while retrieving or refreshing the token.");
            }
        }

        private void OnReconnecting()
        {

        }

        private void OnReconnected()
        {
        }

        private void OnStateChanged(Microsoft.AspNet.SignalR.Client.StateChange obj)
        {
            if (obj.NewState == Microsoft.AspNet.SignalR.Client.ConnectionState.Disconnected && obj.OldState == Microsoft.AspNet.SignalR.Client.ConnectionState.Connected)
            {
                StartConnectionCommand.Execute(null);
            }
        }

        async Task StartConnection()
        {
            try
            {
                await hubConnection.Start();
            }
            catch (System.Exception ex)
            {

            }
        }

        void OnConnectionClosed()
        {
            StartConnectionCommand.Execute(null);
        }
    }
}
