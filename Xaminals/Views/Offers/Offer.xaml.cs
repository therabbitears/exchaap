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
            Shell.Current.Navigation.PushAsync(new CategorySelection(false, this.BindingContext as ICategoriesSelectable));
        }

        private void OnExchangeWithGetsFocus(object sender, FocusEventArgs e)
        {
            Shell.Current.Navigation.PushAsync(new CategorySelection(true, this.BindingContext as ICategoriesSelectable));
        }
    }
}