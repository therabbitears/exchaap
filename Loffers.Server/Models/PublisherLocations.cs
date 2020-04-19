using System;
using System.Collections.Generic;

namespace Loffers.Server.Models
{
    public partial class PublisherLocations
    {
        public PublisherLocations()
        {
            OfferLocations = new HashSet<OfferLocations>();
            PublisherLocationCategories = new HashSet<PublisherLocationCategories>();
        }

        public long PublisherLocationId { get; set; }
        public int? PublisherId { get; set; }
        public long LocationId { get; set; }
        public bool? Active { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastEditedBy { get; set; }
        public DateTime LastEditedOn { get; set; }
        public string Image { get; set; }
        public string Id { get; set; }

        public virtual Locations Location { get; set; }
        public virtual Publishers Publisher { get; set; }
        public virtual ICollection<OfferLocations> OfferLocations { get; set; }
        public virtual ICollection<PublisherLocationCategories> PublisherLocationCategories { get; set; }
    }
}
