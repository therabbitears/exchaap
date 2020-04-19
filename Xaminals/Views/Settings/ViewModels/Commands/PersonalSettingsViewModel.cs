using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xaminals.Infra.Results;
using Xaminals.Models;
using Xaminals.Services.HttpServices;

namespace Xaminals.Views.Settings.ViewModels
{
    public partial class PersonalSettingsViewModel
    {
        public ICommand LoadDetailsCommand { get; set; }
        public ICommand SaveDetailsCommand { get; set; }

        protected override void IntializeCommands()
        {
            base.IntializeCommands();
            LoadDetailsCommand = new Command(async () => await ExecuteLoadDetails());
            SaveDetailsCommand = new Command(async () => await Update(null));
            LoadDetailsCommand.Execute(null);
        }

        public async Task ExecuteLoadDetails()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var result = await new RestService().UserDetail<HttpResult<UserDetailModel>>();
                if (!result.IsError)
                {
                    _email.Value = result.Result.Email;
                    _name.Value = result.Result.Name;
                    _phoneNumber.Value = result.Result.PhoneNumber;
                    //await ExecuteCancelCommand();
                    //await RaiseSuccess("The information has been updated successfully.");
                }
                else
                    await RaiseError(result.Errors.First().Description);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await RaiseError("An error occurred while loading information.");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task Update(object obj)
        {
            if (ValidateUpdate())
            {
                try
                {
                    var service = new RestService();
                    var result = await service.UpdateUser<HttpResult<object>>(new { Name = this.Name.Value, Email = this.Email.Value, Phone = this.PhoneNumber.Value });
                    if (!result.IsError)
                    {
                        await service.UpdateUserSnapshot<HttpResult<object>>(new { Name = this.Name.Value, Email = this.Email.Value, Phone = this.PhoneNumber.Value });
                        App.Context.SettingsModel.Name = this.Name.Value;
                        App.Context.SettingsModel.Email = this.Email.Value;
                        App.Context.SettingsModel.PhoneNumber = this.PhoneNumber.Value;
                        await RaiseSuccess("User information has been updated successfully.");
                    }
                    else
                        await RaiseError(result.Errors.First().Description);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    await RaiseError("An error occurred while updating your information.");
                }
            }
        }
    }
}
