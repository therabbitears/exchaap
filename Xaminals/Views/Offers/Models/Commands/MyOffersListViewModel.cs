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


        protected override void IntializeCommands()
        {
            base.IntializeCommands();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            AddOfferCommand = new Command(async () => await ExecuteAddOfferCommand());
            EditOfferCommand = new Command(EditOffer);
            LoadItemsCommand.Execute(null);
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

                IsBusy = true;

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
    }
}
