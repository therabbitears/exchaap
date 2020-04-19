using Plugin.ValidationRules;
using Xaminals.ViewModels;

namespace Xaminals.Views.Settings.ViewModels
{
    public partial class ChangePasswordViewModel : BaseViewModel
    {
        private ValidatableObject<string> _password;
        private ValidatableObject<string> _confirmPassword;
        protected ValidatableObject<string> _currentPassword;

        protected override void IntializeMembers()
        {
            base.IntializeMembers();
            _currentPassword = new ValidatableObject<string>();
            _password = new ValidatableObject<string>();
            _confirmPassword = new ValidatableObject<string>();
            this.Title = "Change password";
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
    }
}
