using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Diagnostics;
using Xamarin.Forms.Xaml;
using Xaminals.Views.Categories.Models.DTO;
using Xaminals.Views.Publishers.Models;

namespace Xaminals.Views.Publishers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PublisherLocation : PopupPage
    {
        public PublisherLocation()
        {
            InitializeComponent();
        }

        public PublisherLocation(PublisherLocationModel publisherLocationModel)
        {
            InitializeComponent();
            try
            {
                (this.BindingContext as PublisherLocationViewModel).Id = publisherLocationModel.Id;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}