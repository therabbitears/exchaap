using Xaminals.Validations;

namespace Xaminals.Views.Account.ViewModel
{
    public partial class LoginViewModel
    {
        protected override void AddValidations()
        {
            base.AddValidations();
            _username.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Please enter username to login."
            });

            _password.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Please enter password to login."
            });
        }

        private bool Validate()
        {
            bool isValidUser = ValidateUserName();
            bool isValidPassword = ValidatePassword();
            return isValidUser && isValidPassword;
        }

        private bool ValidateUserName()
        {
            return _username.Validate();
        }

        private bool ValidatePassword()
        {
            return _password.Validate();
        }
    }
}
