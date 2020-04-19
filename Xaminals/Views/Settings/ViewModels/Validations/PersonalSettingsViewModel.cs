using Xaminals.Validations;

namespace Xaminals.Views.Settings.ViewModels
{
    public partial class PersonalSettingsViewModel
    {
        protected override void AddValidations()
        {
            _name.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Please enter your name."
            });

            _email.Validations.Add(new EmailRule<string>
            {
                ValidationMessage = "Please enter your email address."
            });
        }

        private bool ValidateUpdate()
        {
            return ValidateName() && ValidateEmail();
        }

        private bool ValidateEmail()
        {
            return _email.Validate();
        }

        private bool ValidateName()
        {
            return _name.Validate();
        }
    }
}
