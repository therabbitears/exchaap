using System;
using System.Collections.Generic;

namespace Loffers.Server.Models
{
    public partial class UserCategories
    {
        public int UserCategoryId { get; set; }
        public int CategoryId { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastEditedOn { get; set; }
        public bool Active { get; set; }

        public virtual Categories Category { get; set; }
    }
}
