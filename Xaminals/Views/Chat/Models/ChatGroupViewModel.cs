using Xaminals.ViewModels;

namespace Loffers.Views.Chat.Models
{
    public class ChatGroupViewModel : BaseViewModel
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        private string _owner;
        public string Owner
        {
            get { return _owner; }
            set { _owner = value; OnPropertyChanged("Owner"); }
        }
    }
}