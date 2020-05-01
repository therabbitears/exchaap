using Plugin.Share;
using Rg.Plugins.Popup.Services;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xaminals.Infra.Results;
using Xaminals.Services.HttpServices;
using Xaminals.Views.Offer_Public;

namespace Xaminals.Views.Offers.ViewModels
{
    public partial class OfferListItemViewModel
    {
        public ICommand OpenMapsCommand { get; set; }
        public ICommand StarOfferCommand { get; set; }
        public ICommand OpenImageCommand { get; set; }
        public ICommand ReportOfferCommand { get; set; }
        public ICommand ChatToPublisherCommand { get; set; }
        public ICommand ShareOfferCommand { get; set; }

        protected override void IntializeCommands()
        {
            base.IntializeCommands();
            OpenMapsCommand = new Command(OpenMaps);
            StarOfferCommand = new Command(Star);
            ReportOfferCommand = new Command(async () => await ExecuteReportOfferCommand());
            ChatToPublisherCommand = new Command(async (object obj) => await ExecuteChatToPublisherCommand(obj));
            ShareOfferCommand = new Command(async (object obj) => await ExecuteShareOfferCommand(obj));
            OpenImageCommand = new Command(async (object obj) => await ExecuteOpenImageCommand(obj));
        }

        private async void OpenMaps(object obj)
        {
            var offerModel = obj as OfferListItemViewModel;
            var location = new Location(offerModel.Coordinates.Lat, offerModel.Coordinates.Long);
            var options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving, Name = string.Format("{0}({1})", offerModel.PublisherName, offerModel.LocationName) };

            await Map.OpenAsync(location, options);
        }

        private async void Star(object obj)
        {
            if (IsLoggedIn)
            {
                try
                {
                    var offerModel = obj as OfferListItemViewModel;
                    var result = await new RestService().Star<HttpResult<object>>(new { token = offerModel.OfferToken, locationToken = offerModel.LocationToken });
                    if (!result.IsError)
                        this.Starred = !this.Starred;
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
            await PopupNavigation.Instance.PushAsync(new GeneralPage(this), true);
        }

        private async Task ExecuteOpenImageCommand(object obj)
        {
            if (obj != null)
                await PopupNavigation.Instance.PushAsync(new OfferCriteria(new exchaup.Models.OpenImageModel(obj.ToString())), true);
        }

        private async Task ExecuteChatToPublisherCommand(object value)
        {
            if (IsLoggedIn)
            {
                if (value is OfferListItemViewModel offer)
                    await Shell.Current.GoToAsync($"chat?offerid=" + offer.Id + "&locationid=" + offer.LocationToken, true);
            }
            else
            {
                await ExecutePopupLoginCommand();
            }
        }

        private async Task ExecuteShareOfferCommand(object value)
        {
            if (value is OfferListItemViewModel offer)
            {
                string text = "Checkout this ";
                if (offer.Categories != null && offer.Categories.Any())
                    text += "exchange ad.";
                else
                    text += "giveaway(giving for free) ad.";

                await CrossShare.Current.Share(new Plugin.Share.Abstractions.ShareMessage() { Text = text, Title = offer.Name, Url = offer.Url });
            }
        }
    }
}