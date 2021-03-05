using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Tables
{
    [Table("Employee")]
    public class Employee
    {
        [Key]
        public string Id { get; set; }
        public int UserId { get; set; }
        public bool CurrentlyEmployed { get; set; }
    }
}
