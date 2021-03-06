using System.Collections.ObjectModel;
using Xaminals.Views.Categories.Models.DTO;
using Xaminals.Views.Offers.Models;

namespace Xaminals.ViewModels.Offers
{
    public partial class SearchViewModel : BaseViewModel, ICategoriesSelectable
    {
        private int _MaxDistance = 1;
        public int MaxDistance
        {
            get { return _MaxDistance; }
            set { SetProperty(ref _MaxDistance, value); }
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

        public int MaxAllowed => int.MaxValue;

        protected override void IntializeMembers()
        {
            base.IntializeMembers();
            this.Categories = new ObservableCollection<CategoryModel>();
            SearchResults = new ObservableCollection<PublisherLocationModel>();
        }
    }
}
