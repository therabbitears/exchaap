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
        public ICommand SaveDataCoammnd { get; set; }
        public ICommand FetchCateoriesDataCoammnd { get; set; }

        protected override void IntializeCommands()
        {
            base.IntializeCommands();
            GoToAppCommand = new Command(async (object sender) => await ExecureGoCommand(null));
            SaveDataCoammnd = new Command((object sender) => ExecureSaveDataCoammnd(null));
            CurrentItemCommand = new Command(async (object sender) => await ExecureCurrentItemCommand(null));
            FetchCateoriesDataCoammnd = new Command(ExecureFetchCateoriesDataCoammnd);
            FetchCateoriesDataCoammnd.Execute(null);
            SaveDataCoammnd.Execute(null);
            RecordAnalyticsEventCommand.Execute(AnalyticsModel.InstanceOf(AnalyticsModel.EventNames.PageViewEvent, AnalyticsModel.ParameterNames.PageName, "startupcarousel"));
        }

        public async Task ExecureGoCommand(object p)
        {
            if (this.FilteredCategories?.Any(c => c.Selected) == true)
            {
                foreach (var item in FilteredCategories.Where(c => c.Selected))
                {
                    App.Context.SearchModel.Categories.Add(item);
                }
            }

            MessagingCenter.Send<StartupScreenCardsViewModel>(this, "GotoApp");
        }

        public async void ExecureSaveDataCoammnd(object p)
        {
            await Database.SaveLastState(new ApplicationStateModel() { SkipIntro = true });
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
