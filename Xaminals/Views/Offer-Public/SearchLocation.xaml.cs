using exchaup.Views.Offer_Public.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xaminals.Views.Categories.Models.DTO;

namespace exchaup.Views.Offer_Public
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchLocation : ContentPage
    {
        public SearchLocation(ILocation location)
        {
            InitializeComponent();
            if (this.BindingContext is SerachLocationListViewModel context)
            {
                context.Selectable = location;
            }
        }
    }
}