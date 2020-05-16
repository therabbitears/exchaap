using Plugin.ValidationRules;
using Xaminals.ViewModels;

namespace Xaminals.Views.Settings.ViewModels
{
    public partial class PersonalSettingsViewModel : BaseViewModel
    {
        private ValidatableObject<string> _name;
        private ValidatableObject<string> _email;
        private ValidatableObject<string> _phoneNumber;


        protected override void IntializeMembers()
        {
            base.IntializeMembers();
            _name = new ValidatableObject<string>();
            _email = new ValidatableObject<string>();
            _phoneNumber = new ValidatableObject<string>();
        }

        public ValidatableObject<string> Name
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public ValidatableObject<string> Email
        {
            get => _email;
            set
            {
                if (value == _email) return;
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public ValidatableObject<string> PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (value == _phoneNumber) return;
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        bool _areCredentialsInvalid;
        public bool AreCredentialsInvalid
        {
            get => _areCredentialsInvalid;
            set
            {
                SetProperty(ref _areCredentialsInvalid, value);
            }
        }

        private string _LastError;
        public string LastError
        {
            get => _LastError;
            set { SetProperty(ref _LastError, value); }
        }
    }
}
