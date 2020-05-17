using exchaup.Models;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace Xaminals.Views.Offer_Public
{
#if DEBUG
    [XamlCompilation(XamlCompilationOptions.Skip)]
#else
   [XamlCompilation(XamlCompilationOptions.Compile)]
#endif
    public partial class OfferCriteria : PopupPage
    {
        public OfferCriteria(OpenImageModel model)
        {
            InitializeComponent();
            this.BindingContext = model;
        }
    }


}