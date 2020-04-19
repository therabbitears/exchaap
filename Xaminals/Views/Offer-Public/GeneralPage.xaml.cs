using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;
using Xaminals.Views.Offers.ViewModels;

namespace Xaminals.Views.Offer_Public
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GeneralPage : PopupPage
    {
        public GeneralPage()
        {
            InitializeComponent();
        }

        public GeneralPage(IOfferViewModel contextModel)
        {
            InitializeComponent();
            var context = this.BindingContext as ReportOfferViewModel;
            if (context != null)
                context.Offer = contextModel;
        }
    }
}