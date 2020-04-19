using System.Collections.ObjectModel;
using Xaminals.Views.Categories.Models.DTO;

namespace Xaminals.ViewModels.Offers
{
    public partial class SearchViewModel : BaseViewModel
    {
        private int _MaxDistance=1;
        public int MaxDistance
        {
            get { return _MaxDistance; }
            set { _MaxDistance = value; OnPropertyChanged("MaxDistance"); }
        }

        ObservableCollection<CategoryModel> _categories;
        public ObservableCollection<CategoryModel> Categories
        {
            get
            {
                return _categories;
            }
            set
            {
                _categories = value;
            }
        }

        public ObservableCollection<PublisherLocationModel> SearchResults { get; private set; }

        protected override void IntializeMembers()
        {
            base.IntializeMembers();
            this.Categories = new ObservableCollection<CategoryModel>();
            SearchResults = new ObservableCollection<PublisherLocationModel>();
        }
    }
}
