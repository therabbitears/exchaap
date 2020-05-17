using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Loffers.Views.Chat
{
#if DEBUG
    [XamlCompilation(XamlCompilationOptions.Skip)]
#else
   [XamlCompilation(XamlCompilationOptions.Compile)]
#endif
    public partial class Chat : ContentPage
    {
       
        public Chat()
        {
            InitializeComponent();         
           
        }

        private void txtMessage_Completed(object sender, System.EventArgs e)
        {
            txtMessage.Focus();
        }
    }
}