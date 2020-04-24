
using exchaup.Views.Offer_Public.Models;
using System.Collections.ObjectModel;

namespace Xaminals.ViewModels.Settings
{
    public partial class SettingPageViewModel : BaseViewModel
    {
        private int _unitOfMeasurement;
        public int UnitOfMeasurement
        {
            get { return _unitOfMeasurement; }
            set { SetProperty(ref _unitOfMeasurement, value); }
        }

        private int _CategoriesCount;

        public int CategoriesCount
        {
            get { return _CategoriesCount; }
            set { SetProperty(ref _CategoriesCount, value); }
        }


        private int _maxRange = 15;
        public int MaxRange
        {
            get { return _maxRange; }
            set
            {
                SetProperty(ref _maxRange, value);
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _PhoneNumber;
        public string PhoneNumber
        {
            get { return _PhoneNumber; }
            set { SetProperty(ref _PhoneNumber, value); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        private ObservableCollection<object> _categories;
        public ObservableCollection<object> Categories
        {
            get { return _categories; }
            set { SetProperty(ref _categories, value); }
        }

        private ObservableCollection<object> _publishers;
        public ObservableCollection<object> Publishers
        {
            get { return _publishers; }
            set { SetProperty(ref _publishers, value); }
        }

        protected override void IntializeMembers()
        {
            base.IntializeMembers();
            this.Title = "Settings";
            if (App.LastState != null && !string.IsNullOrEmpty(App.LastState.LastLocationName))
            {
                this.SelectedLocation = new SearchLocationItemViewModel()
                {
                    Landmark = App.LastState.LastLocationName,
                    Lat = App.LastState.Lat,
                    Long = App.LastState.Long,
                    Name = App.LastState.LastLocationName,
                    IsCurrent = !App.LastState.CustomLocation
                };
            }
            else
            {
                this.SelectedLocation = new SearchLocationItemViewModel() { Landmark = "Current location", Lat = 0, Long = 0, Name = "Current location", IsCurrent = true };
            }
        }

        private bool _useCurrentLocation = true;
        public bool UseCurrentLocatiom
        {
            get { return _useCurrentLocation; }
            set { SetProperty(ref _useCurrentLocation, value); }
        }

        private SearchLocationItemViewModel _SelectedLocation;
        public SearchLocationItemViewModel SelectedLocation
        {
            get { return _SelectedLocation; }
            set { SetProperty(ref _SelectedLocation, value); }
        }
    }
}