using Loffers.Views.Chat.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Loffers.Models
{
    public class ChatModel
    {
        private ChatGroupViewModel _group;
        public ChatGroupViewModel Group
        {
            get { return _group; }
            set { _group = value; }
        }

        List<ChatGroupUserViewModel> _users;
        public List<ChatGroupUserViewModel> Users
        {
            get { return _users; }
            set { _users = value; }
        }
    }
}
