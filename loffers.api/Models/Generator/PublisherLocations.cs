namespace loffers.api.Models.Generator
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PublisherLocations
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PublisherLocations()
        {
            OfferLocations = new HashSet<OfferLocations>();
            PublisherLocationCategories = new HashSet<PublisherLocationCategories>();
        }

        [Key]
        public long PublisherLocationID { get; set; }

        public int? PublisherID { get; set; }

        public long LocationID { get; set; }

        public bool? Active { get; set; }

        [Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [Required]
        [StringLength(50)]
        public string LastEditedBy { get; set; }

        public DateTime LastEditedOn { get; set; }

        [StringLength(200)]
        public string Image { get; set; }

        [Required]
        [StringLength(70)]
        public string Id { get; set; }

        public virtual Locations Locations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OfferLocations> OfferLocations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PublisherLocationCategories> PublisherLocationCategories { get; set; }

        public virtual Publishers Publishers { get; set; }
    }
}
