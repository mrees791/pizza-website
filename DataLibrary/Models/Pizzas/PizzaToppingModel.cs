using DataLibrary.Models.Menus;
using DataLibrary.Models.Menus.Pizzas;
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
                case PizzaToppingAmount.Light:
                    total += MenuPizzaTopping.PriceLight;
                    break;
                case PizzaToppingAmount.Regular:
                    total += MenuPizzaTopping.PriceRegular;
                    break;
                case PizzaToppingAmount.Extra:
                    total += MenuPizzaTopping.PriceExtra;
                    break;
            }

            switch (ToppingHalf)
            {
                case PizzaToppingHalf.Left:
                case PizzaToppingHalf.Right:
                    total /= 2.0m;
                    break;
            }

            return total;
        }
    }
}
