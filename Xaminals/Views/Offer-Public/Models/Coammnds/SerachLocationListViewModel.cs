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

        protected override void AddListeners()
        {
            base.AddListeners();
            this.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Query")
                SearchCommand.Execute(Query);
        }

        async Task ExecuteItemSelectionCommand(object sender)
        {
            var selectedItem = sender as SearchLocationItemViewModel;
            try
            {
                if (this.Selectable != null)
                {
                    if (!selectedItem.IsCurrent)
                    {
                        this.Selectable.Long = selectedItem.Long;
                        this.Selectable.Lat = selectedItem.Lat;
                    }

                    this.Selectable.DisplayAddress = selectedItem.Landmark;
                    this.Selectable.Name = selectedItem.Name;
                    this.Selectable.IsCurrent = selectedItem.IsCurrent;
                }
                else
                {
                    Context.SettingsModel.SelectedLocation.SelectedAt = DateTime.Now;
                    Context.SettingsModel.SelectedLocation.IsCurrent = selectedItem.IsCurrent;
                    Context.SettingsModel.SelectedLocation.Landmark = selectedItem.Landmark;
                    Context.SettingsModel.SelectedLocation.Long = selectedItem.Long;
                    Context.SettingsModel.SelectedLocation.Lat = selectedItem.Lat;
                    Context.SettingsModel.SelectedLocation.Name = selectedItem.Name;
                    if (!selectedItem.IsCurrent)
                    {
                        await AddLocation(selectedItem);
                    }
                    await Database.DeleteStates();
                    var newState = new exchaup.Models.ApplicationStateModel()
                    {
                        SkipIntro = true,
                        CustomLocation = !selectedItem.IsCurrent,
                        LastLocationName = selectedItem.Name,
                        Long = selectedItem.Long,
                        Lat = selectedItem.Lat
                    };
                    await Database.SaveLastState(newState);
                }

                await Shell.Current.Navigation.PopAsync(true);
            }
            catch (Exception ex)
            {
                await RaiseError(ex.Message);
                Debug.WriteLine(ex.Message);
            }
        }

        async Task ExecuteLoadExistingCommand(object sender)
        {
            try
            {
                this.Saved.Add(new SearchLocationItemViewModel() { Landmark = "Current location", Lat = 0, Long = 0, Name = "Current location", IsCurrent = true });
                var items = await Database.GetAllLocationsAsync();
                if (items != null)
                {
                    foreach (var item in items.OrderByDescending(c => c.IsCurrent))
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

        string lastQuery = string.Empty;
        async Task ExecuteSearchCommand(object sender)
        {
            if (IsBusy)
                return;

            if (!string.IsNullOrEmpty(Query) && Query.Length > 2)
            {
                lastQuery = Query;
                IsBusy = true;

                try
                {
                    var result = await new RestService().SearchLocations<HttpResult<List<SearchLocationItemViewModel>>>(Query);
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

                if (lastQuery != Query)
                    await ExecuteSearchCommand(Query);
            }
        }

        async Task AddLocation(SearchLocationItemViewModel selectedItem)
        {
            var existing = await Database.FindSingle(selectedItem);
            if (existing == null)
                await Database.SaveLocationAsync(selectedItem);

            Saved.Add(selectedItem);
        }
    }
}