using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTest2
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Program program = new Program();
            await program.Start();
        }

        public async Task Start()
        {
            using (var pizzaDb = new PizzaDatabase())
            {
                //IEnumerable<string> roleNames = await pizzaDb.GetRolesAsync(2);
                //int rowsAffected = await pizzaDb.RemoveFromRoleAsync(1, "MyRole1");
                //int rowsAffected = await pizzaDb.RemoveClaimAsync(1, "TypeA", "ValueA");
                int rowsAffected = await pizzaDb.RemoveLoginAsync(1, "Google", "MyKeyA");
            }
        }
    }
}