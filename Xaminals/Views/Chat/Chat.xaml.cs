using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Loffers.Views.Chat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
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