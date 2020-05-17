using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Loffers.Views.Settings
{
#if DEBUG
    [XamlCompilation(XamlCompilationOptions.Skip)]
#else
   [XamlCompilation(XamlCompilationOptions.Compile)]
#endif
    public partial class About : ContentPage
    {
        public About()
        {
            InitializeComponent();
        }
    }
}