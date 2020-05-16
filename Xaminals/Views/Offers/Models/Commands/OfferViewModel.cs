using exchaup.Views.Offer_Public;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
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
        public ICommand LoadOfferCommand { get; set; }
        public ICommand SelectLocationCommand { get; set; }
        public ICommand LoadCurrentLocationCommand { get; set; }
        public ICommand ClickedOnCategoryItemCommand { get; set; }

        public string Image { get; set; }
        public string OriginalImage { get; set; }

        protected override void IntializeCommands()
        {
            base.IntializeCommands();
            SaveOfferCommand = new Command(SaveOffer);
            SelectImageCommand = new Command(SelectImage);
            LoadCurrentLocationCommand = new Command(ExecuteLoadCurrentLocationCommand);
            ClickedOnCategoryItemCommand = new Command(ExecuteClickedOnCategoryItemCommand);
            SelectLocationCommand = new Command(async (object sender) => await Shell.Current.Navigation.PushAsync(new SearchLocation(this.Location), true));
            if (!string.IsNullOrEmpty(Id))
            {
                LoadOfferCommand = new Command(async () => await ExecuteLoadOfferCommand());
                LoadOfferCommand.Execute(null);
            }

            LoadCurrentLocationCommand.Execute(null);
        }

        void ExecuteClickedOnCategoryItemCommand(object sender)
        {
            if (sender is CategoryModel category)
                Categories.Remove(category);
        }

        async void ExecuteLoadCurrentLocationCommand()
        {
            try
            {
                var location = await LoadLocation(true);
                this.Location.Lat = location.Latitude;
                this.Location.Long = location.Longitude;

                try
                {
                    var placemarks = await Geocoding.GetPlacemarksAsync(location);
                    var placemark = placemarks?.FirstOrDefault();
                    if (placemark != null)
                    {
                        this.Location.Name = !string.IsNullOrEmpty(placemark.SubLocality) ? placemark.SubLocality : placemark.Locality;
                        this.Location.DisplayAddress = string.Format("{0},{1},{2}", placemark.Locality, placemark.AdminArea, placemark.PostalCode);
                    }
                }
                catch (Exception debug)
                {
                    this.Location.Name = "Current location";
                    this.Location.DisplayAddress = "Current location";
                    Debug.WriteLine("An error while fetching geocoding info:" + debug.Message);
                }
            }
            catch (Exception ex)
            {
                await RaiseError(ex.Message);
            }
        }

        protected override void AddListeners()
        {
            base.AddListeners();
            this.PropertyChanged += OnPropertryChanged;
            this.Location.PropertyChanged += OnLocationPropertyChanged;
            // this.Id = "18d9377d-11d3-42c8-8696-0462673c18d6";
        }

        private void OnLocationPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsCurrent")
                LoadCurrentLocationCommand.Execute(null);
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
            try
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
                        Debug.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                await RaiseError("An error occurred, make sure you have granted permission to access the media.");
                Debug.WriteLine(ex.Message);
            }
        }

        ImageModel imageModel = null;
        private async void SaveOffer(object obj)
        {
            if (Validate())
            {
                IsBusy = true;
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

                    //if (!this.Categories.Any())
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
                        Terms = this.Terms,
                        ValidFrom = this.ImmediatelyAvailable ? default(DateTime?) : this.Validfrom.Value.Value,
                        Image = imageModel != null ? imageModel.url : Image,
                        OriginalImage = imageModel != null ? imageModel.originalUrl : OriginalImage,
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
                        MessagingCenter.Send<OfferViewModel>(this, "OfferAdded");
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
                    this.OriginalImage = model.OriginalImage;

                    if (!string.IsNullOrEmpty(model.Image))
                        this.SourceImage = ImageSource.FromUri(new Uri(model.Image));

                    this.Detail.Value = model.OfferDescription;
                    this.Terms = model.TermsAndConditions;
                    if (model.ValidFrom != null)
                    {
                        this.Validfrom.Value = model.ValidFrom.Value.Date;
                    }

                    Category.Id = model.Category.Id;
                    Category.Image = model.Category.Image;
                    Category.Name = model.Category.Name;
                    if (model.Categories != null)
                    {
                        this.Categories.Clear();
                        foreach (var item in model.Categories)
                        {
                            this.Categories.Add(new CategoryModel() { Id = item.Id, Name = item.Name, Selected = item.Selected, Image = item.Image });
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
