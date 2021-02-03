using DataLibrary.Models.Menu.Pizzas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Pizzas
{
    public class PizzaToppingModel
    {
        public int Id { get; set; }
        public string ToppingHalf { get; set; }
        public string ToppingAmount { get; set; }
        public MenuPizzaToppingModel MenuPizzaTopping { get; set; }
        public int PizzaId { get; set; }

        public decimal GetPrice()
        {
            decimal total = 0.0m;

            switch (ToppingAmount)
            {
                case "Light":
                    total += MenuPizzaTopping.PriceLight;
                    break;
                case "Regular":
                    total += MenuPizzaTopping.PriceRegular;
                    break;
                case "Extra":
                    total += MenuPizzaTopping.PriceExtra;
                    break;
            }

            switch (ToppingHalf)
            {
                case "Left":
                case "Right":
                    total /= 2.0m;
                    break;
            }

            return total;
        }
    }
}
