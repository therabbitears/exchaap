using System;
using System.Collections.Generic;

namespace Loffers.Server.Models
{
    public partial class PublisherLocationCategories
    {
        public long PublisherLocationCategoryId { get; set; }
        public long? PublisherLocationId { get; set; }
        public int? CategoryId { get; set; }
        public bool? Active { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastEditedBy { get; set; }
        public DateTime LastEditedOn { get; set; }

        public virtual Categories Category { get; set; }
        public virtual PublisherLocations PublisherLocation { get; set; }
    }
}
