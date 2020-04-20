using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xaminals.Infra.Results;
using Xaminals.Services.HttpServices;

namespace exchaup.Views.Offer_Public.Models
{
    public partial class SerachLocationListViewModel
    {
        public ICommand SearchCommand { get; set; }
        public ICommand ItemSelectionCommand { get; set; }

        protected override void IntializeCommands()
        {
            base.IntializeCommands();            
            SearchCommand = new Command(async (object sender) => await ExecuteSearchCommand(sender));
            ItemSelectionCommand = new Command(async (object sender) => await ExecuteItemSelectionCommand(sender));
        }

        async Task ExecuteItemSelectionCommand(object sender)
        {
            await Database.SaveLocationAsync(sender as SearchLocationItemViewModel);
        }

        async Task ExecuteSearchCommand(object sender)
        {
            IsBusy = true;

            try
            {
                var result = await new RestService().SearchLocations<HttpResult<List<SearchLocationItemViewModel>>>(sender.ToString());
                if (!result.IsError)
                {
                    this.Locations.Clear();

                    foreach (var item in result.Result)
                    {
                        this.Locations.Add(item);
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
                await RaiseError("An error occurred while searching for locations.");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}