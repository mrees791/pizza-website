using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Tables
{
    [Table("CartPizzaTopping")]
    public class CartPizzaTopping
    {
        [Key]
        public int Id { get; set; }
        public int CartItemId { get; set; }
        public string ToppingHalf { get; set; }
        public string ToppingAmount { get; set; }
        public int MenuPizzaToppingTypeId { get; set; }
    }
}
