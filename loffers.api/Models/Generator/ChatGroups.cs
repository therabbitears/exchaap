using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace loffers.api.Models.Generator
{
    public class ChatGroups
    {
        [Key]
        public long GroupID { get; set; }

        public long OfferID { get; set; }

        public long OfferLocationID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual Offers Offers { get; set; }

        public virtual OfferLocations OfferLocations { get; set; }

        public int Status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChatGroupUsers> ChatGroupUsers { get; set; }
    }
}