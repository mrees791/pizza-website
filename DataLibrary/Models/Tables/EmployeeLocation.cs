using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Tables
{
    [Table("EmployeeLocation")]
    public class EmployeeLocation
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int StoreId { get; set; }
    }
}
