using Xaminals.ViewModels;

namespace Loffers.Views.Chat.Models
{
    public class ChatGroupUserViewModel : BaseViewModel
    {
        private string _displayName;
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; OnPropertyChanged("DisplayName"); }
        }

        private string _userId;
        public string UserId
        {
            get { return _userId; }
            set { _userId = value; OnPropertyChanged("UserId"); }
        }

        public bool IsSelf { get; set; }
    }
}