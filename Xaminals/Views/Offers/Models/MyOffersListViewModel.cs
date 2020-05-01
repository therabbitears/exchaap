using System.Collections.ObjectModel;
using Xaminals.ViewModels;
using Xaminals.Views.Offers.ViewModels;

namespace Xaminals.Views.Offers.Models
{
    public partial class MyOffersListViewModel : ListBaseViewModel
    {
        ObservableCollection<OfferModel> _myOffers;
        public ObservableCollection<OfferModel> MyOffers
        {
            get
            {
                return _myOffers;
            }
            set 
            {
                _myOffers = value;
            }
        }

        private OfferModel _OfferSelectedItem;

        public OfferModel offerSelectedItem
        {
            get { return _OfferSelectedItem; }
            set { _OfferSelectedItem = value; }
        }

        protected override void IntializeMembers()
        {
            base.IntializeMembers();
            MyOffers = new ObservableCollection<OfferModel>();
        }
    }
}
