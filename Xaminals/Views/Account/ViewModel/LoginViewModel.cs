using Plugin.ValidationRules;
using Xaminals.ViewModels;

namespace Xaminals.Views.Account.ViewModel
{
    public partial class LoginViewModel : BaseViewModel
    {
        private ValidatableObject<string> _username;
        private ValidatableObject<string> _password;
        private bool _areCredentialsInvalid;

        public LoginViewModel()
        {
            AreCredentialsInvalid = false;
        }      

        public ValidatableObject<string> Username
        {
            get => _username;
            set
            {
                if (value == _username) return;
                _username = value;
                OnPropertyChanged(nameof(Username));
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

        public bool AreCredentialsInvalid
        {
            get => _areCredentialsInvalid;
            set
            {
                if (value == _areCredentialsInvalid) return;
                _areCredentialsInvalid = value;
                OnPropertyChanged(nameof(AreCredentialsInvalid));
            }
        }      

        protected override void IntializeMembers()
        {
            base.IntializeMembers();
            _password = new ValidatableObject<string>();
            _username = new ValidatableObject<string>();
        }
    }
}
