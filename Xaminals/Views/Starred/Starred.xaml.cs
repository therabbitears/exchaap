using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xaminals.Views.Starred
{

#if DEBUG
    [XamlCompilation(XamlCompilationOptions.Skip)]
#else
   [XamlCompilation(XamlCompilationOptions.Compile)]
#endif
    public partial class Starred : ContentPage
    {
        public Starred()
        {
            InitializeComponent();
        }
    }
}