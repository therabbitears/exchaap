using exchaup.Views.Common;
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
            if (open)
            {
                MessagingCenter.Send<QuickFilter>(this, "CloseCrieteria");
            }
            else
            {
                MessagingCenter.Send<QuickFilter>(this, "ShowCrieteria");
            }

            open = !open;
        }

        private void OnSearchBarTapped(object sender, FocusEventArgs e)
        {
            Shell.Current.Navigation.PushAsync(new CategorySelection(true, this.BindingContext as ICategoriesSelectable));
        }
    }
}