using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace loffers.api.Models.Generator
{
    public class ChatGroupUsers
    {
        [Key]
        public long ChatGroupUserID { get; set; }

        public long GroupID { get; set; }

        [Required]
        [StringLength(50)]
        public string UserId { get; set; }

        public ChatGroups ChatGroups { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChatGroupMessages> ChatGroupMessages { get; set; }

        public bool Active { get; set; }
    }
}