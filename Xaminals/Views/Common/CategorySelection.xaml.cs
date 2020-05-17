using exchaup.Views.Common.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xaminals.Views.Offers.Models;

namespace exchaup.Views.Common
{
#if DEBUG
    [XamlCompilation(XamlCompilationOptions.Skip)]
#else
   [XamlCompilation(XamlCompilationOptions.Compile)]
#endif
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
                if (category is ICategoriesSelectable categories && maxAllowed > 1)
                {
                    context.MultiSelection = true;
                    foreach (var item in categories.Categories)
                    {
                        context.SelectedCategories.Add(item);
                    }
                }
                else
                    context.MultiSelection = false;
            }
        }
    }
}