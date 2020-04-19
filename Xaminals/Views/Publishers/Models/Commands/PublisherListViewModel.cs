using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xaminals.Infra.Results;
using Xaminals.Services.HttpServices;

namespace Xaminals.Views.Publishers.Models
{
    public partial class PublisherListViewModel
    {
        public ICommand LoadItemsCommand { get; set; }
        public ICommand AddPublisherCommand { get; set; }
        public ICommand EditPublisherCommand { get; set; }

        protected override void IntializeCommands()
        {
            base.IntializeCommands();
            LoadItemsCommand = new Command(async () => await LoadMyPublihsers());
            AddPublisherCommand = new Command(async () => await AddPublisher());
            EditPublisherCommand = new Command(async () => await EditPublisher());
            LoadItemsCommand.Execute(null);
        }

        async Task EditPublisher()
        {
            if (SelectedItem != null)
                await Shell.Current.GoToAsync($"publisher?publisherid=" + SelectedItem.Id, true);
        }

        async Task AddPublisher()
        {
            await Shell.Current.GoToAsync($"publisher", true);
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
            this.Publishers.Clear();
        }

        async Task LoadMyPublihsers()
        {
            if (IsLoggedIn)
            {
                if (IsBusy)
                    return;

                IsBusy = true;

                try
                {
                    _publishers.Clear();
                    var result = await new RestService().Publishers<HttpResult<List<PublisherListItemViewModel>>>();
                    if (!result.IsError)
                    {
                        foreach (var item in result.Result)
                        {
                            _publishers.Add(item);
                        }
                    }
                    else
                        await RaiseError(result.Errors.First().Description);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    await RaiseError("An error occurred while loading publishers.");
                }
                finally
                {
                    HasItems = _publishers.Any();
                    IsBusy = false;
                }
            }
            else
                _publishers.Clear();
        }
    }
}
