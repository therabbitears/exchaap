using Loffers.Models;
using Loffers.Views.Account;
using Loffers.Views.Settings;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xaminals.Infra.Results;
using Xaminals.Services.HttpServices;
using Xaminals.Views.Settings;

namespace Xaminals.ViewModels.Settings
{
    public partial class SettingPageViewModel
    {
        public ICommand ClickPersonalInfoCommand { get; set; }
        public ICommand ClickCategoriesInfoCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public ICommand LogoutConfirmCommand { get; set; }
        public ICommand UpdatePasswordCommand { get; set; }
        public ICommand ClickSwitchSpaceCommand { get; set; }
        public ICommand ClickAboutCommand { get; set; }
        
        public ICommand LoadSettingCommands { get; set; }
        public ICommand SaveSettingCommands { get; set; }

        protected override void IntializeCommands()
        {
            base.IntializeCommands();
            ClickPersonalInfoCommand = new Command(async () => await ExecuteClickPersonalInfoCommand(null));
            LogoutCommand = new Command(async () => await ExecuteLogoutCommand(null));
            LogoutConfirmCommand = new Command(async () => await Logout(null));
            UpdatePasswordCommand = new Command(async () => await ExecuteUpdatePasswordCommand(null));
            ClickCategoriesInfoCommand = new Command(async () => await ExecuteCategoriesInfoCommand(null));
            ClickSwitchSpaceCommand = new Command(async () => await SwitchSpace());
            LoadSettingCommands = new Command(async () => await ExecuteLoadSettingCommands(null));
            SaveSettingCommands = new Command(async () => await SaveSettings(new { this.MaxRange }));
            ClickAboutCommand = new Command(async () => await Application.Current.MainPage.Navigation.PushAsync(new About(), true));
            MessagingCenter.Subscribe<App>(this, "Hi", (sender) =>
            {
                // I have received an even.
            });

            LoadSettingCommands.Execute(null);
        }

        protected override void OnLoginStateChanged(bool isLoggedIn)
        {
            base.OnLoginStateChanged(isLoggedIn);
            if (isLoggedIn)
                LoadSettingCommands.Execute(null);
        }

        protected override void AddListeners()
        {
            base.AddListeners();
            this.PropertyChanged += OnPropertyChanged;
        }

        async void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "UnitOfMeasurement")
            {
                await SaveSettings(new { this.UnitOfMeasurement });
            }
        }

        async Task<bool> SaveSettings(object obj)
        {
            IsBusy = true;

            try
            {
                var result = await new RestService().UpdateConfiguration<HttpResult<bool>>(obj);
                if (!result.IsError)
                {
                    return true;
                }
                else
                {
                    await RaiseError(result.Errors.First().Description);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await RaiseError("An error occurred while loading categories.");
            }
            finally
            {
                IsBusy = false;
            }

            return false;
        }

        async Task SwitchSpace()
        {
            var result = await SaveSettings(new { IsPublisher = !this.Context.SessionModel.IsPublisher });
            if (result)
            {
                this.Context.SessionModel.IsPublisher = !this.Context.SessionModel.IsPublisher;
            }
        }

        async Task ExecuteLoadSettingCommands(object obj)
        {
            if (IsLoggedIn)
            {
                IsBusy = true;
                if (this.Categories != null)
                    this.Categories.Clear();
                else
                    this.Categories = new ObservableCollection<object>();

                try
                {
                    var result = await new RestService().LoadConfiguration<HttpResult<SettingsModel>>();
                    if (!result.IsError)
                    {
                        this.MaxRange = result.Result.Configuration.MaxRange;
                        this.Context.SessionModel.IsPublisher = result.Result.Configuration.IsPublisher;
                        this.UnitOfMeasurement = result.Result.Configuration.UnitOfMeasurement;
                        this.CategoriesCount = result.Result.Categories.Count(c => c.Selected);
                        this.Name = result.Result.Name;
                        this.Email = result.Result.Email;
                        this.PhoneNumber = result.Result.PhoneNumber;
                        result.Result.Categories.ForEach(c => this.Categories.Add(c));
                    }
                    else
                    {
                        await RaiseError(result.Errors.First().Description);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    await RaiseError("An error occurred while loading categories.");
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        async Task ExecuteCategoriesInfoCommand(object data)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new Categories(), false);
        }

        async Task ExecuteUpdatePasswordCommand(object data)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new UpdatePassword(), true);
        }

        async Task ExecuteLogoutCommand(object data)
        {
            await CloseAllPopups();
            await PopupNavigation.Instance.PushAsync(new ConfirmPopup(), true);
            //Confirm(async () => await Logout(), null);
            //return true;
            //var result = await Application.Current.MainPage.DisplayAlert("AA", "BB", "Ok");
            //MessagingCenter.Send<BaseViewModel, string>(new BaseViewModel() { IsBusy = false }, "Hi", "John");
        }

        async Task Logout(object data)
        {
            Context.SessionModel.Token.expiration = DateTime.MinValue;
            Context.SessionModel.Token.token = null;
            Context.SessionModel.Token.vendor = null;
            Context.SessionModel.IsLoggedIn = false;
            await App.Database.DeleteTokenAsync();
            await CloseAllPopups();
        }

        async Task ExecuteClickPersonalInfoCommand(object obj)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new PersonalSettings(), true);
        }
    }
}
