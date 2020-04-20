using exchaup.CustomControls.SearchHandlers;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace exchaup.Views.Offer_Public.Models
{
    public partial class SerachLocationListViewModel
    {
        public ICommand SearchCommand { get; set; }

        protected override void IntializeCommands()
        {
            base.IntializeCommands();
            Locations = new System.Collections.ObjectModel.ObservableCollection<SearchLocationItemViewModel>();
            SearchCommand = new Command(async (object sender) => await ExecuteSearchCommand(sender));
        }

        async Task ExecuteSearchCommand(object sender)
        {
            var data = MonkeyData.Monkeys.Where(monkey => monkey.Name.ToLower().Contains(sender.ToString().ToLower()));

            this.Locations.Clear();
            foreach (var item in data)
            {
                Locations.Add(new SearchLocationItemViewModel { Name = item.Name, ImageUrl = item.ImageUrl });
            }
        }
    }
}
