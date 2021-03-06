using exchaup.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xaminals;
using Xaminals.Infra.Results;
using Xaminals.Services.HttpServices;
using Xaminals.Views.Categories.Models.DTO;

namespace exchaup.Views.Home.Model
{
    public partial class StartupScreenCardsViewModel
    {
        public ICommand GoToAppCommand { get; set; }
        public ICommand CurrentItemCommand { get; set; }
        public ICommand FetchCateoriesDataCoammnd { get; set; }
        public ICommand FetchLocationCoammnd { get; set; }

        protected override void IntializeCommands()
        {
            base.IntializeCommands();
            GoToAppCommand = new Command(async (object sender) => await ExecureGoCommand(null));
            CurrentItemCommand = new Command(async (object sender) => await ExecureCurrentItemCommand(null));
            FetchCateoriesDataCoammnd = new Command(ExecureFetchCateoriesDataCoammnd);
            FetchLocationCoammnd = new Command(FetchLocation);
            FetchCateoriesDataCoammnd.Execute(null);
            FetchLocationCoammnd.Execute(null);
            RecordAnalyticsEventCommand.Execute(AnalyticsModel.InstanceOf(AnalyticsModel.EventNames.PageViewEvent, AnalyticsModel.ParameterNames.PageName, "startupcarousel"));
        }

        public async void FetchLocation(object obj)
        {
            try
            {
                var location = await GetCurrentLocation();
                if (location != null)
                {
                    var state = new ApplicationStateModel() { SkipIntro = true, CustomLocation = false };
                    Context.SettingsModel.SelectedLocation.SelectedAt = DateTime.Now;
                    Context.SettingsModel.SelectedLocation.IsCurrent = true;
                    Context.SettingsModel.SelectedLocation.Long = state.Long = location.Longitude;
                    Context.SettingsModel.SelectedLocation.Lat = state.Lat = location.Latitude;
                    await Database.SaveLastState(state);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task ExecureGoCommand(object p)
        {
            if (this.FilteredCategories?.Any(c => c.Selected) == true)
            {
                foreach (var item in FilteredCategories)
                {
                    var oneCategory = App.Context.SearchModel.Categories.FirstOrDefault(c => c.Id == item.Id);
                    if (oneCategory == null)
                        App.Context.SearchModel.Categories.Add(item);
                    else
                        oneCategory.Selected = item.Selected;
                }
            }

            MessagingCenter.Send<StartupScreenCardsViewModel>(this, "GotoApp");
        }

        public async Task ExecureCurrentItemCommand(object p)
        {
            if (p is StartupCardModel value)
            {

            }
        }

        async void ExecureFetchCateoriesDataCoammnd(object p)
        {
            IsBusy = true;

            try
            {
                _categories.Clear();
                var result = await new RestService().PublicCategories<HttpResult<List<CategoryModel>>>();
                if (!result.IsError)
                {
                    foreach (var item in result.Result.Where(c => string.IsNullOrEmpty(c.ParentId)))
                    {
                        _categories.Add(item);
                        foreach (var itemChild in result.Result.Where(c => c.ParentId == item.Id))
                        {
                            _categories.Add(itemChild);
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
                await RaiseError("An error occurred while loading categories.");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
