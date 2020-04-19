namespace loffers.api.Models.Generator
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Locations
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Locations()
        {
            PublisherLocations = new HashSet<PublisherLocations>();
        }

        [Key]
        public long LocationID { get; set; }

        public decimal Lat { get; set; }

        public decimal Long { get; set; }

        public bool Active { get; set; }

        [Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        [Required]
        [StringLength(50)]
        public string LastEditedBy { get; set; }

        public DateTime LastEditedOn { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string DisplayAddress { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PublisherLocations> PublisherLocations { get; set; }
    }
}
