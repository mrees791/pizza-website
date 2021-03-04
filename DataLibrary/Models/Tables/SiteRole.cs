using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Tables
{
    [Table("SiteRole")]
    public class SiteRole
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
