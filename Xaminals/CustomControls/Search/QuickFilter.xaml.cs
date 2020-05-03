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
        double intialY = 0;
        public QuickFilter()
        {
            InitializeComponent();
            intialY = bottomSheet.Y;
        }

        async void OnTapped(object sender, EventArgs e)
        {
            if (open)
            {
                await bottomSheet.TranslateTo(bottomSheet.X, intialY, 200, Easing.Linear);
            }
            else
            {
                await bottomSheet.TranslateTo(bottomSheet.X, bottomSheet.Y - 300, 200, Easing.Linear);
            }

            open = !open;
        }

        private void OnSearchBarTapped(object sender, FocusEventArgs e)
        {
            Shell.Current.Navigation.PushAsync(new CategorySelection(true, this.BindingContext as ICategoriesSelectable));
        }
    }
}