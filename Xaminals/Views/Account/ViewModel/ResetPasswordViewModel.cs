using Plugin.ValidationRules;
using Xaminals.Validations;
using Xaminals.Views.Settings.ViewModels;

namespace Loffers.Views.Account.ViewModel
{
    public partial class ResetPasswordViewModel : ChangePasswordViewModel
    {
        private ValidatableObject<string> _securityCode;
        private ValidatableObject<string> _username;

        private bool _IsCodeSent;
        public bool IsCodeSent
        {
            get { return _IsCodeSent; }
            set { SetProperty(ref _IsCodeSent, value); }
        }

        protected override void IntializeMembers()
        {
            base.IntializeMembers();
            _securityCode = new ValidatableObject<string>();
            _username = new ValidatableObject<string>();
        }        

        public ValidatableObject<string> SecurityCode
        {
            get => _securityCode;
            set
            {
                if (value == _securityCode) return;
                _securityCode = value;
                OnPropertyChanged(nameof(SecurityCode));
            }
        }

        public ValidatableObject<string> UserName
        {
            get => _username;
            set
            {
                if (value == _username) return;
                _username = value;
                OnPropertyChanged(nameof(UserName));
            }
        }
    }
}
