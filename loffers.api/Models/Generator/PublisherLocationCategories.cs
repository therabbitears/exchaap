namespace loffers.api.Models.Generator
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PublisherLocationCategories
    {
        [Key]
        public long PublisherLocationCategoryID { get; set; }

        public long? PublisherLocationID { get; set; }

        public int? CategoryID { get; set; }

        public bool? Active { get; set; }

        [Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [Required]
        [StringLength(50)]
        public string LastEditedBy { get; set; }

        public DateTime LastEditedOn { get; set; }

        public virtual Categories Categories { get; set; }

        public virtual PublisherLocations PublisherLocations { get; set; }
    }
}
