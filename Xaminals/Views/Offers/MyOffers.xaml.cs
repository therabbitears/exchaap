using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xaminals.Views.Offers.ViewModels;

namespace Xaminals.Views.Offers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyOffers : ContentPage
    {
        public MyOffers()
        {
            InitializeComponent();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as OfferModel;
            if (item == null)
                return;

            ShellNavigationState state = Shell.Current.CurrentState;
            await Shell.Current.GoToAsync($"offer?offerid=" + item.Id, true);

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

    }
}