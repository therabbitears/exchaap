using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xaminals;
using Xaminals.ViewModels;

namespace exchaup.Views.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartupCarousel : ContentPage
    {
        int count = 0;
        public StartupCarousel()
        {
            InitializeComponent();
        }

        private void CarouselView_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            try
            {
                if (count == 2)
                {
                    Application.Current.MainPage = new AppShell();
                    return;
                }

                count++;
            }
            catch (Exception ex)
            {
                // Shell 
            }
        }

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