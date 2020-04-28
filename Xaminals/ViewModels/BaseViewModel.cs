using exchaup;
using exchaup.Models;
using exchaup.Views.Home.Model;
using Loffers.GlobalViewModel;
using Loffers.Services.LocationServices;
using Plugin.Geolocator;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xaminals.Data.Database;
using Xaminals.Infra.Context;
using Xaminals.Models;
using Xaminals.Services.HttpServices;
using Xaminals.Views.Account;
using Xaminals.Views.Common.Alerts;

namespace Xaminals.ViewModels
{
    public class BaseViewModel : NotificableObject
    {
        public ICommand CancelCommand { get; set; }
        public ICommand OpenUrlCommand { get; set; }
        public ICommand CancelPopupCommand { get; set; }
        public ICommand PopupLoginCommand { get; set; }
        public ICommand PopupRegisterCommand { get; set; }
        public ICommand OpenLocationDialogCommand { get; set; }
        public ICommand RecordAnalyticsEventCommand { get; set; }
        public ICommand GotoAppCommand { get; set; }
        
        public BaseViewModel()
        {
            IntializeMembers();
            IntializeCommands();
            AddValidations();
            AddListeners();
        }

        ~BaseViewModel()
        {
            RemoveListeners();
        }

        protected virtual void AddListeners()
        {
            this.Context.SessionModel.PropertyChanged += OnSessionModelPropertyChanged;
        }

        protected virtual void RemoveListeners()
        {
            this.Context.SessionModel.PropertyChanged -= OnSessionModelPropertyChanged;
        }

        private void OnSessionModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsLoggedIn")
                OnLoginStateChanged(this.Context.SessionModel.IsLoggedIn);
        }

        protected virtual void IntializeCommands()
        {
            CancelCommand = new Command(async () => await ExecuteCancelCommand());
            CancelPopupCommand = new Command(async () => await ExecuteCancelPopupCommand());
            PopupLoginCommand = new Command(async () => await ExecutePopupLoginCommand());
            PopupRegisterCommand = new Command(async () => await ExecutePopupRegisterCommand());
            OpenUrlCommand = new Command(async (object parameter) => await Launcher.TryOpenAsync(new Uri(parameter?.ToString())));
            GotoAppCommand = new Command(ReloadApp);
            RecordAnalyticsEventCommand = new Command(ExecuteRecordAnalyticsEventCommand);
            OpenLocationDialogCommand = new Command(OpenLocationSettings);
        }

        private void ReloadApp(object obj)
        {
            MessagingCenter.Send<StartupScreenCardsViewModel>(new StartupScreenCardsViewModel(), "GotoApp");
        }

        protected async Task ExecutePopupRegisterCommand()
        {
            await CloseAllPopups();
            await PopupNavigation.Instance.PushAsync(new RegisterPopup(), true);
        }

        protected virtual void AddValidations()
        {
            // Do nothing here.
        }

        protected virtual void IntializeMembers()
        {
            //
        }

        protected virtual void OnLoginStateChanged(bool isLoggedIn)
        {
            if (!isLoggedIn)
                OnUserLogsOut();
        }

        protected virtual void OnUserLogsOut()
        {
            // Do nothing.
        }

        protected async Task ExecuteCancelCommand()
        {
            await Shell.Current.Navigation.PopAsync(true);
        }

        protected async Task ExecuteCancelPopupCommand()
        {
            await PopupNavigation.Instance.PopAsync(true);
        }

        protected async Task ExecutePopupLoginCommand()
        {
            await CloseAllPopups();
            await PopupNavigation.Instance.PushAsync(new LoginPopup(), true);
        }

        protected async Task CloseAllPopups()
        {
            if (PopupNavigation.Instance.PopupStack?.Count > 0)
                await PopupNavigation.Instance.PopAllAsync(true);
        }

        protected virtual async Task<object> SaveProperty(string key, object value)
        {
            Application.Current.Properties[key] = value;
            await Application.Current.SavePropertiesAsync();

            return value;
        }

        protected virtual void Invalidate()
        {
            Refresh();
        }

        protected virtual void Refresh()
        {
            // 
        }


        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        private bool _FetchingLocation;
        public bool FetchingLocation
        {
            get { return _FetchingLocation; }
            set { SetProperty(ref _FetchingLocation, value); }
        }


        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        private int _CurrentOrientation;
        public int CurrentOrientation
        {
            get { return _CurrentOrientation; }
            set { SetProperty(ref _CurrentOrientation, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void Confirm(Action success, Action noResult, string title, string message, string confirmButtonText, string cancelButtonText)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await Application.Current.MainPage.DisplayAlert("Alert!", "Do you really want to exit?", "Yes", "No");
                if (result) success?.Invoke(); // or anything else
                else noResult?.Invoke();
            });
        }

        public SingletonLoffersContext Context
        {
            get { return SingletonLoffersContext.Context; }
        }

        public LoffersDb Database
        {
            get
            {
                return App.Database;
            }
        }

        public RestService Service
        {
            get
            {
                return App.Service;
            }
        }

        public bool IsLoggedIn
        {
            get
            {
                return Context?.SessionModel?.IsLoggedIn == true;
            }
        }

        public async Task RaiseError(string description)
        {
            await PopupNavigation.Instance.PushAsync(new Error(description), true);
        }

        public async Task RaiseSuccess(string description)
        {
            await PopupNavigation.Instance.PushAsync(new Success(description), true);
        }

        protected virtual async Task<Location> GetCurrentLocation(bool tryCached = false, CancellationToken token = default)
        {
            if (!Context.SettingsModel.SelectedLocation.IsCurrent)
                return new Location(Context.SettingsModel.SelectedLocation.Lat, Context.SettingsModel.SelectedLocation.Long);

            return await LoadLocation(tryCached, token);
        }

        protected async Task<Location> LoadLocation(bool tryCached, CancellationToken token = default)
        {
            var service = DependencyService.Get<ILocationService>();
            if (service != null && !service.IsLocationAvailable())
            {
                LocationAvailable = false;
                throw new Exception("Location services are not enabled.");
            }

            LocationAvailable = true;
            FetchingLocation = true;
            var request = new GeolocationRequest(GeolocationAccuracy.Default, TimeSpan.FromSeconds(10));
            Location location = null;

            try
            {
                if (tryCached)
                {
                    try
                    {
                        location = await Geolocation.GetLastKnownLocationAsync();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }

                location = location ?? await Geolocation.GetLocationAsync(request, token);
                var reverse = await Geocoding.GetPlacemarksAsync(location);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            FetchingLocation = false;
            return location;
        }

        protected virtual void OpenLocationSettings()
        {
            if (Device.RuntimePlatform == global::Xamarin.Forms.Device.Android)
            {
                //DependencyService.Get<ISettingsService>().OpenSettings();
                DependencyService.Get<ILocationService>()?.OpenSettings();
            }
            else
            {
                DependencyService.Get<ILocationService>()?.OpenSettings();
            }
        }

        public ApplicationViewModel ApplicationModel
        {
            get
            {
                return (Application.Current as App).ApplicationModel;
            }
        }

        bool _locationAvailable = true;
        public bool LocationAvailable { get { return _locationAvailable; } set { SetProperty(ref _locationAvailable, value); } }

        private IFirebaseAnalytics _analyticsService;
        public IFirebaseAnalytics AnalyticsService
        {
            get
            {
                if (_analyticsService == null)
                {
                    _analyticsService = DependencyService.Get<IFirebaseAnalytics>();
                }

                return _analyticsService;
            }
        }

        async void ExecuteRecordAnalyticsEventCommand(object obj)
        {
            try
            {
                var analiticsObject = obj as AnalyticsModel;
                AnalyticsService.SendEvent(analiticsObject.EventName, analiticsObject.Parameters);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //await RaiseError(ex.Message);
            }
        }

        //protected void LogPageViews(string pageName)
        //{
        //    try
        //    {
        //        AnalyticsService.SendEvent("pageview", "pageName", pageName);
        //    }
        //    catch (Exception ex)
        //    {
        //        RaiseError(ex.Message);
        //    }
        //}
    }
}