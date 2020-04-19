namespace loffers.api.Models.Generator
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Offers
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Offers()
        {
            OfferCategories = new HashSet<OfferCategories>();
            OfferComplaints = new HashSet<OfferComplaints>();
            OfferLocations = new HashSet<OfferLocations>();
            UserStarredOffers = new HashSet<UserStarredOffers>();
        }

        [Key]
        public long OfferID { get; set; }

        public int PublisherID { get; set; }

        [Required]
        [StringLength(255)]
        public string OfferHeadline { get; set; }

        [Required]
        [StringLength(500)]
        public string OfferDescription { get; set; }

        [Required]
        [StringLength(5000)]
        public string TermsAndConditions { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTill { get; set; }

        public bool Active { get; set; }

        public DateTime CreatedOn { get; set; }

        [Required]
        [StringLength(50)]
        public string LastEditedBy { get; set; }

        public DateTime LastEditedOn { get; set; }

        [Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }

        [StringLength(200)]
        public string Image { get; set; }

        [Required]
        [StringLength(70)]
        public string Id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OfferCategories> OfferCategories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OfferComplaints> OfferComplaints { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OfferLocations> OfferLocations { get; set; }

        public virtual Publishers Publishers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserStarredOffers> UserStarredOffers { get; set; }
    }
}
