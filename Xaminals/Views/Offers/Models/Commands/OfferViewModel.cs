using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xaminals.Infra.Results;
using Xaminals.Models;
using Xaminals.Services.HttpServices;
using Xaminals.Views.Categories.Models.DTO;
using Xaminals.Views.Offers.ViewModels;
using static Xaminals.Services.HttpServices.RestService;

namespace Xaminals.Views.Offers.Models
{
    public partial class OfferViewModel
    {
        public ICommand SaveOfferCommand { get; set; }
        public ICommand SelectImageCommand { get; set; }
        public Command LoadCategoriesCommand { get; set; }
        public Command LoadLocationsCommand { get; set; }
        public Command LoadOfferCommand { get; set; }


        public string Image { get; set; }

        protected override void IntializeCommands()
        {
            base.IntializeCommands();
            SaveOfferCommand = new Command(SaveOffer);
            SelectImageCommand = new Command(SelectImage);

            if (string.IsNullOrEmpty(Id))
            {
                LoadCategoriesCommand = new Command(async () => await ExecuteLoadCategoriesCommand());
                LoadCategoriesCommand.Execute(null);

                LoadLocationsCommand = new Command(async () => await ExecuteLoadLocationsCommand());
                LoadLocationsCommand.Execute(null);
            }
            else
            {
                LoadOfferCommand = new Command(async () => await ExecuteLoadOfferCommand());
                LoadOfferCommand.Execute(null);
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
                var service = new RestService();

                if (ImageStream != null && imageModel == null)
                {
                    imageModel = await service.UploadImageAsynch(ImageStream, this.Heading.Value.Replace(" ", string.Empty) + ".png");
                    imageModel.uploaded = true;
                }

                var objectToSend = new
                {
                    Heading = this.Heading.Value,
                    Detail = this.Detail.Value,
                    Terms = this.Terms.Value,
                    ValidFrom = this.Validfrom.Value.Value,
                    Image = imageModel != null ? imageModel.url : Image,
                    Categories = Categories.Where(c => c.Selected).Select(c => new { c.Id, c.Name }).ToArray(),
                    OfferLocations = Locations.Where(c => c.Selected).Select(c => new { c.Id, c.Name }).ToArray(),
                    Id
                };
                try
                {
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


        async Task ExecuteLoadLocationsCommand()
        {
            IsBusy = true;

            try
            {
                _locations.Clear();
                var result = await new RestService().OfferLocations<HttpResult<List<OfferPublisherLocationModel>>>();
                if (!result.IsError)
                {
                    foreach (var item in result.Result)
                    {
                        _locations.Add(item);
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
                await RaiseError("An error occurred while loading locations.");
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task ExecuteLoadCategoriesCommand()
        {
            IsBusy = true;

            try
            {
                _categories.Clear();
                var result = await new RestService().OfferCategories<HttpResult<List<CategoryModel>>>();
                if (!result.IsError)
                {
                    foreach (var item in result.Result)
                    {
                        _categories.Add(item);
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
                await RaiseError("An error occurred while loading categories.");
            }
            finally
            {
                IsBusy = false;
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
                        this.Locations.Clear();
                        foreach (var item in model.Locations)
                        {
                            this.Locations.Add(new OfferPublisherLocationModel() { Id = item.Id, Name = item.Name, Selected = item.Selected });
                        }
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
