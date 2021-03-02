using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Tables
{
    [Table("Cart")]
    public class Cart
    {
        [Key]
        public int Id { get; set; }
    }
}
