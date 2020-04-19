using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xaminals.Views.Offers.ViewModels
{
    public partial class OfferMapListViewModel
    {
        public ICommand LoadCurrenLocCommand { get; set; }

        protected override void IntializeCommands()
        {
            LoadCurrenLocCommand = new Command(async () => await LoadCurrenLocation(null));
            LoadCurrenLocCommand.Execute(null);
            base.IntializeCommands();
        }

        private async Task LoadCurrenLocation(object obj)
        {
            var position = await Plugin.Geolocator.CrossGeolocator.Current.GetPositionAsync();
            MyLocation = new Coordinates(position.Latitude, position.Longitude);
        }
    }
}
