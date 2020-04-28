using Xamarin.Forms;
using Xaminals.ViewModels;

namespace exchaup.Views
{
    public class BaseContentPage : ContentPage
    {
        private double width = 0;
        private double height = 0;
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (this.BindingContext is BaseViewModel model)
            {
                if (width != this.width || height != this.height)
                {
                    this.width = width;
                    this.height = height;
                    if (width > height)
                    {
                        model.CurrentOrientation = 1;
                    }
                    else
                    {
                        model.CurrentOrientation = 0;
                    }
                } 
            }
        }
    }
}
