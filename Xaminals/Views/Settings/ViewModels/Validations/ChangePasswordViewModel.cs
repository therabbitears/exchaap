using Xaminals.Validations;

namespace Xaminals.Views.Settings.ViewModels
{
    public partial class ChangePasswordViewModel
    {
        protected override void AddValidations()
        {
            _password.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Please enter password."
            });

            _currentPassword.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Please enter your current password."
            });

            _confirmPassword.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Please confirm password."
            });

            _confirmPassword.Validations.Add(new CompareValuesRule<string>() { CompareFunction = () => _password.Value, ValidationMessage = "Password and confirm password need to be same." });
        }

        protected virtual bool Validate()
        {
            return ValidateCurrentPassword() && ValidatePassword() && ValidateConfirmPassword();
        }

        private bool ValidateCurrentPassword()
        {
            return _currentPassword.Validate();
        }

        private bool ValidateConfirmPassword()
        {
            return _confirmPassword.Validate();
        }

        private bool ValidatePassword()
        {
            return _password.Validate();
        }
    }
}
