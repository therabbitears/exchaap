using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        }

        protected override void AddListeners()
        {
            base.AddListeners();
            this.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RequireData" && this.RequireData)
                LoadItemsCommand.Execute(null);
        }

        async Task ExecuteCategoryClickedCommand(object sender)
        {
            if (sender is CategoryModel category)
            {
                var single = await Context.Database.GetCategoryAsync(category.Id);
                if (single != null)
                {
                    single.Selected = !category.Selected;
                    await Context.Database.UpdateCategoryAsync(single);
                }

                category.Selected = !category.Selected;
            }
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
                var result = await new RestService().OfferCategories<HttpResult<List<CategoryModel>>>();
                if (!result.IsError)
                {
                    foreach (var item in result.Result.Where(c => !string.IsNullOrEmpty(c.ParentId)))
                    {
                        var firstOrDefault = _categories.FirstOrDefault(c => c.Id == item.Id);
                        if (firstOrDefault == null)
                        {
                            await Context.Database.InsertCategoryAsync(item);
                            _categories.Add(item);
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
