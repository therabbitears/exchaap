using exchaup.Views.Common;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xaminals.Views.Offers.Models;

namespace exchaup.CustomControls.Search
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExtendedSearch : PopupPage
    {
        public ExtendedSearch(object binding)
        {
            InitializeComponent();
            this.BindingContext = binding;
        }

        private void SearchBar_Focused(object sender, Xamarin.Forms.FocusEventArgs e)
        {
            Shell.Current.Navigation.PushAsync(new CategorySelection(this.BindingContext as ICategoriesSelectable, int.MaxValue));
        }
    }
}