using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xaminals.ViewModels;
using Xaminals.ViewModels.Offers;

namespace Xaminals.Views.Offers.ViewModels
{
    public partial class OffersListViewModel : ListBaseViewModel
    {
        protected ObservableCollection<OfferListItemViewModel> _myOffers;
        public ObservableCollection<OfferListItemViewModel> Offers
        {
            get { return _myOffers; }
            set { _myOffers = value; }
        }

        public OffersListViewModel()
        {
            Title = "Search offers";
        }

        protected override void IntializeMembers()
        {
            base.IntializeMembers();
            Offers = new ObservableCollection<OfferListItemViewModel>();
            Categories = new List<string>();
            this.Title = "Discover offers";
        }

        protected override void AddListeners()
        {
            base.AddListeners();
            MessagingCenter.Subscribe<SearchViewModel>(this, "CriteriaUpdated", async (obj) =>
            {
                this.MaxDistance = Context.SearchModel.MaxDistance;
                if (Context.SearchModel.Categories != null && Context.SearchModel.Categories.Any(c => c.Selected))
                    Categories = Context.SearchModel.Categories.Where(c => c.Selected).Select(c => c.Id).ToList();

                await ExecuteLoadItemsCommand(true);
            });
        }

        private int _maxDistance = 15;
        public int MaxDistance
        {
            get { return _maxDistance; }
            set
            {
                _maxDistance = value;
                OnPropertyChanged("MaxDistance");
            }
        }

        private List<string> _categories;
        public List<string> Categories
        {
            get { return _categories; }
            set { _categories = value; }
        }
    }
}
