using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Tables
{
    [Table("CartItem")]
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public int CartId { get; set; }
        public decimal PricePerItem { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public string ProductCategory { get; set; }
    }
}