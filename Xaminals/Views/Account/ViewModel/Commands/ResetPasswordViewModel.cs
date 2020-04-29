using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xaminals.Infra.Results;
using Xaminals.Services.HttpServices;
using Xaminals.Validations;

namespace Loffers.Views.Account.ViewModel
{
    public partial class ResetPasswordViewModel
    {
        public ICommand GenerateCodeCommand { get; set; }
        public ICommand SetPasswordCommand { get; set; }

        protected override void IntializeCommands()
        {
            base.IntializeCommands();
            GenerateCodeCommand = new Command(async () => await ExecuteGenerateCodeCommand());
            SetPasswordCommand = new Command(async () => await ExecuteSetPasswordCommand());
        }

        protected override void AddValidations()
        {
            base.AddValidations();
            _username.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Please enter your email address/username."
            });

            _securityCode.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Please enter security code sent over to your email/phone."
            });
        }

        async Task ExecuteGenerateCodeCommand()
        {
            if (ValidateUserName())
            {
                IsBusy = true;
                var service = new RestService();
                try
                {

                    var result = await service.GeneratePasswordRequest<HttpResult<object>>(new { EmailAddress = this.UserName.Value });
                    if (!result.IsError)
                    {
                        IsCodeSent = true;
                        var value = this.UserName.Value;
                        this.UserName.Value = null;
                        this.UserName.Value = value;
                    }
                    else
                        await RaiseError(result.Errors.First().Description);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    await RaiseError("An error occurred while generating new password request.");
                }
                finally
                {
                    service = null;
                    IsBusy = false;
                }
            }
        }

        private bool ValidateUserName()
        {
            return _username.Validate();
        }

        private bool ValidateSecurityCode()
        {
            return _securityCode.Validate();
        }

        async Task ExecuteSetPasswordCommand()
        {
            if (Validate())
            {
                IsBusy = true;
                var service = new RestService();
                try
                {
                    var result = await service.ResetPasswordRequest<HttpResult<object>>(new { EmailAddress = this.UserName.Value, ConfirmPassword = ConfirmPassword.Value, NewPassword = Password.Value, ResetCode = SecurityCode.Value });
                    if (!result.IsError)
                    {
                        await CloseAllPopups();
                        await ExecutePopupLoginCommand();
                        await RaiseSuccess("Password has been changed, please use new password for login.");
                    }
                    else
                        await RaiseError(result.Errors.First().Description);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    await RaiseError("An error occurred while generating new password request.");
                }
                finally
                {
                    IsBusy = false;
                    service = null;
                }
            }
        }

        protected override bool Validate()
        {
            var validUser = ValidateUserName();
            var validSecurity = ValidateSecurityCode();
            return validUser && validSecurity && base.Validate();
        }
    }
}
