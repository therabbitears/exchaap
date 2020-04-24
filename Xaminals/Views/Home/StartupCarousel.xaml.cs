using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xaminals;

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
    }
}