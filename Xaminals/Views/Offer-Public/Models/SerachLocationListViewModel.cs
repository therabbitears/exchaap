using System.Collections.ObjectModel;
using Xaminals.ViewModels;
using Xaminals.Views.Categories.Models.DTO;

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

        ObservableCollection<SearchLocationItemViewModel> _saved;
        public ObservableCollection<SearchLocationItemViewModel> Saved
        {
            get { return _saved; }
            set { _saved = value; }
        }

        private string _Query;
        public string Query
        {
            get { return _Query; }
            set { SetProperty(ref _Query, value); }
        }

        private ILocation _selectable;
        public ILocation Selectable
        {
            get { return _selectable; }
            set { _selectable = value; }
        }


        protected override void IntializeMembers()
        {
            base.IntializeMembers();
            Locations = new ObservableCollection<SearchLocationItemViewModel>();
            Saved = new ObservableCollection<SearchLocationItemViewModel>();
        }
    }
}
