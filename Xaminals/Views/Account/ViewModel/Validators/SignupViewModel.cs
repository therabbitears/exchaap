using Xaminals.Validations;

namespace Xaminals.Views.Account.ViewModel
{
    public partial class SignupViewModel
    {
        protected override void AddValidations()
        {
            _agreed.Validations.Add(new MustBeTrueRule<bool>
            {
                ValidationMessage = "Please confirm you are agreed with app terms and privacy policy."
            });

            _name.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Please enter your name."
            });

            _email.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Please enter your email address."
            });

            _email.Validations.Add(new EmailRule<string>
            {
                ValidationMessage = "Please enter a valid email address."
            });

            _currentPassword.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Please enter the current password."
            });

            _password.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Please enter your password."
            });

            _confirmPassword.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Please confirm the password."
            });

            _confirmPassword.Validations.Add(new CompareValuesRule<string>() { CompareFunction = () => _password.Value, ValidationMessage = "Password and confirmed password required to be same." });

        }

        private bool Validate()
        {
            var userValid = ValidateName();
            var agreed = ValidateAgreed();
            var validUserName = ValidateUserName();
            var validPassword = ValidatePassword();
            var validConfirm = ValidateConfirmPassword();
            return userValid && agreed && validUserName && validPassword && validConfirm;
        }

        private bool ValidateAgreed()
        {
            return _agreed.Validate();
        }

        private bool ValidateUpdate()
        {
            var userValid = ValidateName();
            var validUserName = ValidateUserName();
            return userValid && validUserName;
        }

        private bool ValidateUpdatePassword()
        {
            var currentPassword = ValidateCurrentPassword();
            var validPassword = ValidatePassword();
            var validConfirm = ValidateConfirmPassword();

            return
                currentPassword
                && validPassword
                && validConfirm;
        }

        private bool ValidateName()
        {
            return _name.Validate();
        }

        private bool ValidateCurrentPassword()
        {
            return _currentPassword.Validate();
        }

        private bool ValidateConfirmPassword()
        {
            return _confirmPassword.Validate();
        }

        private bool ValidateUserName()
        {
            return _email.Validate();
        }

        private bool ValidatePassword()
        {
            return _password.Validate();
        }
    }
}
