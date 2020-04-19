using Plugin.ValidationRules;
using Xaminals.ViewModels;

namespace Xaminals.Views.Publishers.Models
{
    public partial class PublisherLocationViewModel : BaseViewModel
    {
        private ValidatableObject<string> _displayName;
        public ValidatableObject<string> Name
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        private ValidatableObject<string> _displayAddress;
        public ValidatableObject<string> DisplayAddress
        {
            get { return _displayAddress; }
            set { _displayAddress = value; }
        }


        private ValidatableObject<double> _lat;
        public ValidatableObject<double> Lat
        {
            get { return _lat; }
            set { _lat = value; }
        }

        private ValidatableObject<double> _long;
        public ValidatableObject<double> Long
        {
            get { return _long; }
            set { _long = value; }
        }


        private string _Id;
        public string Id
        {
            get { return _Id; }
            set { SetProperty(ref _Id, value); }
        }


        protected override void IntializeMembers()
        {
            base.IntializeMembers();
            this.Title = "Publisher locations";
            _displayName = new ValidatableObject<string>();
            _displayAddress = new ValidatableObject<string>();
            _lat = new ValidatableObject<double>();
            _long = new ValidatableObject<double>();
        }
    }
}
