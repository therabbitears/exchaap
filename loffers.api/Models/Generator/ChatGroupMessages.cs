using System;
using System.ComponentModel.DataAnnotations;

namespace loffers.api.Models.Generator
{
    public class ChatGroupMessages
    {
        [Key]
        public long ChatGroupMessageID { get; set; }

        public DateTime CreatedOn { get; set; }

        [Required]
        [StringLength(500)]
        public string Message { get; set; }

        public long ChatGroupUserID { get; set; }

        public ChatGroupUsers ChatGroupUsers { get; set; }

        public int Status { get; set; }
    }
}