using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xaminals.Infra.Results;
using Xaminals.Services.HttpServices;
using Xaminals.Views.Offers.ViewModels;

namespace Xaminals.Views.Starred.Models
{
    public partial class StarredOffersListViewModel : OffersListViewModel
    {
        protected override async Task ExecuteLoadItemsCommand(bool isReferesh)
        {
            if (IsLoggedIn)
            {
                try
                {
                    var location = await GetCurrentLocation(false);
                    IsBusy = true;
                    var result = await new RestService().Starred<HttpResult<List<OfferListItemViewModel>>>(location.Latitude, location.Longitude, "M");
                    if (!result.IsError)
                    {
                        _myOffers.Clear();
                        HasItems = result.Result?.Any() == true;
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
                    await RaiseError("An error occurred while loading your starred offers.");
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }
    }
}
