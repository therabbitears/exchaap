using exchaup.Models;
using System.Collections.ObjectModel;
using Xaminals.ViewModels;

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

        protected override void IntializeMembers()
        {
            base.IntializeMembers();
            this.Cards = new ObservableCollection<StartupCardModel>();
            this.Cards.Add(new StartupCardModel()
            {
                Title = "Baboon",
                Description = "Baboons are African and Arabian Old World monkeys belonging to the genus Papio, part of the subfamily Cercopithecinae.",
                Image = "https://img.icons8.com/flat_round/4x/year-of-monkey.png"
            });

            this.Cards.Add(new StartupCardModel()
            {
                Title = "Capuchin Monkey",
                Description = "The capuchin monkeys are New World monkeys of the subfamily Cebinae. Prior to 2011, the subfamily contained only a single genus, Cebus.",
                Image = "https://img.icons8.com/emoji/4x/monkey.png"
            });

            this.Cards.Add(new StartupCardModel()
            {
                Title = "Blue Monkey",
                Description = "The blue monkey or diademed monkey is a species of Old World monkey native to Central and East Africa, ranging from the upper Congo River basin east to the East African Rift and south to northern Angola and Zambia",
                Image = "https://img.icons8.com/officel/4x/gorilla.png",
                ShowGoButton = true
            });

            LogPageViews("startupcarousel");
        }
    }
}
