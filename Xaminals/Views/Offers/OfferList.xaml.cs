using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xaminals.Views.Offers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OfferList : ContentPage
    {
        public OfferList()
        {
            SetValue(NavigationPage.HasNavigationBarProperty, false);
            InitializeComponent();
        }
    }
}