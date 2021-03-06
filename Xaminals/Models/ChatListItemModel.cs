
using Newtonsoft.Json;
using System;
using Xaminals.Models;
using Xaminals.Views.Categories.Models.DTO;

namespace Loffers.Models
{
    public class ChatListItemModel : NotificableObject
    {
        private DateTime _stamp;
        public DateTime Stamp
        {
            get { return _stamp; }
            set { _stamp = value; OnPropertyChanged("Stamp"); }
        }

        public string Sender { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged("Message"); }
        }

        [JsonProperty("OfferHeadline")]
        public string OfferHeading { get; set; }
        public string Image { get; set; }

        private bool _isSelf;
        public bool IsSelf
        {
            get { return _isSelf; }
            set { _isSelf = value; OnPropertyChanged("IsSelf"); }
        }

        private string _displayName;
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        public string OfferId { get; set; }

        private int _status;
        public int Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged("Status"); }
        }

        private CategoryModel _category;
        public CategoryModel Category
        {
            get { return _category; }
            set { _category = value; }
        }

    }
}