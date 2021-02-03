using DataLibrary.Models.Menu.Pizzas;
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
                case "Small":
                    total += MenuPizzaCrust.PriceSmall;
                    break;
                case "Medium":
                    total += MenuPizzaCrust.PriceMedium;
                    break;
                case "Large":
                    total += MenuPizzaCrust.PriceLarge;
                    break;
            }

            switch (SauceAmount)
            {
                case "Light":
                    total += MenuPizzaSauce.PriceLight;
                    break;
                case "Regular":
                    total += MenuPizzaSauce.PriceRegular;
                    break;
                case "Extra":
                    total += MenuPizzaSauce.PriceExtra;
                    break;
            }

            switch (CheeseAmount)
            {
                case "Light":
                    total += MenuPizzaCheese.PriceLight;
                    break;
                case "Regular":
                    total += MenuPizzaCheese.PriceRegular;
                    break;
                case "Extra":
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
