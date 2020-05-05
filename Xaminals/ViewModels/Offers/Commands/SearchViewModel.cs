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
using Xaminals.Views.Categories.Models.DTO;

namespace Xaminals.ViewModels.Offers
{
    public partial class SearchViewModel
    {
        public ICommand LoadItemsCommand { get; set; }
        public ICommand SaveCriteriaCommand { get; set; }
        public ICommand CategoryClickedCommand { get; set; }

        protected override void IntializeCommands()
        {
            base.IntializeCommands();
            LoadItemsCommand = new Command(async () => await ExecuteLoadCategoriesCommand());
            SaveCriteriaCommand = new Command(async () => await ExecuteSaveCriteriaCommand());
            CategoryClickedCommand = new Command(async (object sender) => await ExecuteCategoryClickedCommand(sender));
            LoadItemsCommand.Execute(null);
        }

        async Task ExecuteCategoryClickedCommand(object sender)
        {
            if (sender is CategoryModel category)
                category.Selected = !category.Selected;
        }

        async Task ExecuteSaveCriteriaCommand()
        {
            await PopupNavigation.Instance.PopAsync(true);
            MessagingCenter.Send<SearchViewModel>(this, "CriteriaUpdated");
        }

        async Task ExecuteLoadCategoriesCommand()
        {
            IsBusy = true;

            try
            {
                _categories.Clear();
                var result = await new RestService().OfferCategories<HttpResult<List<CategoryModel>>>();
                if (!result.IsError)
                {
                    foreach (var item in result.Result)
                    {
                        _categories.Add(item);
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
