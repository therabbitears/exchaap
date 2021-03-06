using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loffers.Server.Data.Authentication
{
    public class UpdatePasswordModel
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class UpdateUserModel
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
    }
}
