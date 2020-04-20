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
        public ICommand LoadExistingCommand { get; set; }
        public ICommand ItemSelectionCommand { get; set; }

        protected override void IntializeCommands()
        {
            base.IntializeCommands();
            SearchCommand = new Command(async (object sender) => await ExecuteSearchCommand(sender));
            LoadExistingCommand = new Command(async (object sender) => await ExecuteLoadExistingCommand(sender));
            ItemSelectionCommand = new Command(async (object sender) => await ExecuteItemSelectionCommand(sender));
            LoadExistingCommand.Execute(null);
        }

        async Task ExecuteItemSelectionCommand(object sender)
        {
            var selectedItem = sender as SearchLocationItemViewModel;
            try
            {
                var existing = await Database.FindSingle(selectedItem);
                if (existing == null)
                    await Database.SaveLocationAsync(selectedItem);

                Saved.Add(selectedItem);
                await Shell.Current.Navigation.PopAsync(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        async Task ExecuteLoadExistingCommand(object sender)
        {
            try
            {
                var items = await Database.GetAllLocationsAsync();
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        this.Saved.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
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