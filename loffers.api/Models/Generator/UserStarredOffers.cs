namespace loffers.api.Models.Generator
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserStarredOffers
    {
        [Key]
        public int StarredOfferId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserId { get; set; }

        public long OfferId { get; set; }

        public bool Active { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime LastEditedOn { get; set; }

        public long OfferLocationID { get; set; }

        public virtual OfferLocations OfferLocations { get; set; }

        public virtual Offers Offers { get; set; }
    }
}
