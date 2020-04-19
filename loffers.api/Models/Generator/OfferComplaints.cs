namespace loffers.api.Models.Generator
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OfferComplaints
    {
        [Key]
        public long ReportId { get; set; }

        public long OfferId { get; set; }

        [Required]
        [StringLength(1000)]
        public string ReportContent { get; set; }

        public DateTime CreatedOn { get; set; }

        [StringLength(100)]
        public string CreatedBy { get; set; }

        public virtual Offers Offers { get; set; }
    }
}
