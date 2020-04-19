using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xaminals.Views.Common.Alerts.ViewModels;

namespace Xaminals.Views.Common.Alerts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Error : PopupPage
    {
        public Error(string message)
        {
            InitializeComponent();
            var context = this.BindingContext as AlertViewModel;
            if (context != null)
                context.Message = message;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            int SecondsElapsed = 0;
            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
            {
                if (SecondsElapsed < 2)
                {
                    SecondsElapsed++;
                    return true;
                }

                if (PopupNavigation.Instance.PopupStack != null && PopupNavigation.Instance.PopupStack.Any(c => c == this))
                    PopupNavigation.Instance.PopAsync(true);

                return false;
            });

        }
    }
}