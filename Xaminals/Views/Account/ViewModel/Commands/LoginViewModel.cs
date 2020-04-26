using exchaup.Models;
using Loffers.Views.Account;
using Rg.Plugins.Popup.Services;
using System;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;
using Xaminals.Data.Database;
using Xaminals.Infra.Results;
using Xaminals.Models;
using Xaminals.Services.HttpServices;

namespace Xaminals.Views.Account.ViewModel
{
    /// <summary>
    /// LoginViewModel
    /// </summary>
    public partial class LoginViewModel
    {
        public ICommand AuthenticateCommand { get; set; }
        public ICommand RegisterCommand { get; set; }
        public ICommand ForgotPasswordCommand { get; set; }

        protected override void IntializeCommands()
        {
            base.IntializeCommands();
            AuthenticateCommand = new Command(Authenticate);
            RegisterCommand = new Command(Register);
            ForgotPasswordCommand = new Command(async () => { await CloseAllPopups(); await PopupNavigation.Instance.PushAsync(new ForgotPopup(), true); });
            RecordAnalyticsEventCommand.Execute(AnalyticsModel.InstanceOf(AnalyticsModel.EventNames.PageViewEvent, AnalyticsModel.ParameterNames.PageName, "login"));
        }

        private async void Authenticate(object obj)
        {
            if (Validate())
            {
                

                var service = new RestService();
                IsBusy = true;
                try
                {
                    RecordAnalyticsEventCommand.Execute(AnalyticsModel.InstanceOf(AnalyticsModel.EventNames.Login, AnalyticsModel.ParameterNames.User, this.Username.Value));

                    var result = await service.Login(new System.Collections.Generic.Dictionary<string, string>() {
                    { "username", this.Username.Value },
                    { "password", this.Password.Value },
                    { "grant_type", "password" }});

                    if (result != null && result.token != null)
                    {
                        App.Context.SessionModel.Token.expiration = result.expiration;
                        App.Context.SessionModel.Token.token = result.token;
                        App.Context.SessionModel.Token.vendor = result.vendor;
                        App.Context.SessionModel.IsLoggedIn = true;

                        try
                        {
                            await App.Database.SaveTokenAsync(result);
                        }
                        catch (System.Exception ex)
                        {
                            // Nothing.
                        }



                        var userDetailModel = await service.UserInfo<HttpResult<SessionModel>>();
                        // await SaveProperty("token", result);
                        AreCredentialsInvalid = false;
                        await CloseAllPopups();
                        return;
                    }

                    AreCredentialsInvalid = true;
                }
                catch (Exception e) 
                {
                    Debug.WriteLine(e.Message);
                }
                finally
                {
                    service = null;
                    IsBusy = false;
                }
            }

            // navigationService.GoBack();
        }

        private async void Register(object obj)
        {
            await ExecutePopupRegisterCommand();
        }
    }
}
