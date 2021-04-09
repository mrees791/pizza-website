using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Interfaces
{
    public interface ICartItemType
    {
        decimal CalculatePrice(PizzaDatabase pizzaDb);
        string GetName(PizzaDatabase pizzaDb);
        string GetDescriptionHtml(PizzaDatabase pizzaDb);
    }
}
