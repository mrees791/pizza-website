using DataLibrary.Models.Carts;
using DataLibrary.Models.CustomerOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Users
{
    public class UserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public int CurrentCartId { get; set; }
        public int ConfirmOrderCartId { get; set; }
        public int OrderConfirmationId { get; set; }
        public bool IsBanned { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string ZipCode { get; set; }
        public IList<string> Roles { get; set; }

        public UserModel()
        {
            Roles = new List<string>();
        }
        /*public List<DeliveryAddressModel> DeliveryAddresses { get; set; }
        public List<UserRoleModel> UserRoles { get; set; }
        public List<CustomerOrderModel> CustomerOrders { get; set; }*/
    }
}
