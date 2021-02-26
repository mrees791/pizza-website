using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Databases
{
    public class DatabaseContext
    {
        public static DummyDatabase Create()
        {
            return new DummyDatabase();
        }
    }
}