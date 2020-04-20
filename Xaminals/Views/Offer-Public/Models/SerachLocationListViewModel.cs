using System.Collections.ObjectModel;
using Xaminals.ViewModels;

namespace exchaup.Views.Offer_Public.Models
{
    public partial class SerachLocationListViewModel : BaseViewModel
    {
        ObservableCollection<SearchLocationItemViewModel> _locations;
        public ObservableCollection<SearchLocationItemViewModel> Locations
        {
            get { return _locations; }
            set { _locations = value; }
        }

        protected override void IntializeMembers()
        {
            base.IntializeMembers();
            Locations = new ObservableCollection<SearchLocationItemViewModel>();
        }
    }
}
