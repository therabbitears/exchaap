namespace loffers.api.Models.Generator
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class UserCategories
    {
        [Key]
        public int UserCategoryId { get; set; }

        public int CategoryID { get; set; }

        [Required]
        [StringLength(50)]
        public string UserId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime LastEditedOn { get; set; }

        public bool Active { get; set; }

        public virtual Categories Categories { get; set; }
    }
}
