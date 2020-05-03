using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xaminals.CustomControls.Search;

namespace Xaminals.Views.Offers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OfferList : ContentPage
    {
        double intialY = 0;
        public OfferList()
        {
            InitializeComponent();
            intialY = bottomSheet.Y;
            Subscribe();
        }

        void Subscribe()
        {
            MessagingCenter.Subscribe<QuickFilter>(this, "CloseCrieteria", async (obj) =>
            {
                var layout = bottomSheet.Bounds;
                layout.Y = layout.Y + 200;
                await bottomSheet.LayoutTo(layout, 250, Easing.Linear);

                //await bottomSheet.TranslateTo(bottomSheet.X, intialY, 200, Easing.Linear);
            });

            MessagingCenter.Subscribe<QuickFilter>(this, "ShowCrieteria", async (obj) =>
            {
                var layout = bottomSheet.Bounds;
                layout.Y = layout.Y - 200;
                await bottomSheet.LayoutTo(layout, 250, Easing.Linear);
            });
        }
    }
}