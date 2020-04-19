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

namespace Xaminals.ViewModels.Settings
{
    public partial class SettingPageCategoriesViewModel
    {
        public ICommand SaveCategoriesInfoCommand { get; set; }
        public ICommand LoadCategoriesInfoCommand { get; set; }

        protected override void IntializeCommands()
        {
            base.IntializeCommands();
            SaveCategoriesInfoCommand = new Command(async () => await ExecuteSaveCategoryCommand(null));
            LoadCategoriesInfoCommand = new Command(async () => await ExecuteLoadCategoriesCommand());
            LoadCategoriesInfoCommand.Execute(null);
        }

        protected override void OnLoginStateChanged(bool isLoggedIn)
        {
            base.OnLoginStateChanged(isLoggedIn);
            if (isLoggedIn)
                LoadCategoriesInfoCommand.Execute(null);
        }

        protected override void OnUserLogsOut()
        {
            base.OnUserLogsOut();
            this.Categories.Clear();
        }

        async Task ExecuteSaveCategoryCommand(object data)
        {
            try
            {
                var result = await new RestService().SaveCategories<HttpResult<object>>(Categories.Where(c => c.Selected));
                if (!result.IsError)
                {
                    App.Context.SettingsModel.CategoriesCount = Categories.Where(c => c.Selected).Count();
                    await RaiseSuccess("Categories has been updated successfully.");
                }
                else
                {
                    await RaiseError(result.Errors.First().Description);
                }
            }
            // await Shell.Current.Navigation.PopAsync(true);}
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await RaiseError("An error occurred while saving categories.");
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task ExecuteLoadCategoriesCommand()
        {
            IsBusy = true;

            try
            {
                _categories.Clear();
                var result = await new RestService().UserCategories<HttpResult<List<CategoryModel>>>();
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
