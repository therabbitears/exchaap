using exchaup.Data;
using exchaup.Models;
using System.Collections.ObjectModel;
using Xaminals.ViewModels;
using Xaminals.Views.Categories.Models.DTO;

namespace exchaup.Views.Home.Model
{
    public partial class StartupScreenCardsViewModel : BaseViewModel
    {
        private ObservableCollection<StartupCardModel> _cards;
        public ObservableCollection<StartupCardModel> Cards
        {
            get { return _cards; }
            set { _cards = value; }
        }

        private ObservableCollection<CategoryModel> _categories;
        public ObservableCollection<CategoryModel> FilteredCategories
        {
            get { return _categories; }
            set { _categories = value; }
        }

        protected override void IntializeMembers()
        {
            base.IntializeMembers();
            this.Cards = new ObservableCollection<StartupCardModel>();
            this.FilteredCategories = new ObservableCollection<CategoryModel>(DataStore.Categories());
            this.Cards.Add(new StartupCardModel()
            {
                Title = "Things get old soon?",
                ShortTitle = "Or rather we can have limited use out from them.",
                Description = "But people around you may still want it, in return of their 'old' stuff, they might be still new for someone? List your old stuff like books, toys, games, mobiles or anything and find people who looking for it in exchange of their old stuff.",
                Image = "https://img.icons8.com/flat_round/4x/year-of-monkey.png",
                IsFirstScreen = true
            });

            this.Cards.Add(new StartupCardModel()
            {
                Title = "Things are ageless",
                ShortTitle = "And we utilize them just upto our needs.",
                Description = "Not everything has an age, like few things. We limit the use of stuff upto our needs and then we don't want them. But people around you may still want it, it might still worthy for them. List your such stuff to giveaway to them who need it. It is not always money, but sometimes it's about utilize them for upto their best value.",
                Image = "giveaway.png",
                ShowGoButton = true,
                IsSecondScreen = true
            });
        }
    }
}
