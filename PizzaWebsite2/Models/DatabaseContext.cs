using PizzaWebsite2.Models.Databases;
using PizzaWebsite2.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite2.Models
{
    public class DatabaseContext
    {
        public static DummyDatabase Create()
        {
            return new DummyDatabase();
        }
    }
}