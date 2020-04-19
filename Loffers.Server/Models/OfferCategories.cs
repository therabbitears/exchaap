using System;
using System.Collections.Generic;

namespace Loffers.Server.Models
{
    public partial class OfferCategories
    {
        public int OfferCategoryId { get; set; }
        public long OfferId { get; set; }
        public int CategoryId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastEditedBy { get; set; }
        public DateTime? LastEditedOn { get; set; }
        public string Id { get; set; }
        public bool Active { get; set; }

        public virtual Categories Category { get; set; }
        public virtual Offers Offer { get; set; }
    }
}
