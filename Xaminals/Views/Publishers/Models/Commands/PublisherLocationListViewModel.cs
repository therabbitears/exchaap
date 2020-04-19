using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xaminals.Infra.Attributes;
using Xaminals.Infra.Results;
using Xaminals.Services.HttpServices;
using Xaminals.Views.Categories.Models.DTO;

namespace Xaminals.Views.Publishers.Models
{
    public partial class PublisherLocationListViewModel
    {
        public ICommand AddNewCommand { get; set; }
        public Command LoadItemsCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand OnTappedCommand { get; set; }

        protected override void IntializeCommands()
        {
            base.IntializeCommands();
            AddNewCommand = new Command(AddNew);
            DeleteCommand = new Command(Delete);
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            OnTappedCommand = new Command(async (object tappedItem) => await ExecuteOnTappedCommand(tappedItem));

            //MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            //{
            //    var newItem = item as Item;
            //    Items.Add(newItem);
            //    await DataStore.AddItemAsync(newItem);
            //});

            LoadItemsCommand.Execute(null);
        }

        async Task ExecuteOnTappedCommand(object tappedItem)
        {
            await PopupNavigation.Instance.PushAsync(new PublisherLocation(tappedItem as PublisherLocationModel), true);
        }

        protected override void OnLoginStateChanged(bool isLoggedIn)
        {
            base.OnLoginStateChanged(isLoggedIn);
            if (isLoggedIn)
                LoadItemsCommand.Execute(null);
        }

        protected override void OnUserLogsOut()
        {
            base.OnUserLogsOut();
            this.AvailableLocations.Clear();
        }

        private async void Delete(object obj)
        {
            // Delete
        }

        [Debt("Need to move this code out from here and handle it by messaging.")]
        private void AddNew(object obj)
        {
            PopupNavigation.Instance.PushAsync(new PublisherLocation(), true);
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsLoggedIn)
            {
                if (IsBusy)
                    return;

                IsBusy = true;

                try
                {
                    // var location = await Geolocation.GetLastKnownLocationAsync();

                    _availableLocations.Clear();
                    var result = await new RestService().PublisherLocations<HttpResult<List<PublisherLocationModel>>>();
                    if (!result.IsError)
                    {
                        foreach (var item in result.Result)
                        {
                            _availableLocations.Add(item);
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
                    await RaiseError("An error occurred while loading locations.");
                }
                finally
                {
                    IsBusy = false;
                }
            }
            else
                _availableLocations.Clear();
        }
    }
}
