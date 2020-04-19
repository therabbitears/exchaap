using System;
using System.Collections.Generic;

namespace Loffers.Server.Models
{
    public partial class OfferLocations
    {
        public OfferLocations()
        {
            UserStarredOffers = new HashSet<UserStarredOffers>();
        }

        public long OfferLocationId { get; set; }
        public long OfferId { get; set; }
        public long? PublisherLocationId { get; set; }
        public bool Active { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastEditedBy { get; set; }
        public DateTime LastEditedOn { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTill { get; set; }

        public virtual Offers Offer { get; set; }
        public virtual PublisherLocations PublisherLocation { get; set; }
        public virtual ICollection<UserStarredOffers> UserStarredOffers { get; set; }
    }
}
