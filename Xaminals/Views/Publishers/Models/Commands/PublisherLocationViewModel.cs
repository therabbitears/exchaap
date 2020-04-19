using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xaminals.Infra.Results;
using Xaminals.Services.HttpServices;
using Xaminals.Views.Categories.Models.DTO;

namespace Xaminals.Views.Publishers.Models
{
    public partial class PublisherLocationViewModel
    {
        // Define the cancellation token.
        CancellationTokenSource tokenSource;
        CancellationToken token;

        public ICommand LoadLocationCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand FetchLocation { get; set; }

        protected override void IntializeCommands()
        {
            base.IntializeCommands();
            SaveCommand = new Command(Save);
            FetchLocation = new Command(async (object sender) => await ExecuteFetchLocation(sender));
            LoadLocationCommand = new Command(async (object sender) => await ExecuteLoadLocationCommand(sender));
            FetchLocation.Execute(this);
        }

        protected override void AddListeners()
        {
            base.AddListeners();
            this.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Id" && !string.IsNullOrEmpty(Id))
            {
                this.tokenSource?.Cancel();
                LoadLocationCommand.Execute(this);
            }
        }

        async Task ExecuteLoadLocationCommand(object sender)
        {
            IsBusy = true;

            try
            {
                var result = await new RestService().PublisherLocation<HttpResult<PublisherLocationModel>>(Id);
                if (!result.IsError)
                {
                    this.Name.Value = result.Result.Name;
                    this.DisplayAddress.Value = result.Result.DisplayAddress;
                    this.Lat.Value = result.Result.Lat;
                    this.Long.Value = result.Result.Long;
                }
                else
                {
                    await RaiseError(result.Errors.First().Description);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await RaiseError("An error occurred while loading location.");
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task ExecuteFetchLocation(object sender)
        {
            try
            {
                tokenSource = new CancellationTokenSource();
                token = tokenSource.Token;
                var location = await GetCurrentLocation(false, token);
                if (location != null)
                {
                    _lat.Value = location.Latitude;
                    _long.Value = location.Longitude;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception  
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception  
            }
            catch (Exception ex)
            {
                // Unable to get location  
            }
        }

        private async void Save(object obj)
        {
            if (Validate())
            {
                try
                {
                    var service = new RestService();

                    var result = await service.AddPublisherLocation<HttpResult<object>>(new
                    {
                        Name = this.Name.Value,
                        DisplayAddress = this.DisplayAddress.Value,
                        Lat = this.Lat.Value,
                        Long = this.Long.Value
                    });

                    if (!result.IsError)
                        await RaiseSuccess("The location has been saved successfully.");
                    else
                        await RaiseError(result.Errors.First().Description);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    await RaiseError("An error occurred while saving this location.");
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }
    }
}
