using exchaup.Views.Offer_Public.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xaminals.Views.Categories.Models.DTO;

namespace exchaup.Views.Offer_Public
{
#if DEBUG
    [XamlCompilation(XamlCompilationOptions.Skip)]
#else
   [XamlCompilation(XamlCompilationOptions.Compile)]
#endif
    public partial class SearchLocation : ContentPage
    {
        public SearchLocation(ILocation location = null)
        {
            InitializeComponent();
            if (this.BindingContext is SerachLocationListViewModel context)
            {
                context.Selectable = location;
            }
        }
    }
}