using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;
using Xaminals.ViewModels;

namespace exchaup.CustomControls.Search
{
#if DEBUG
    [XamlCompilation(XamlCompilationOptions.Skip)]
#else
   [XamlCompilation(XamlCompilationOptions.Compile)]
#endif
    public partial class ExtendedSearch : PopupPage
    {
        public ExtendedSearch(object binding)
        {
            InitializeComponent();
            this.BindingContext = binding;
        }

        protected override void OnAppearingAnimationEnd()
        {
            base.OnAppearingAnimationEnd();
            (this.BindingContext as BaseViewModel).Context.SearchModel.RequireData = true;
        }

        protected override void OnDisappearingAnimationEnd()
        {
            base.OnDisappearingAnimationEnd();
            (this.BindingContext as BaseViewModel).Context.SearchModel.RequireData = false;
        }
    }
}