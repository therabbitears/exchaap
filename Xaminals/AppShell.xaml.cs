using Loffers.Views.Chat;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xaminals.Infra.Context;
using Xaminals.Views.Offer_Public;
using Xaminals.Views.Offers;
namespace Xaminals
{
    public partial class AppShell : Shell
    {
        public Dictionary<string, Type> Routes { get; } = new Dictionary<string, Type>();

        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
            this.BindingContext = SingletonLoffersContext.Context.SessionModel;
        }

        void RegisterRoutes()
        {
            Routes.Add("offer", typeof(Offer));
            Routes.Add("chat", typeof(Chat));
            Routes.Add("offerdetails", typeof(OfferDetails));
            foreach (var item in Routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }
        }

        void OnNavigating(object sender, ShellNavigatingEventArgs e)
        {
            // Cancel any back navigation
            //if (e.Source == ShellNavigationSource.Pop)
            //{
            //    e.Cancel();
            //}
        }

        protected override bool OnBackButtonPressed()
        {
            Current.Navigation.PopAsync(true);
            return true;
        }

        private double width = 0;
        private double height = 0;
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (Context != null)
            {
                if (width != this.width || height != this.height)
                {
                    this.width = width;
                    this.height = height;
                    if (width > height)
                    {
                        Context.CurrentOrientation = 1;
                    }
                    else
                    {
                        Context.CurrentOrientation = 0;
                    }
                }
            }
        }

        public static SingletonLoffersContext Context
        {
            get { return SingletonLoffersContext.Context; }
        }
    }
}