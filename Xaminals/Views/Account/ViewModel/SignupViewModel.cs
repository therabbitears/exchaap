using Plugin.ValidationRules;
using Xaminals.ViewModels;

namespace Xaminals.Views.Account.ViewModel
{
    public partial class SignupViewModel : BaseViewModel
    {
        private ValidatableObject<bool> _agreed;
        private ValidatableObject<string> _name;
        private ValidatableObject<string> _email;
        private ValidatableObject<string> _password;
        private ValidatableObject<string> _phoneNumber;
        private ValidatableObject<string> _confirmPassword;
        private ValidatableObject<string> _currentPassword;

        private bool _areCredentialsInvalid;

        public SignupViewModel()
        {

        }

        protected override void IntializeMembers()
        {
            base.IntializeMembers();
            _agreed = new ValidatableObject<bool>();
            _name = new ValidatableObject<string>();
            _currentPassword = new ValidatableObject<string>();
            _password = new ValidatableObject<string>();
            _confirmPassword = new ValidatableObject<string>();
            _email = new ValidatableObject<string>();
            _phoneNumber = new ValidatableObject<string>();
            AreCredentialsInvalid = false;
        }

        public ValidatableObject<bool> Agreed
        {
            get => _agreed;
            set
            {
                if (value == _agreed) return;
                _agreed = value;
                OnPropertyChanged(nameof(Agreed));
            }
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

        public ValidatableObject<string> Password
        {
            get => _password;
            set
            {
                if (value == _password) return;
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ValidatableObject<string> CurrentPassword
        {
            get => _currentPassword;
            set
            {
                if (value == _currentPassword) return;
                _currentPassword = value;
                OnPropertyChanged(nameof(CurrentPassword));
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

        public ValidatableObject<string> ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                if (value == _confirmPassword) return;
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }

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
