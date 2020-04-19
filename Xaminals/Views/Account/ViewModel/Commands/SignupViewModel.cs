using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xaminals.Infra.Results;
using Xaminals.Services.HttpServices;

namespace Xaminals.Views.Account.ViewModel
{
    public partial class SignupViewModel
    {
        public ICommand AuthenticateCommand { get; set; }

        protected override void IntializeCommands()
        {
            base.IntializeCommands();
            AuthenticateCommand = new Command(CreateUser);
        }

        private async void CreateUser(object obj)
        {
            if (Validate())
            {
                var service = new RestService();
                try
                {
                    var result = await service.CreateUser<HttpResult<object>>(new { Name = this.Name.Value, Email = this.Email.Value, Password = this.Password.Value, ConfirmPassword = this.ConfirmPassword.Value });
                    if (!result.IsError)
                    {
                        await RaiseSuccess("User has been created successfully.");
                        await CloseAllPopups();
                        await ExecutePopupLoginCommand();
                    }
                    else
                        await RaiseError(result.Errors.First().Description);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    await RaiseError("An error occurred while signup.");
                }
                finally 
                {
                    IsBusy = false;
                    service = null;
                }
            }
        }
    }
}
