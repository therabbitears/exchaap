using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xaminals.Infra.Results;
using Xaminals.Services.HttpServices;
using Xaminals.Views.Offers.ViewModels;

namespace Xaminals.Views.Offers.Models
{
    public partial class MyOffersListViewModel
    {
        public Command LoadItemsCommand { get; set; }
        public Command EditOfferCommand { get; set; }
        public Command AddOfferCommand { get; set; }
        public Command DeactivateCommand { get; set; }


        protected override void IntializeCommands()
        {
            base.IntializeCommands();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            AddOfferCommand = new Command(async () => await ExecuteAddOfferCommand());
            EditOfferCommand = new Command(EditOffer);
            DeactivateCommand = new Command(async (object sender) => await ExecuteDeactivateCommand(sender));
            LoadItemsCommand.Execute(null);
        }

        protected override void AddListeners()
        {
            base.AddListeners();
            MessagingCenter.Subscribe<OfferViewModel>(this, "OfferAdded", async (obj) =>
            {
                await ExecuteLoadItemsCommand();
            });
        }

        async Task ExecuteAddOfferCommand()
        {
            if (IsLoggedIn)
            {
                ShellNavigationState state = Shell.Current.CurrentState;
                await Shell.Current.GoToAsync($"offer", true);
            }
            else
            {
                await ExecutePopupLoginCommand();
            }
        }

        protected override void OnLoginStateChanged(bool isLoggedIn)
        {
            base.OnLoginStateChanged(isLoggedIn);
            if (isLoggedIn)
                LoadItemsCommand.Execute(null);
        }

        protected override void OnUserLogsOut()
        {
            base.OnUserLogsOut();
            this.MyOffers.Clear();
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsLoggedIn)
            {
                if (IsBusy)
                    return;

                HasItems = IsBusy = true;

                try
                {
                    _myOffers.Clear();
                    var result = await new RestService().MyOffers<HttpResult<List<OfferModel>>>();
                    if (!result.IsError)
                    {
                        foreach (var item in result.Result)
                        {
                            _myOffers.Add(item);
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
                    await RaiseError("An error occurred while loading your offers.");
                }
                finally
                {
                    IsBusy = false;
                    HasItems = _myOffers.Any();
                }
            }
            else
                _myOffers.Clear();
        }


        async void EditOffer(object obj)
        {
            if (offerSelectedItem != null)
            {
                ShellNavigationState state = Shell.Current.CurrentState;
                await Shell.Current.GoToAsync($"offer?offerid=" + offerSelectedItem.Id, true);
            }
        }

        async Task ExecuteDeactivateCommand(object sender)
        {
            if (sender is OfferModel offer)
            {
                IsBusy = true;
                try
                {
                    var result = await new RestService().ActivateOffers<HttpResult<bool>>(new { offer.Id, offer.Active });
                    if (!result.IsError)
                        offer.Active = !offer.Active;
                    else
                        await RaiseError(result.Errors.First().Description);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    await RaiseError("An error occurred while updating the ad.");
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }
    }
}
