using exchaup.Models;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;
using Xaminals.Infra.Context;

namespace Xaminals.Views.Offer_Public
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OfferCriteria : PopupPage
    {
        public OfferCriteria(OpenImageModel model)
        {
            InitializeComponent();
            this.BindingContext = model;
        }
    }


}