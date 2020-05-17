using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xaminals.Views.Settings
{
#if DEBUG
    [XamlCompilation(XamlCompilationOptions.Skip)]
#else
   [XamlCompilation(XamlCompilationOptions.Compile)]
#endif
    public partial class PersonalSettings : ContentPage
    {
        public PersonalSettings()
        {
            InitializeComponent();
        }
    }
}