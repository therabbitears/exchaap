using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xaminals.CustomControls.Search
{
#if DEBUG
    [XamlCompilation(XamlCompilationOptions.Skip)]
#else
   [XamlCompilation(XamlCompilationOptions.Compile)]
#endif
    public partial class QuickFilter : ContentView
    {
        public QuickFilter()
        {
            InitializeComponent();
        }
    }
}