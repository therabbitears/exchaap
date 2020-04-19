using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;
using Xaminals.Infra.Context;

namespace Xaminals.Views.Offer_Public
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OfferCriteria : PopupPage
    {
        public OfferCriteria()
        {
            InitializeComponent();
            this.BindingContext = SingletonLoffersContext.Context.SearchModel;
            //this.Animation = new CustomMoveAnimation(Rg.Plugins.Popup.Enums.MoveAnimationOptions.Bottom, Rg.Plugins.Popup.Enums.MoveAnimationOptions.Bottom);
        }
    }


}