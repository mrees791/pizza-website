using DataLibrary.Models;

namespace PizzaWebsite.Models.Databases
{
    public class DatabaseContext
    {
        public static PizzaDatabase Create()
        {
            return new PizzaDatabase();
        }
    }
}