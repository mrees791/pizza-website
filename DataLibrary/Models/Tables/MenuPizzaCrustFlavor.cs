using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Tables
{
    [Table("MenuPizzaCrustFlavor")]
    public class MenuPizzaCrustFlavor
    {
        [Key]
        public int Id { get; set; }
        public int SortOrder { get; set; }
        public bool AvailableForPurchase { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasMenuIcon { get; set; }
        public string MenuIconFile { get; set; }
        public bool HasPizzaBuilderImage { get; set; }
        public string PizzaBuilderImageFile { get; set; }
    }
}
