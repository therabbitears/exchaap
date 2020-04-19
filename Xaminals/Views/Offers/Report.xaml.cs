using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;
using Xaminals.Views.Offers.ViewModels;

namespace Xaminals.Views.Offers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Report : PopupPage
    {
        public Report(object contextModel)
        {
            InitializeComponent();
            var context = this.BindingContext as ReportOfferViewModel;
            if (context!=null)
                context.Offer = contextModel as OfferListItemViewModel;
        }

        public Report()
        {
            InitializeComponent();
        }
    }
}