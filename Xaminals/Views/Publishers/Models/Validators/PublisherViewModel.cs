using Xaminals.Validations;

namespace Xaminals.Views.Publishers.Models
{
    public partial class PublisherViewModel
    {
        protected override void AddValidations()
        {
            _name.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Please enter your business/shop/cafe name."
            });
        }

        private bool Validate()
        {
            return ValidateName();
        }

        private bool ValidateName()
        {
            return _name.Validate();
        }
    }
}
