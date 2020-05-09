using exchaup.Views.Common.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xaminals.Views.Offers.Models;

namespace exchaup.Views.Common
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategorySelection : ContentPage
    {
        public CategorySelection()
        {
            InitializeComponent();
            if (this.BindingContext is CategorySelectionViewModel context)
                context.SelectInto = new OfferViewModel();
        }

        public CategorySelection(ISelectable category) : this()
        {
            if (this.BindingContext is CategorySelectionViewModel context)
            {
                context.SelectInto = category;
                if (category is ICategoriesSelectable multiple)
                    context.MultiSelection = true;
            }
        }
    }
}