using Xaminals.Models;

namespace exchaup.Views.Offer_Public.Models
{
    public class SearchLocationItemViewModel : NotificableObject
    {
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; OnPropertyChanged("Name"); }
        }

        private string _Landmark;
        public string Landmark
        {
            get { return _Landmark; }
            set { _Landmark = value; OnPropertyChanged("Landmark"); }
        }

        private double _Lat;
        public double Lat
        {
            get { return _Lat; }
            set { _Lat = value; OnPropertyChanged("Lat"); }
        }

        private double _Long;
        public double Long
        {
            get { return _Long; }
            set { _Long = value; OnPropertyChanged("Long"); }
        }

        private bool _IsCurrent;
        public bool IsCurrent
        {
            get { return _IsCurrent; }
            set { _IsCurrent = value; OnPropertyChanged("IsCurrent"); }
        }
    }
}
