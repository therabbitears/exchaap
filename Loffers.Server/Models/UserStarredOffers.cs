using System;
using System.Collections.Generic;

namespace Loffers.Server.Models
{
    public partial class UserStarredOffers
    {
        public int StarredOfferId { get; set; }
        public string UserId { get; set; }
        public long OfferId { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastEditedOn { get; set; }
        public long OfferLocationId { get; set; }

        public virtual Offers Offer { get; set; }
        public virtual OfferLocations OfferLocation { get; set; }
    }
}
