using System;
using System.Collections.Generic;
using Xaminals.Models;
using Xaminals.Views.Categories.Models.DTO;

namespace Xaminals.Views.Offers.ViewModels
{
    public class OfferModel : NotificableObject
    {
        public string Image { get; set; }
        public string Id { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTill { get; set; }
        public string OfferHeadline { get; set; }
        public string OfferDescription { get; set; }
        public string TermsAndConditions { get; set; }
        public List<CategoryModel> Categories { get; set; }
        public CategoryModel Category { get; set; }
        public OfferPublisherLocationModel Locations { get; set; }
        public string OriginalImage { get; set; }
        private bool _Active;
        public bool Active
        {
            get { return _Active; }
            set { _Active = value; OnPropertyChanged("Active"); }
        }
    }
}
