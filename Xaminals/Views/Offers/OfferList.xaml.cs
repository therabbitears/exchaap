using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xaminals.Views.Offers
{
#if DEBUG
    [XamlCompilation(XamlCompilationOptions.Skip)]
#else
   [XamlCompilation(XamlCompilationOptions.Compile)]
#endif
    public partial class OfferList : ContentPage
    {
        public OfferList()
        {
            InitializeComponent();
        }
    }
}