using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loffers.Server.Data.Authentication
{
    [Table("Users")]
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
