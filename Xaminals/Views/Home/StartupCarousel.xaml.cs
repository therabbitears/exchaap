using exchaup.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xaminals.ViewModels;

namespace exchaup.Views.Home
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
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
                this.BackgroundImageSource = null;
            }
            else
            {
                this.BackgroundImageSource = ImageSource.FromFile("bg.png");
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