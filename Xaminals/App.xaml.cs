﻿using exchaup.Models;
using exchaup.Views.Home;
using Loffers.GlobalViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xaminals.Data.Database;
using Xaminals.Infra.Context;
using Xaminals.Services.HttpServices;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Xaminals
{
    public partial class App : Application
    {
        public enum UnitOfMeasurement
        {
            Miles,
            Killometers
        }

        public App()
        {
            InitializeComponent();

            if (Database.FindLastState().Result == null)
            {
                Database.SaveLastState(new ApplicationStateModel() { SkipIntro = true });
                MainPage = new StartupCarousel();                
                return;
            }
            else
            {
                MainPage = new AppShell();
            }
        }

        protected override void OnStart()
        {
            base.OnStart();
            TryRefreshingTheValue();
        }

        async void TryRefreshingTheValue()
        {

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        static readonly Lazy<LoffersDb> lazyInitializer = new Lazy<LoffersDb>(() =>
        {
            return new LoffersDb();
        });

        public static LoffersDb Database => lazyInitializer.Value;


        static readonly Lazy<RestService> lazyServiceInitializer = new Lazy<RestService>(() =>
        {
            return new RestService();
        });

        public static RestService Service => lazyServiceInitializer.Value;

        public static SingletonLoffersContext Context
        {
            get { return SingletonLoffersContext.Context; }
        }

        public ApplicationViewModel ApplicationModel
        {
            get
            {
                return this.BindingContext as ApplicationViewModel;
            }
        }

        public static Color BaseThemeColor { get { return Color.FromHex("#D00000"); } }
        public static Color BaseThemeSecondaryColor { get { return Color.FromHex("#cf4848"); } }
    }
}
