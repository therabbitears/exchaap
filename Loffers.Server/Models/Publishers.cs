using System;
using System.Collections.Generic;

namespace Loffers.Server.Models
{
    public partial class Publishers
    {
        public Publishers()
        {
            Offers = new HashSet<Offers>();
            PublisherLocations = new HashSet<PublisherLocations>();
        }

        public int PublisherId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public bool Active { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastEditedBy { get; set; }
        public DateTime LastEditedOn { get; set; }
        public string Id { get; set; }

        public virtual ICollection<Offers> Offers { get; set; }
        public virtual ICollection<PublisherLocations> PublisherLocations { get; set; }
    }
}
