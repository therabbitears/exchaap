using Loffers.Views.Chat;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xaminals.Infra.Context;
using Xaminals.ViewModels.Offers;
using Xaminals.Views.Offer_Public;
using Xaminals.Views.Offers;
using Xaminals.Views.Publishers;
using Xaminals.Views.Settings;

namespace Xaminals
{
    public partial class AppShell : Shell
    {
        public Dictionary<string, Type> Routes { get; } = new Dictionary<string, Type>();
        public ICommand HelpCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));
        public ICommand EditOfferCommand => new Command(async () => await EditOffer());

        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();
            AddListners();
            this.BindingContext = SingletonLoffersContext.Context.SessionModel;
            MessagingCenter.Subscribe<Settings>(this, "Hi", (sender) =>
            {

            });

            MessagingCenter.Subscribe<SearchViewModel>(this, "CriteriaUpdated", async (obj) =>
            {

            });

            UpdateUI(SingletonLoffersContext.Context.SessionModel.IsPublisher);
        }

        private void AddListners()
        {
            SingletonLoffersContext.Context.SessionModel.OnOnAppSpaceChanged += OnSpaceChanged;
            (Application.Current as App).ApplicationModel.OnMessageArrived += OnMessageReceived;
        }

        private void OnMessageReceived(Loffers.Views.Chat.Models.MessageViewModel message)
        {
            if (tabChats.Icon is FontImageSource icon)
            {
                // Need Badge support here.
                // icon.Color = Color.FromHex("#ec8021");
            }
        }

        private void OnSpaceChanged(bool isPublisher)
        {
            UpdateUI(SingletonLoffersContext.Context.SessionModel.IsPublisher);
        }

        private void UpdateUI(bool isPublisher)
        {
            if (!isPublisher)
            {
                RemovePublisherTabs();
                AddUserTabs();
            }
            else
            {
                RemoveUserTabs();
                AddPublisherTabs();
            }
        }

        private void AddPublisherTabs()
        {
            if (!RootTab.Items.Contains(tbPublisher))
                RootTab.Items.Insert(0, tbPublisher);

            if (!RootTab.Items.Contains(tabLocations))
                RootTab.Items.Insert(1, tabLocations);

            if (!RootTab.Items.Contains(tabMyOffers))
                RootTab.Items.Insert(2, tabMyOffers);
        }

        private void AddUserTabs()
        {
            if (!RootTab.Items.Contains(tabDiscover))
                RootTab.Items.Insert(0, tabDiscover);

            if (!RootTab.Items.Contains(tabStarred))
                RootTab.Items.Insert(2, tabStarred);
            // RootTab.Items.Insert(1, tabMap);
        }

        private void RemovePublisherTabs()
        {
            RootTab.Items.Remove(tbPublisher);
            RootTab.Items.Remove(tabMyOffers);
            RootTab.Items.Remove(tabLocations);
        }

        void RemoveUserTabs()
        {
            RootTab.Items.Remove(tabDiscover);
            //RootTab.Items.Remove(tabMap);
            RootTab.Items.Remove(tabStarred);
        }

        void RegisterRoutes()
        {
            Routes.Add("offer", typeof(Offer));
            Routes.Add("chat", typeof(Chat));
            Routes.Add("offerdetails", typeof(OfferDetails));
            Routes.Add("publisher", typeof(Publisher));

            foreach (var item in Routes)
            {
                Routing.RegisterRoute(item.Key, item.Value);
            }
        }

        async Task EditOffer()
        {
            ShellNavigationState state = Shell.Current.CurrentState;
            await Shell.Current.GoToAsync($"offer?offerid=72d90861-4203-430a-8530-e7989953b4b6");
            Shell.Current.FlyoutIsPresented = false;
        }

        void OnNavigating(object sender, ShellNavigatingEventArgs e)
        {
            // Cancel any back navigation
            //if (e.Source == ShellNavigationSource.Pop)
            //{
            //    e.Cancel();
            //}
        }

        void OnNavigated(object sender, ShellNavigatedEventArgs e)
        {
            //
        }

        protected override bool OnBackButtonPressed()
        {
            Current.Navigation.PopAsync(true);
            return true;
        }
    }
}
