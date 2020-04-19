using System;
using System.Collections.Generic;

namespace Loffers.Server.Models
{
    public partial class Categories
    {
        public Categories()
        {
            OfferCategories = new HashSet<OfferCategories>();
            PublisherLocationCategories = new HashSet<PublisherLocationCategories>();
            UserCategories = new HashSet<UserCategories>();
        }

        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public bool Active { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastEditedBy { get; set; }
        public DateTime LastEditedOn { get; set; }
        public string Id { get; set; }
        public int CategoryType { get; set; }

        public virtual ICollection<OfferCategories> OfferCategories { get; set; }
        public virtual ICollection<PublisherLocationCategories> PublisherLocationCategories { get; set; }
        public virtual ICollection<UserCategories> UserCategories { get; set; }
    }
}
