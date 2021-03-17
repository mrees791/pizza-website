using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Tables
{
    [Table("MenuPizza")]
    public class MenuPizza
    {
        [Key]
        public int Id { get; set; }
        // todo: Finish
    }
}
