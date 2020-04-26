using Xaminals.Validations;

namespace Xaminals.Views.Offers.Models
{
    public partial class OfferViewModel
    {
        protected override void AddValidations()
        {
            _heading.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Please enter ad heading."
            });

            _detail.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "Please enter short detail of this ad, like brand, age, genre etc."
            });
        }

        private bool Validate()
        {
            IsCategorySelected = Category != null && !string.IsNullOrEmpty(Category.Id);
            return ValidateHeading() && ValidateDetail() && IsCategorySelected;
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
