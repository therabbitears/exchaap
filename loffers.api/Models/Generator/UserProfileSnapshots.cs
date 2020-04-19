using System;
using System.ComponentModel.DataAnnotations;

namespace loffers.api.Models.Generator
{
    public class UserProfileSnapshots
    {
        [Key]
        [StringLength(50)]
        public string UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [StringLength(65)]
        public string PrimaryEmail { get; set; }
        
        public string PrimaryPhone { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public long? LastLat { get; set; }
        
        public long? LastLong { get; set; }

        [StringLength(10)]
        public string SecurityCode { get; set; }

        public DateTime SecurityCodeValidatity { get; set; }
    }
}