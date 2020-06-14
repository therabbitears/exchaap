using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(exchaup.CustomControls.CustomeEntry), typeof(Xaminals.Droid.CustomeRenderers.CustomeEntryRenderer))]
namespace Xaminals.Droid.CustomeRenderers
{
    public class CustomeEntryRenderer : EntryRenderer
    {
        public CustomeEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement == null) return;
            Control.SetPadding(Control.PaddingLeft, 0, Control.PaddingRight, 15);
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Entry.TextProperty.PropertyName)
            {
                base.Control.Text = base.Element.Text;
                if (base.Control.IsFocused)
                    base.Control.SetSelection(base.Control.Text.Length);

                return;
            }
            base.OnElementPropertyChanged(sender, e);
        }
    }
}