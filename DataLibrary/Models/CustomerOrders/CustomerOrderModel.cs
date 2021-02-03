using DataLibrary.Models.Carts;
using DataLibrary.Models.Stores;
using DataLibrary.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.CustomerOrders
{
    public class CustomerOrderModel
    {
        public int Id { get; set; }
        public UserModel User { get; set; }
        public StoreLocationModel Store { get; set; }
        public CartModel Cart { get; set; }
        public decimal OrderSubtotal { get; set; }
        public decimal OrderTax { get; set; }
        public decimal OrderTotal { get; set; }
        public int OrderPhase { get; set; }
        public bool OrderCompleted { get; set; }
        public DateTime DateOfOrder { get; set; }
        public bool IsDelivery { get; set; }
        public DeliveryInfoModel DeliveryInfo { get; set; }
    }
}
