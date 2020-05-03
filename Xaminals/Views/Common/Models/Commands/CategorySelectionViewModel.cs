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
using Xaminals.Views.Offers.Models;

namespace exchaup.Views.Common.Models
{
    public partial class CategorySelectionViewModel
    {
        public ICommand LoadItemsCommand { get; set; }
        public ICommand ExrecuteQueryCommand { get; set; }
        public ICommand ItemSelectionCommand { get; set; }
        public ICommand RaiseOkCommand { get; set; }

        protected override void IntializeCommands()
        {
            base.IntializeCommands();
            LoadItemsCommand = new Command(async (object sender) => await ExecuteLoadItemsCommand(sender));
            ExrecuteQueryCommand = new Command(async () => await ExecuteQuery());
            ItemSelectionCommand = new Command(async (object sender) => await ExecuteItemSelectionCommand(sender));
            RaiseOkCommand = new Command(async (object sender) => await ExecuteRaiseOkCommand(sender));            
            LoadItemsCommand.Execute(null);
        }

        async Task ExecuteRaiseOkCommand(object sender)
        {
            try
            {
                if (SelectInto != null && SelectInto is ICategoriesSelectable selectable)
                {
                    foreach (var item in SelectedCategories)
                    {
                        selectable.Categories.Add(item);
                    }
                }

                await Shell.Current.Navigation.PopAsync(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        async Task ExecuteItemSelectionCommand(object sender)
        {
            try
            {
                if (sender is CategoryModel selected)
                {
                    if (!MultiSelection && SelectInto is ICategorySelectable selectable)
                    {
                        //SelectInto.Category = selected;
                        selectable.Category.Id = selected.Id;
                        selectable.Category.Image = selected.Image;
                        selectable.Category.Name = selected.Name;
                        selectable.Category.Selected = selected.Selected;
                        await Shell.Current.Navigation.PopAsync(true);
                    }
                    else
                    {
                        if (!selected.IsParent && !SelectedCategories.Any(c => c.Id == selected.Id))
                            SelectedCategories.Add(selected);
                    }
                }
            }
            catch (Exception ex)
            {
                await RaiseError(ex.Message);
                Debug.WriteLine(ex.Message);
            }
        }

        protected override void AddListeners()
        {
            base.AddListeners();
            this.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Query")
            {
                ExrecuteQueryCommand.Execute(null);
            }
        }

        async Task ExecuteLoadItemsCommand(object sender)
        {
            IsBusy = true;

            try
            {
                _availableCategories.Clear();
                var result = await new RestService().PublicCategories<HttpResult<List<CategoryModel>>>();
                if (!result.IsError)
                {
                    foreach (var item in result.Result.Where(c => string.IsNullOrEmpty(c.ParentId)))
                    {
                        _availableCategories.Add(item);
                        foreach (var itemChild in result.Result.Where(c => c.ParentId == item.Id))
                        {
                            _availableCategories.Add(itemChild);
                        }
                    }

                    await ExecuteQuery();
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

        async Task ExecuteQuery()
        {
            try
            {
                _categories.Clear();
                foreach (var item in _availableCategories.Where(c => !string.IsNullOrEmpty(c.ParentId) && (string.IsNullOrEmpty(Query) || c.Name.Contains(Query))).OrderBy(c => c.ParentId))
                {
                    if (!_categories.Any(c => c.Id == item.ParentId))
                        _categories.Add(_availableCategories.FirstOrDefault(c => c.Id == item.ParentId));

                    _categories.Add(item);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}