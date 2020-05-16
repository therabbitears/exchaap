using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xaminals.Infra.Results;
using Xaminals.Services.HttpServices;

namespace Xaminals.Views.Settings.ViewModels
{
    public partial class ChangePasswordViewModel
    {
        public ICommand UpdatePasswordCommand { get; set; }

        protected override void IntializeCommands()
        {
            base.IntializeCommands();
            UpdatePasswordCommand = new Command(UpdatePassword);
        }

        private async void UpdatePassword(object obj)
        {
            if (Validate())
            {
                try
                {
                    var service = new RestService();
                    var result = await service.UpdatePassword<HttpResult<object>>(new { CurrentPassword = this.CurrentPassword.Value, NewPassword = this.Password.Value });
                    if (!result.IsError)
                    {
                        await ExecuteCancelCommand();
                        await RaiseSuccess("The passwords has been updated successfully.");
                    }
                    else
                    {
                        LastError = result.Errors.First().Description;
                        AreCredentialsInvalid = true;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    await RaiseError("An error occurred while updating passwords.");
                }
            }
        }
    }
}
