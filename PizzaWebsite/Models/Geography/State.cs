using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Geography
{
    public class State
    {
        public State() { }

        public State(string name, string abbreviation)
        {
            Name = name;
            Abbreviation = abbreviation;
        }

        public string Name { get; set; }
        public string Abbreviation { get; set; }
    }
}