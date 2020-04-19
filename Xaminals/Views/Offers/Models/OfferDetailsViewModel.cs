using System;
using Xamarin.Forms;
using Xaminals.ViewModels;
using Xaminals.Views.Offers.ViewModels;

namespace Xaminals.Views.Offers.Models
{
    [QueryProperty("LocationId", "locationid")]
    [QueryProperty("Id", "offerid")]
    public partial class OfferDetailsViewModel : BaseViewModel
    {
        private GeneralOfferViewModel _offer;
        public GeneralOfferViewModel Offer
        {
            get { return _offer; }
            set { _offer = value; }
        }

        protected override void IntializeMembers()
        {
            base.IntializeMembers();
            Offer = new GeneralOfferViewModel();
        }

        private string _id;

        public string Id
        {
            get { return _id; }
            set
            {
                var newValue = Uri.UnescapeDataString(value);
                if (newValue != _id)
                {
                    _id = newValue;
                    OnPropertyChanged("Id");
                }
            }
        }

        private string _locationId;
        public string LocationId
        {
            get { return _locationId; }
            set
            {
                var newValue = Uri.UnescapeDataString(value);
                if (newValue != _locationId)
                {
                    _locationId = newValue;
                    OnPropertyChanged("LocationId");
                }
            }
        }
    }
}
