using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xaminals.ViewModels;
using Xaminals.Views.Offers.ViewModels;

namespace Loffers.Views.Chat.Models
{
    [QueryProperty("GroupName", "groupname")]
    [QueryProperty("LocationId", "locationid")]
    [QueryProperty("Id", "offerid")]
    public partial class ChatViewModel : BaseViewModel
    {
        private ObservableCollection<MessageViewModel> _messages;
        public ObservableCollection<MessageViewModel> Messages
        {
            get { return _messages; }
            set { _messages = value; }
        }

        private GeneralOfferViewModel _offer;
        public GeneralOfferViewModel Offer
        {
            get { return _offer; }
            set { _offer = value; }
        }

        private ChatGroupViewModel _group;
        public ChatGroupViewModel Group
        {
            get { return _group; }
            set { _group = value; }
        }

        ObservableCollection<ChatGroupUserViewModel> _users;
        public ObservableCollection<ChatGroupUserViewModel> Users
        {
            get { return _users; }
            set { _users = value; }
        }

        private string _grpupName;
        public string GroupName
        {
            get { return _grpupName; }
            set
            {
                var newValue = Uri.UnescapeDataString(value);
                if (newValue != _grpupName)
                {
                    _grpupName = newValue;
                    OnPropertyChanged("GroupName");
                }
            }
        }

        private string _id;
        public string Id
        {
            get { return _id; }
            set
            {
                var newValue = Uri.UnescapeDataString(value);
                if (newValue != _id)
                {
                    _id = newValue;
                    OnPropertyChanged("Id");
                }
            }
        }


        private string _locationId;
        public string LocationId
        {
            get { return _locationId; }
            set
            {
                var newValue = Uri.UnescapeDataString(value);
                if (newValue != _locationId)
                {
                    _locationId = newValue;
                    OnPropertyChanged("LocationId");
                }
            }
        }

        private string _messGE;
        public string Message
        {
            get { return _messGE; }
            set { _messGE = value; OnPropertyChanged("Message"); }
        }

        protected override void IntializeMembers()
        {
            base.IntializeMembers();
            Messages = new ObservableCollection<MessageViewModel>();
            Offer = new GeneralOfferViewModel();
            this.Group = new ChatGroupViewModel();
            this.Users = new ObservableCollection<ChatGroupUserViewModel>();
        }
    }
}
