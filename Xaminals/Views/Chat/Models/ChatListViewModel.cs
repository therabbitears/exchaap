﻿using Loffers.Models;
using System.Collections.ObjectModel;
using Xaminals.ViewModels;

namespace Loffers.Views.Chat.Models
{
    public partial class ChatListViewModel : BaseViewModel
    {
        private ObservableCollection<ChatListItemModel> _chats;
        public ObservableCollection<ChatListItemModel> Chats
        {
            get { return _chats; }
            set { _chats = value; }
        }

        protected override void IntializeMembers()
        {
            base.IntializeMembers();
            this.Title = "Conversations";
            this.Chats = new ObservableCollection<ChatListItemModel>();
        }
    }
}