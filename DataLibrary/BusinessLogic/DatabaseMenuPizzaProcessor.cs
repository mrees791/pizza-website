using DataLibrary.DataAccess;
using DataLibrary.Models.Menu.Pizza;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public static class DatabaseMenuPizzaProcessor
    {
        public static List<MenuPizzaCheeseModel> LoadMenuPizzaCheeses()
        {
            string sql = @"select Id, AvailableForPurchase, Name, PriceLight, PriceRegular, PriceExtra, Description, HasMenuIcon, MenuIconFile, HasPizzaBuilderImage, PizzaBuilderImageFile from dbo.MenuPizzaCheese;";

            return SqlDataAccess.LoadData<MenuPizzaCheeseModel>(sql);
        }

        public static int UpdateMenuPizzaCheese(
            int id,
            bool availableForPurchase,
            string name,
            decimal priceLight,
            decimal priceRegular,
            decimal priceExtra,
            string description,
            bool hasMenuIcon,
            string menuIconFile,
            bool hasPizzaBuilderImage,
            string pizzaBuilderImageFile)
        {
            MenuPizzaCheeseModel data = new MenuPizzaCheeseModel();

            data.Id = id;
            data.AvailableForPurchase = availableForPurchase;
            data.Name = name;
            data.PriceLight = priceLight;
            data.PriceRegular = priceRegular;
            data.PriceExtra = priceExtra;
            data.Description = description;
            data.HasMenuIcon = hasMenuIcon;
            data.MenuIconFile = menuIconFile;
            data.HasPizzaBuilderImage = hasPizzaBuilderImage;
            data.PizzaBuilderImageFile = pizzaBuilderImageFile;

            string sql = @"update dbo.MenuPizzaCheese set AvailableForPurchase = @AvailableForPurchase, Name = @Name, PriceLight = @PriceLight, PriceRegular = @PriceRegular, PriceExtra = @PriceExtra,
                           Description = @Description, HasMenuIcon = @HasMenuIcon, MenuIconFile = @MenuIconFile, HasPizzaBuilderImage = @HasPizzaBuilderImage, PizzaBuilderImageFile = @PizzaBuilderImageFile where Id = @Id;";

            return SqlDataAccess.UpdateRecord(sql, data);
        }
    }
}