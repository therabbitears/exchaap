using Xaminals.Models;

namespace Xaminals.Views.Categories.Models.DTO
{
    public class PublisherLocationModel : NotificableObject, ILocation
    {
        public string Id { get; set; }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; OnPropertyChanged("Name"); }
        }

        private string _DisplayAddress;
        public string DisplayAddress
        {
            get { return _DisplayAddress; }
            set { _DisplayAddress = value;OnPropertyChanged("DisplayAddress"); }
        }

        public double Lat { get; set; }
        public double Long { get; set; }

        bool _isCurrent;
        public bool IsCurrent
        {
            get { return _isCurrent; }
            set { _isCurrent = value; OnPropertyChanged("IsCurrent"); }
        }
    }

    public class OfferPublisherLocationModel : PublisherLocationModel
    {
        public bool Selected { get; set; }
    }
}
