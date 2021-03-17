using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Tables
{
    [Table("MenuPizzaTopping")]
    public class MenuPizzaTopping
    {
        [Key]
        public int Id { get; set; }
        public int MenuPizzaId { get; set; }
        public string ToppingHalf { get; set; }
        public string ToppingAmount { get; set; }
        public int MenuPizzaToppingTypeId { get; set; }
    }
}