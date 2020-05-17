using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace exchaup.CustomControls.Search
{
#if DEBUG
    [XamlCompilation(XamlCompilationOptions.Skip)]
#else
   [XamlCompilation(XamlCompilationOptions.Compile)]
#endif
    public partial class FetchingLocation : ContentView
    {
        //CancellationToken token = new CancellationToken(false);
        public FetchingLocation()
        {
            InitializeComponent();
        }

        //protected override async void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    base.OnPropertyChanged(propertyName);
        //    if (propertyName == "IsVisible")
        //    {
        //        if (this.IsVisible)
        //        {
        //            token = new CancellationToken(false);
        //            await RotateElement(labelCompans);
        //        }
        //        else
        //        {
        //            token = new CancellationToken(true);
        //            await RotateElement(labelCompans);
        //        }
        //    }
        //}

        //private async Task RotateElement(VisualElement element)
        //{
        //    while (!token.IsCancellationRequested)
        //    {
        //        await element.RotateTo(360, 800, Easing.Linear);
        //        await element.RotateTo(0, 0); // reset to initial position
        //    }
        //}
    }
}