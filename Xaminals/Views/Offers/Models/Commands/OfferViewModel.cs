using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xaminals.Infra.Results;
using Xaminals.Models;
using Xaminals.Services.HttpServices;
using Xaminals.Views.Categories.Models.DTO;
using Xaminals.Views.Offers.ViewModels;

namespace Xaminals.Views.Offers.Models
{
    public partial class OfferViewModel
    {
        public ICommand SaveOfferCommand { get; set; }
        public ICommand SelectImageCommand { get; set; }
        public Command LoadOfferCommand { get; set; }
        public Command LoadCurrentLocationCommand { get; set; }


        public string Image { get; set; }

        protected override void IntializeCommands()
        {
            base.IntializeCommands();
            SaveOfferCommand = new Command(SaveOffer);
            SelectImageCommand = new Command(SelectImage);
            LoadCurrentLocationCommand = new Command(ExecuteLoadCurrentLocationCommand);
            if (!string.IsNullOrEmpty(Id))
            {
                LoadOfferCommand = new Command(async () => await ExecuteLoadOfferCommand());
                LoadOfferCommand.Execute(null);
            }

            LoadCurrentLocationCommand.Execute(null);
        }

        async void ExecuteLoadCurrentLocationCommand()
        {
            try
            {
                var location = await LoadLocation(false);
                this.Location.Lat = location.Latitude;
                this.Location.Long = location.Longitude;

                var placemarks = await Geocoding.GetPlacemarksAsync(location);

                var placemark = placemarks?.FirstOrDefault();
                if (placemark != null)
                {
                    var geocodeAddress =
                        $"AdminArea:       {placemark.AdminArea}\n" +
                        $"CountryCode:     {placemark.CountryCode}\n" +
                        $"CountryName:     {placemark.CountryName}\n" +
                        $"FeatureName:     {placemark.FeatureName}\n" +
                        $"Locality:        {placemark.Locality}\n" +
                        $"PostalCode:      {placemark.PostalCode}\n" +
                        $"SubAdminArea:    {placemark.SubAdminArea}\n" +
                        $"SubLocality:     {placemark.SubLocality}\n" +
                        $"SubThoroughfare: {placemark.SubThoroughfare}\n" +
                        $"Thoroughfare:    {placemark.Thoroughfare}\n";

                    await RaiseSuccess(geocodeAddress);
                }
            }
            catch (Exception ex)
            {
                await RaiseError(ex.Message);
                this.Location.Name = "N/A";
                this.Location.DisplayAddress = "N/A";
            }
        }

        protected override void AddListeners()
        {
            base.AddListeners();
            this.PropertyChanged += OnPropertryChanged;
        }

        private void OnPropertryChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e != null && e.PropertyName == "Id")
            {
                LoadOfferCommand = LoadOfferCommand ?? new Command(async () => await ExecuteLoadOfferCommand());
                LoadOfferCommand.Execute(null);
            }
        }

        private async void SelectImage(object obj)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                //await DisplayAlert("Error", "This is not support on your device.", "OK");
                return;
            }
            else
            {
                var mediaOption = new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                };

                _mediaFile = await CrossMedia.Current.PickPhotoAsync();
                if (_mediaFile == null) return;

                //SourceImage = ImageSource.FromStream(() => _mediaFile.GetStream());

                ImageStream = _mediaFile.GetStream();
                try
                {
                    SourceImage = ImageSource.FromStream(() => _mediaFile.GetStream());
                }
                catch (Exception ex)
                {
                }
            }
        }

        ImageModel imageModel = null;
        private async void SaveOffer(object obj)
        {
            if (Validate())
            {
                try
                {
                    var service = new RestService();

                    if (ImageStream != null && imageModel == null)
                    {
                        imageModel = await service.UploadImageAsynch(ImageStream, this.Heading.Value.Replace(" ", string.Empty) + ".png");
                        imageModel.uploaded = true;
                    }
                    //if (string.IsNullOrEmpty(this.Category.Name))
                    //{
                    //    this.Category.Id = "46f188db-a2a8-44d5-b171-c0adbbb3d7d9";
                    //    this.Category.Name = "Mobile";
                    //}

                    //if (!this.IsGiveUp && !this.Categories.Any())
                    //{
                    //    this.Categories = new ObservableCollection<CategoryModel>()
                    //{
                    //    new CategoryModel(){Id="46f188db-a2a8-44d5-b171-c0adbbb3d7d9", Name="Mobile" },
                    //    new CategoryModel(){Id="e175cd99-72c9-441e-a43d-f959d9b0e3e4", Name="Mobile headphones" }
                    //};
                    //}

                    var objectToSend = new
                    {
                        Category = new { Category.Id, Category.Name },
                        Heading = this.Heading.Value,
                        Detail = this.Detail.Value,
                        Terms = this.Terms.Value,
                        ValidFrom = this.ImmediatelyAvailable ? default(DateTime?) : this.Validfrom.Value.Value,
                        Image = imageModel != null ? imageModel.url : Image,
                        Categories = Categories.Select(c => new { c.Id, c.Name }).ToArray(),
                        OfferLocation = new
                        {
                            this.Location.Id,
                            this.Location.Lat,
                            this.Location.Long,
                            this.Location.Name,
                            this.Location.Selected
                        },
                        Id
                    };


                    var result = !string.IsNullOrEmpty(this.Id) ? await service.UpdateOffer<HttpResult<object>>(objectToSend) : await service.CreateOffer<HttpResult<object>>(objectToSend);

                    if (!result.IsError)
                    {
                        await ExecuteCancelCommand();
                        await RaiseSuccess("The offer has been saved successfully.");
                    }
                    else
                        await RaiseError(result.Errors.First().Description);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    await RaiseError("An error occurred while saving offer.");
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        async Task ExecuteLoadOfferCommand()
        {
            if (!IsBusy)
                IsBusy = true;
            try
            {
                var service = new RestService();
                var result = await service.GetOfferToEdit<HttpResult<OfferModel>>(Id);
                if (!result.IsError)
                {
                    var model = result.Result;
                    this.Heading.Value = model.OfferHeadline;
                    this.Image = model.Image;

                    if (!string.IsNullOrEmpty(model.Image))
                        this.SourceImage = ImageSource.FromUri(new Uri(model.Image));

                    this.Detail.Value = model.OfferDescription;
                    this.Terms.Value = model.TermsAndConditions;
                    if (model.ValidFrom != null)
                    {
                        this.Validfrom.Value = model.ValidFrom.Value.Date;
                    }

                    if (model.Categories != null)
                    {
                        this.Categories.Clear();
                        foreach (var item in model.Categories)
                        {
                            this.Categories.Add(new CategoryModel() { Id = item.Id, Name = item.Name, Selected = item.Selected });
                        }
                    }

                    if (model.Locations != null)
                    {
                        this.Location.DisplayAddress = model.Locations.DisplayAddress;
                        this.Location.Id = model.Locations.Id;
                        this.Location.Lat = model.Locations.Lat;
                        this.Location.Long = model.Locations.Long;
                        this.Location.Name = model.Locations.Name;
                        this.Location.Selected = model.Locations.Selected;
                    }
                }
                else
                {
                    await RaiseError(result.Errors.First().Description);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await RaiseError("An error occurred while fetching offer.");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
