using Xaminals.Validations;

namespace Xaminals.Views.Publishers.Models
{
    public partial class PublisherLocationViewModel
    {
        protected override void AddValidations()
        {
            _displayName.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Please enter location name."
            });

            _displayAddress.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Please enter the display address, this will help your customers to find you."
            });

            _lat.Validations.Add(new IsNotNullOrEmptyRule<double>
            {
                ValidationMessage = "Your device address is not accessible or you have left the latitude value empty."
            });

            _long.Validations.Add(new IsNotNullOrEmptyRule<double>
            {
                ValidationMessage = "Your device address is not accessible or you have left the longitude value empty."
            });
        }

        private bool Validate()
        {
            return _displayName.Validate() &&
                _displayAddress.Validate() &&
                _lat.Validate() &&
                _long.Validate();
        }
    }
}
