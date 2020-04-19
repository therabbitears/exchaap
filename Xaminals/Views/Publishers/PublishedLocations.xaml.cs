using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xaminals.Views.Publishers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PublishedLocations : ContentPage
    {
        public PublishedLocations()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PushAsync(new PublisherLocation(), true);
        }
    }
}