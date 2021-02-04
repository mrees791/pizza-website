using DataLibrary.Models.Menus.Pizzas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Pizzas
{
    public class PizzaModel
    {
        public int Id { get; set; }
        public string Size { get; set; }
        public string SauceAmount { get; set; }
        public string CheeseAmount { get; set; }
        public MenuPizzaCrustModel MenuPizzaCrust { get; set; }
        public MenuPizzaSauceModel MenuPizzaSauce { get; set; }
        public MenuPizzaCheeseModel MenuPizzaCheese { get; set; }
        public MenuPizzaCrustFlavorModel MenuPizzaCrustFlavor { get; set; }
        public List<PizzaToppingModel> PizzaToppings { get; set; }

        public PizzaModel()
        {
            PizzaToppings = new List<PizzaToppingModel>();
        }

        public decimal GetPrice()
        {
            decimal total = 0.0m;

            switch (Size)
            {
                case PizzaSize.Small:
                    total += MenuPizzaCrust.PriceSmall;
                    break;
                case PizzaSize.Medium:
                    total += MenuPizzaCrust.PriceMedium;
                    break;
                case PizzaSize.Large:
                    total += MenuPizzaCrust.PriceLarge;
                    break;
            }

            switch (SauceAmount)
            {
                case PizzaSauceAmount.Light:
                    total += MenuPizzaSauce.PriceLight;
                    break;
                case PizzaSauceAmount.Regular:
                    total += MenuPizzaSauce.PriceRegular;
                    break;
                case PizzaSauceAmount.Extra:
                    total += MenuPizzaSauce.PriceExtra;
                    break;
            }

            switch (CheeseAmount)
            {
                case PizzaCheeseAmount.Light:
                    total += MenuPizzaCheese.PriceLight;
                    break;
                case PizzaCheeseAmount.Regular:
                    total += MenuPizzaCheese.PriceRegular;
                    break;
                case PizzaCheeseAmount.Extra:
                    total += MenuPizzaCheese.PriceExtra;
                    break;
            }

            foreach (PizzaToppingModel pizzaTopping in PizzaToppings)
            {
                total += pizzaTopping.GetPrice();
            }

            return total;
        }
    }
}
