using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xaminals.ViewModels;
using Xaminals.Views.Offers.Models;

namespace Xaminals.Views.Offers.ViewModels
{
    public partial class ReportOfferViewModel : BaseViewModel
    {
        private IOfferViewModel _offer;
        public IOfferViewModel Offer
        {
            get { return _offer; }
            set { _offer = value; OnPropertyChanged("Offer"); }
        }

        public ObservableCollection<ReportOptionModel> Options { get; set; } = new ObservableCollection<ReportOptionModel>()
        {
            new ReportOptionModel{ Text="Offensive and inappropriate"},
            new ReportOptionModel{ Text="Incorrect information"},
            new ReportOptionModel{ Text="Vendor denied the offer"},
            new ReportOptionModel{ Text="Other"}
        };

        private string _comment;
        public string Comment
        {
            get { return _comment; }
            set { _comment = value; OnPropertyChanged("Comment"); }
        }

        protected override void IntializeMembers()
        {
            base.IntializeMembers();
        }
    }
}