using exchaup.Views.Common;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xaminals.Views.Offers.Models;

namespace Xaminals.Views.Offers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Offer : ContentPage
    {
        public Offer()
        {
            InitializeComponent();
        }

        private void OnListToGetsFocus(object sender, FocusEventArgs e)
        {
            Shell.Current.Navigation.PushAsync(new CategorySelection(this.BindingContext as ICategorySelectable, 1));
        }

        private void OnExchangeWithGetsFocus(object sender, FocusEventArgs e)
        {
            var bindingContext = this.BindingContext as ICategoriesSelectable;
            Shell.Current.Navigation.PushAsync(new CategorySelection(bindingContext, bindingContext.MaxAllowed));
        }
    }
}