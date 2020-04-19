using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xaminals.ViewModels;
using Xaminals.Views.Categories.Models.DTO;

namespace Xaminals.Views.Offers.ViewModels
{
    public class GeneralOfferViewModel : BaseViewModel, IOfferViewModel
    {
        public string Id { get; set; }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; OnPropertyChanged("Name"); }
        }

        private string _Detail;
        public string Detail
        {
            get { return _Detail; }
            set { _Detail = value; OnPropertyChanged("Detail"); }
        }

        private string _Terms;
        public string Terms
        {
            get { return _Terms; }
            set { _Terms = value; OnPropertyChanged("Terms"); OnPropertyChanged("TermsMultiline"); }
        }

        public string[] TermsMultiline
        {
            get
            {
                if (!string.IsNullOrEmpty(Terms))
                {
                    return Terms.Split(
                                              new[] { "\r\n", "\r", "\n" },
                                              StringSplitOptions.None
                                          ).Select(c => c).ToArray();
                }

                return new string[0];
            }
        }

        private string _Image;
        public string Image
        {
            get { return _Image; }
            set { _Image = value; OnPropertyChanged("Image"); }
        }

        public string OfferToken { get; set; }

        private string _PublisherName;
        public string PublisherName
        {
            get { return _PublisherName; }
            set { _PublisherName = value; OnPropertyChanged("PublisherName"); }
        }

        private string _PublisherLogo;
        public string PublisherLogo
        {
            get { return _PublisherLogo; }
            set { _PublisherLogo = value; OnPropertyChanged("PublisherLogo"); }
        }

        private string _LocationName;
        public string LocationName
        {
            get { return _LocationName; }
            set { _LocationName = value; OnPropertyChanged("LocationName"); }
        }

        private string _LocationAddress;
        public string LocationAddress
        {
            get { return _LocationAddress; }
            set { _LocationAddress = value; OnPropertyChanged("LocationAddress"); }
        }

        private string _SubPublisherLogo;
        public string SubPublisherLogo
        {
            get { return _SubPublisherLogo; }
            set { _SubPublisherLogo = value; OnPropertyChanged("SubPublisherLogo"); }
        }

        private DateTime _ValidFrom;
        public DateTime ValidFrom
        {
            get { return _ValidFrom; }
            set { _ValidFrom = value; OnPropertyChanged("ValidFrom"); }
        }

        private DateTime _ValidTill;
        public DateTime ValidTill
        {
            get { return _ValidTill; }
            set { _ValidTill = value; OnPropertyChanged("ValidTill"); }
        }

        private long _Distance;
        public long Distance
        {
            get { return _Distance; }
            set { _Distance = value; OnPropertyChanged("Distance"); }
        }

        public string PublisherToken { get; set; }
        public ObservableCollection<CategoryModel> Categories { get; set; }
        public Coordinates Coordinates { get; set; }
        
        private bool _Starred;
        public bool Starred
        {
            get { return _Starred; }
            set { _Starred = value;OnPropertyChanged("Starred"); }
        }

        public string LocationId { get; set; }
    }
}
