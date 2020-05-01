using exchaup.Views.Offer_Public;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xaminals.Infra.Results;
using Xaminals.Services.HttpServices;
using Xaminals.Views.Offer_Public;

namespace Xaminals.Views.Offers.ViewModels
{
    public partial class OffersListViewModel
    {
        public Command LoadItemsCommand { get; set; }
        public ICommand ShowCritriaCommand { get; set; }
        public ICommand OnTappedCommand { get; set; }
        public ICommand OnDataRequiredCommand { get; set; }
        public ICommand SelectLocationCommand { get; set; }
        public ICommand SaveLastItemsCommand { get; set; }

        protected override void IntializeCommands()
        {
            base.IntializeCommands();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand(true));
            ShowCritriaCommand = new Command(async () => await ExecuteShowCritriaCommand());
            OnTappedCommand = new Command(async (object sender) => await ExecuteOnTappedCommand(sender));
            OnDataRequiredCommand = new Command(async (object sender) => await ExecuteLoadItemsCommand(false));
            SelectLocationCommand = new Command(async (object sender) => await Shell.Current.Navigation.PushAsync(new SearchLocation()));
            SaveLastItemsCommand = new Command(async (object sender) => ExecuteSaveLastItemsCommand(sender));
            LoadItemsCommand.Execute(null);
        }

        async Task ExecuteOnTappedCommand(object sender)
        {
            if (sender is OfferListItemViewModel selectedObject)
            {
                ShellNavigationState state = Shell.Current.CurrentState;
                await Shell.Current.GoToAsync($"offerdetails?offerid=" + selectedObject.Id + "&locationid=" + selectedObject.LocationToken, true);
            }
        }

        async Task ExecuteShowCritriaCommand()
        {
            //await PopupNavigation.Instance.PushAsync(new OfferCriteria(), true);
        }

        protected virtual async Task ExecuteLoadItemsCommand(bool isRefresh)
        {
            try
            {
                HasItems = true;
                if (isRefresh)
                    CurrentPageNumber = 0;

                var meter = Context.SettingsModel.UnitOfMeasurement == (int)App.UnitOfMeasurement.Killometers ? MaxDistance * 1000 : MaxDistance * 1609;

                double latVal = 0; double longVal = 0;
                var location = await GetCurrentLocation(!isRefresh);
                latVal = location.Latitude;
                longVal = location.Longitude;
                IsBusy = true;
                var items = await new RestService().SearchOffers<HttpResult<List<OfferListItemViewModel>>>(latVal, longVal, meter, "M", Categories.ToArray(), CurrentPageNumber);
                if (!items.IsError)
                {
                    PopulateViewContext(items.Result, isRefresh);
                }
                else
                {
                    await RaiseError(items.Errors.First().Description);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await RaiseError("An error occurred while searching for offers.");
            }
            finally
            {
                IsBusy = false;
            }
        }

        protected virtual void PopulateViewContext(List<OfferListItemViewModel> result, bool isRefresh)
        {
            if (isRefresh)
            {
                _myOffers.Clear();
                SaveLastItemsCommand.Execute(result);
            }

            foreach (var item in result)
            {
                _myOffers.Add(item);
            }

            HasItems = _myOffers.Any();
        }

        async void ExecuteSaveLastItemsCommand(object v)
        {
            if (v != null && v is List<OfferListItemViewModel> list)
            {
               // await Database.SaveItemAsync(list);
            }
        }
    }
}