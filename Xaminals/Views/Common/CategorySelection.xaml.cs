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
        }

        public CategorySelection(ISelectable category, int maxAllowed) : this()
        {
            if (this.BindingContext is CategorySelectionViewModel context)
            {
                context.SelectInto = category;
                if (category is ICategoriesSelectable && maxAllowed > 1)
                    context.MultiSelection = true;
                else
                    context.MultiSelection = false;
            }
        }
    }
}