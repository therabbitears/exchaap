using exchaup.Models;
using exchaup.Views.Home;
using exchaup.Views.Home.Model;
using Loffers.GlobalViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xaminals.Data.Database;
using Xaminals.Infra.Context;
using Xaminals.Services.HttpServices;

[assembly: ExportFont("MerriweatherSans-Regular.ttf", Alias = "Merri")]
[assembly: ExportFont("MerriweatherSans-Bold.ttf", Alias = "MerriBold")]
[assembly: ExportFont("FontAwesome5Free-Regular-400.otf", Alias = "FontAwesomeRegular")]
[assembly: ExportFont("FontAwesome5Free-Solid-900.otf", Alias = "FontAwesomeSolid")]
//[assembly: ExportFont("Playball-Regular.ttf", Alias = "Playball")]
#if DEBUG
[assembly: XamlCompilation(XamlCompilationOptions.Skip)]
#else
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
#endif
namespace Xaminals
{
    public partial class App : Application
    {
        public enum UnitOfMeasurement
        {
            Killometers,
            Miles            
        }

        public App()
        {
            InitializeComponent();
            LastState = Database.FindLastState().Result;
            if (LastState == null)
            {
                MainPage = new StartupCarousel();                
            }
            else
            {
                MainPage = new AppShell();
            }

            MessagingCenter.Subscribe<StartupScreenCardsViewModel>(this, "GotoApp", async (obj) =>
            {
                MainPage = new AppShell();
            });
        }

        static readonly Lazy<LoffersDb> databaseLazy = new Lazy<LoffersDb>(() =>
        {
            return new LoffersDb();
        });

        public static LoffersDb Database => databaseLazy.Value;


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

        public static ApplicationStateModel LastState { get; set; }
    }
}
