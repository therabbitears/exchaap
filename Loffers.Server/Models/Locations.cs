using System;
using System.Collections.Generic;

namespace Loffers.Server.Models
{
    public partial class Locations
    {
        public Locations()
        {
            PublisherLocations = new HashSet<PublisherLocations>();
        }

        public long LocationId { get; set; }
        public decimal Lat { get; set; }
        public decimal Long { get; set; }
        public bool Active { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastEditedBy { get; set; }
        public DateTime LastEditedOn { get; set; }
        public string Name { get; set; }
        public string DisplayAddress { get; set; }

        public virtual ICollection<PublisherLocations> PublisherLocations { get; set; }
    }
}
