using exchaup.Views.Offer_Public.Models;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xaminals.Views.Categories.Models.DTO;

namespace exchaup.Views.Offer_Public
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchLocationPopup : PopupPage
    {
        public SearchLocationPopup(ILocation location = null)
        {
            InitializeComponent();
            if (this.BindingContext is SerachLocationListViewModel context)
            {
                context.Selectable = location;
            }
        }
    }
}