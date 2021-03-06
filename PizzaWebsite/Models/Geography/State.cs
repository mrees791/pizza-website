using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Geography
{
    public class State
    {
        private string name;
        private string abbreviation;

        public State(string name, string abbreviation)
        {
            if (abbreviation.Length != 2)
            {
                throw new Exception($"State abbreviation must be two characters. {name} {abbreviation}");
            }
            this.name = name;
            this.abbreviation = abbreviation;
        }

        public string Name { get => name; }
        public string Abbreviation { get => abbreviation; }
    }
}