using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xaminals.Views.Settings
{
#if DEBUG
    [XamlCompilation(XamlCompilationOptions.Skip)]
#else
   [XamlCompilation(XamlCompilationOptions.Compile)]
#endif
    public partial class Settings : ContentPage
    {
        public Settings()
        {
            InitializeComponent();
            this.BindingContext = App.Context.SettingsModel;
            MessagingCenter.Subscribe<App>(this, "Hi", (sender) =>
            {

            });
        }

        //private void Button_Clicked(object sender, EventArgs e)
        //{
        //    MessagingCenter.Send<Settings, string>(this, "Hi", "John");
        //}
    }
}