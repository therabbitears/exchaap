using System;
using Xaminals.ViewModels;

namespace Loffers.Views.Chat.Models
{
    public class MessageViewModel : BaseViewModel
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged("Message"); }
        }

        private bool _isSelf;
        public bool IsSelf
        {
            get { return _isSelf; }
            set { _isSelf = value; OnPropertyChanged("IsSelf"); }
        }

        private DateTime _stamp;
        public DateTime Stamp
        {
            get { return _stamp; }
            set { _stamp = value; OnPropertyChanged("Stamp"); }
        }

        private string _grpupName;
        public string GroupName
        {
            get { return _grpupName; }
            set { _grpupName = value; OnPropertyChanged("GroupName"); }
        }

    }
}