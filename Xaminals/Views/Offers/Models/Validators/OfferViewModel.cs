using Xaminals.Validations;

namespace Xaminals.Views.Offers.Models
{
    public partial class OfferViewModel
    {
        protected override void AddValidations()
        {
            _heading.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Please enter offer heading/title."
            });

            _detail.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Please enter detail/description of the offer."
            });
        }     

        private bool Validate()
        {
            bool isValidHeading = ValidateHeading();
            bool isValidDetail = ValidateDetail();
            return isValidHeading && isValidDetail;
        }

        private bool ValidateHeading()
        {
            return _heading.Validate();
        }

        private bool ValidateDetail()
        {
            return _detail.Validate();
        }
    }
}
