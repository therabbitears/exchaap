using Plugin.Share;
using Rg.Plugins.Popup.Services;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xaminals.Infra.Mappers;
using Xaminals.Infra.Results;
using Xaminals.Services.HttpServices;
using Xaminals.Views.Offer_Public;
using Xaminals.Views.Offers.ViewModels;

namespace Xaminals.Views.Offers.Models
{
    public partial class OfferDetailsViewModel
    {
        public ICommand LoadItemsCommand { get; set; }
        public ICommand OpenMapsCommand { get; set; }
        public ICommand StarOfferCommand { get; set; }
        public ICommand ReportOfferCommand { get; set; }
        public ICommand ChatToPublisherCommand { get; set; }
        public ICommand OpenImageCommand { get; set; }
        public ICommand ShareOfferCommand
        {
            get; set;
        }       

        protected override void AddListeners()
        {
            base.AddListeners();
            this.PropertyChanged += OnPropertChanged;
            //Id = "18d9377d-11d3-42c8-8696-0462673c18d6";
        }

        protected override void IntializeCommands()
        {
            base.IntializeCommands();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            OpenMapsCommand = new Command(async () => await OpenMaps());


            StarOfferCommand = new Command(Star);
            ReportOfferCommand = new Command(async () => await ExecuteReportOfferCommand());
            ChatToPublisherCommand = new Command(async (object obj) => await ExecuteChatToPublisherCommand(obj));
            ShareOfferCommand = new Command(async (object obj) => await ExecuteShareOfferCommand(obj));
            OpenImageCommand = new Command(async (object obj) => await ExecuteOpenImageCommand(obj));

            //MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            //{
            //    var newItem = item as Item;
            //    Items.Add(newItem);
            //    await DataStore.AddItemAsync(newItem);
            //});


        }

        private async Task ExecuteOpenImageCommand(object obj)
        {
            if (obj != null)
                await PopupNavigation.Instance.PushAsync(new OfferCriteria(new exchaup.Models.OpenImageModel(obj.ToString())), true);
        }

        private async void Star(object obj)
        {
            if (IsLoggedIn)
            {
                try
                {
                    var result = await new RestService().Star<HttpResult<object>>(new { token = this.Offer.OfferToken, locationToken = this.Offer.LocationId });
                    if (!result.IsError)
                        this.Offer.Starred = !this.Offer.Starred;
                    else
                        await RaiseError(result.Errors.First().Description);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    await RaiseError("An error occurred while starring offer.");
                }
            }
            else
            {
                await ExecutePopupLoginCommand();
            }
        }

        private async Task ExecuteReportOfferCommand()
        {
            await PopupNavigation.Instance.PushAsync(new GeneralPage(this.Offer), true);
        }

        private async Task ExecuteChatToPublisherCommand(object value)
        {
            if (IsLoggedIn)
            {
                await Shell.Current.GoToAsync($"chat?offerid=" + this.Offer.Id + "&locationid=" + this.Offer.LocationId, true);
            }
            else
            {
                await ExecutePopupLoginCommand();
            }
        }

        private async Task ExecuteShareOfferCommand(object value)
        {
            string text = "Checkout this ";
            if (Offer.Categories != null && Offer.Categories.Any())
                text += "exchange ad.";
            else
                text += "giveaway(giving for free) ad.";

            await CrossShare.Current.Share(new Plugin.Share.Abstractions.ShareMessage() { Text = text, Title = Offer.Name, Url = Offer.Url });
        }

        async Task OpenMaps()
        {
            var location = new Location(this.Offer.Coordinates.Lat, this.Offer.Coordinates.Long);
            var options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving, Name = string.Format("{0}({1})", this.Offer.PublisherName, this.Offer.LocationName) };

            await Map.OpenAsync(location, options);
        }

        async Task ExecuteLoadItemsCommand()
        {
            try
            {
                var location = await GetCurrentLocation(true);
                IsBusy = true;
                var result = await new RestService().OfferDetail<HttpResult<OfferListItemViewModel>>(Id, location.Latitude, location.Longitude, "M");
                if (!result.IsError)
                {
                    Mapper.Map(result.Result, this.Offer);
                }
                else
                {
                    await RaiseError(result.Errors.First().Description);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await RaiseError("An error occurred while loading offer details.");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OnPropertChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Id" && !string.IsNullOrEmpty(Id))
            {
                LoadItemsCommand.Execute(null);
            }
        }
    }
}