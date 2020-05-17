using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace Loffers.Views.Account
{
#if DEBUG
    [XamlCompilation(XamlCompilationOptions.Skip)]
#else
   [XamlCompilation(XamlCompilationOptions.Compile)]
#endif
    public partial class ConfirmPopup : PopupPage
    {
        public ConfirmPopup()
        {
            InitializeComponent();
        }
    }
}