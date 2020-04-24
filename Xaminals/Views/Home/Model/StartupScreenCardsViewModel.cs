﻿using exchaup.Models;
using System.Collections.ObjectModel;
using Xaminals.ViewModels;

namespace exchaup.Views.Home.Model
{
    public class StartupScreenCardsViewModel : BaseViewModel
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
                Image = "https://loffers.sklative.com/assets/icons/x-Small.png"
            });

            this.Cards.Add(new StartupCardModel()
            {
                Title = "Capuchin Monkey",
                Description = "The capuchin monkeys are New World monkeys of the subfamily Cebinae. Prior to 2011, the subfamily contained only a single genus, Cebus.",
                Image = "https://upload.wikimedia.org/wikipedia/commons/thumb/4/40/Capuchin_Costa_Rica.jpg/200px-Capuchin_Costa_Rica.jpg"
            });

            this.Cards.Add(new StartupCardModel()
            {
                Title = "Blue Monkey",
                Description = "The blue monkey or diademed monkey is a species of Old World monkey native to Central and East Africa, ranging from the upper Congo River basin east to the East African Rift and south to northern Angola and Zambia",
                Image = "https://upload.wikimedia.org/wikipedia/commons/thumb/8/83/BlueMonkey.jpg/220px-BlueMonkey.jpg"
            });
        }
    }
}
