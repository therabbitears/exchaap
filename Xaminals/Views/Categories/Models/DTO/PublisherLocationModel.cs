using Xaminals.Models;

namespace Xaminals.Views.Categories.Models.DTO
{
    public class PublisherLocationModel : NotificableObject, ILocation
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DisplayAddress { get; set; }
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
