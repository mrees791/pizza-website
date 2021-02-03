using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Users
{
    public class UserRoleModel
    {
        public int Id { get; set; }
        public UserModel User { get; set; }
        public string RoleName { get; set; }
    }
}
