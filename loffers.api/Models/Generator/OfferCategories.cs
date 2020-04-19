namespace loffers.api.Models.Generator
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OfferCategories
    {
        [Key]
        public int OfferCategoryID { get; set; }

        public long OfferID { get; set; }

        public int CategoryID { get; set; }

        [Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(50)]
        public string LastEditedBy { get; set; }

        public DateTime? LastEditedOn { get; set; }

        [Required]
        [StringLength(70)]
        public string Id { get; set; }

        public bool Active { get; set; }

        public virtual Categories Categories { get; set; }

        public virtual Offers Offers { get; set; }
    }
}
