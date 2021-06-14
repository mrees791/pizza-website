using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Geography
{
    public class State
    {
        private string _name;
        private string _abbreviation;

        public State(string name, string abbreviation)
        {
            _name = name;
            _abbreviation = abbreviation;
        }

        public string Name { get => _name; }
        public string Abbreviation { get => _abbreviation; }
    }
}