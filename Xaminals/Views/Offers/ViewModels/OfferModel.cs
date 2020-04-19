using System;
using System.Collections.Generic;
using Xaminals.Views.Categories.Models.DTO;

namespace Xaminals.Views.Offers.ViewModels
{
    public class OfferModel
    {
        public string Image { get; set; }
        public string Id { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTill { get; set; }
        public string OfferHeadline { get; set; }
        public string OfferDescription { get; set; }
        public string TermsAndConditions { get; set; }
        public List<CategoryModel> Categories { get; set; }
        public List<OfferPublisherLocationModel> Locations { get; set; }
    }
}
