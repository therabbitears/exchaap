using System;
using System.Collections.Generic;

namespace Loffers.Server.Models
{
    public partial class Offers
    {
        public Offers()
        {
            OfferCategories = new HashSet<OfferCategories>();
            OfferComplaints = new HashSet<OfferComplaints>();
            OfferLocations = new HashSet<OfferLocations>();
            UserStarredOffers = new HashSet<UserStarredOffers>();
        }

        public long OfferId { get; set; }
        public int PublisherId { get; set; }
        public string OfferHeadline { get; set; }
        public string OfferDescription { get; set; }
        public string TermsAndConditions { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTill { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastEditedBy { get; set; }
        public DateTime LastEditedOn { get; set; }
        public string CreatedBy { get; set; }
        public string Image { get; set; }
        public string Id { get; set; }

        public virtual Publishers Publisher { get; set; }
        public virtual ICollection<OfferCategories> OfferCategories { get; set; }
        public virtual ICollection<OfferComplaints> OfferComplaints { get; set; }
        public virtual ICollection<OfferLocations> OfferLocations { get; set; }
        public virtual ICollection<UserStarredOffers> UserStarredOffers { get; set; }
    }
}
