using exchaup.CustomControls.Search;
using exchaup.Views.Common;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xaminals.Views.Offers.Models;

namespace Xaminals.CustomControls.Search
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuickFilter : ContentView
    {
        private bool open;
        public QuickFilter()
        {
            InitializeComponent();
        }

        async void OnTapped(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new ExtendedSearch(this.BindingContext), true);
        }
    }
}