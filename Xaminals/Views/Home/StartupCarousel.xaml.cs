using exchaup.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xaminals.ViewModels;

namespace exchaup.Views.Home
{
#if DEBUG
    [XamlCompilation(XamlCompilationOptions.Skip)]
#else
   [XamlCompilation(XamlCompilationOptions.Compile)]
#endif
    public partial class StartupCarousel : ContentPage
    {
        public StartupCarousel()
        {
            InitializeComponent();
        }

        private void CarouselView_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            if (e.CurrentItem is StartupCardModel card && card.IsSecondScreen)
            {
                bgimg.Source = ImageSource.FromFile("bgtwo.png");
            }
            else
            {
                bgimg.Source = ImageSource.FromFile("bg.png");
            }
        }

        private double width = 0;
        private double height = 0;
        protected override void OnSizeAllocated(double width, double height)
        {
            try
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
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}