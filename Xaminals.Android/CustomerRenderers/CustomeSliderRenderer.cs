using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(exchaup.CustomControls.CustomeSlider), typeof(Xaminals.Droid.CustomerRenderers.CustomeSliderRenderer))]
namespace Xaminals.Droid.CustomerRenderers
{
    public class CustomeSliderRenderer : SliderRenderer
    {
        public CustomeSliderRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement == null) return;
            Control.SetPadding(Control.PaddingLeft, 0, Control.PaddingRight, 15);
        }
    }
}