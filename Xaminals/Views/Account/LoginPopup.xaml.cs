using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace Xaminals.Views.Account
{
#if DEBUG
    [XamlCompilation(XamlCompilationOptions.Skip)]
#else
   [XamlCompilation(XamlCompilationOptions.Compile)]
#endif
    public partial class LoginPopup : PopupPage
    {
        public LoginPopup()
        {
            InitializeComponent();
        }
    }
}