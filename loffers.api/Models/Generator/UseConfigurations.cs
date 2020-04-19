using System;
using System.ComponentModel.DataAnnotations;

namespace loffers.api.Models.Generator
{
    public class UseConfigurations
    {
        [Key]
        [Required]
        [StringLength(65)]
        public string UserId { get; set; }

        [Required]
        [StringLength(500)]
        public string Configuration { get; set; }

        public DateTime LastEditedOn { get; set; }
    }
}