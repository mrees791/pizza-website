using Dapper;
using DataLibrary.DataAccess;
using DataLibrary.Models.Menu.Pizzas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public static class DatabaseMenuPizzaProcessor
    {
        public static List<MenuPizzaCategoryModel> LoadMenuPizzaCategories()
        {
            throw new NotImplementedException();
        }

        public static int UpdateMenuPizzaCategory(MenuPizzaCategoryModel menuPizzaCategoryModel)
        {
            throw new NotImplementedException();
        }

        public static int DeleteMenuPizzaCategory(MenuPizzaCategoryModel menuPizzaCategoryModel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new menu pizza category.
        /// </summary>
        /// <param name="menuPizzaCategoryModel"></param>
        /// <returns>ID of newly created menu pizza category.</returns>
        public static int AddMenuPizzaCategory(MenuPizzaCategoryModel menuPizzaCategoryModel)
        {
            // Add pizza record
            /*int pizzaRowsAdded = DatabasePizzaProcessor.AddPizza(menuPizzaCategoryModel.Pizza);

            if (pizzaRowsAdded == 0)
            {
                throw new Exception($"Unable to add pizza for menu pizza category.");
            }

            // Add menu pizza category record
            string insertSql = @"insert into dbo.MenuPizzaCategory (PizzaId, CategoryName, AvailableForPurchase, PizzaName, Description)
                           output Inserted.Id values (@PizzaId, @CategoryName, @AvailableForPurchase, @PizzaName, @Description);";

            try
            {
                using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
                {
                    menuPizzaCategoryModel.Id = SqlDataAccess.SaveNewRecord(insertSql,
                        new
                        {
                            PizzaId = menuPizzaCategoryModel.Pizza.Id,
                            CategoryName = menuPizzaCategoryModel.PizzaCategoryName,
                            AvailableForPurchase = menuPizzaCategoryModel.AvailableForPurchase,
                            PizzaName = menuPizzaCategoryModel.PizzaName,
                            Description = menuPizzaCategoryModel.Description
                        },
                        connection);
                }
            }
            catch (Exception ex)
            {
                // Rollback changes
                DatabasePizzaProcessor.DeletePizza(menuPizzaCategoryModel.Pizza);
                throw;
            }

            return menuPizzaCategoryModel.Id;*/
            throw new NotImplementedException();
        }

        public static List<MenuPizzaToppingModel> LoadMenuPizzaToppings()
        {
            string sql = @"select Id, AvailableForPurchase, Name, PriceLight, PriceRegular, PriceExtra,
                           PizzaToppingType, Description, HasMenuIcon, MenuIconFile, HasPizzaBuilderImage, PizzaBuilderImageFile from dbo.MenuPizzaTopping;";

            return SqlDataAccess.LoadData<MenuPizzaToppingModel>(sql);
        }

        public static int UpdateMenuPizzaTopping(MenuPizzaToppingModel menuPizzaToppingModel)
        {
            string sql = @"update dbo.MenuPizzaTopping set AvailableForPurchase = @AvailableForPurchase, Name = @Name, PriceLight = @PriceLight,
                           PriceRegular = @PriceRegular, PriceExtra = @PriceExtra, PizzaToppingType = @PizzaToppingType, Description = @Description,
                           HasMenuIcon = @HasMenuIcon, MenuIconFile = @MenuIconFile, HasPizzaBuilderImage = @HasPizzaBuilderImage, PizzaBuilderImageFile = @PizzaBuilderImageFile where Id = @Id;";

            return SqlDataAccess.UpdateRecord(sql, menuPizzaToppingModel);
        }

        public static int AddMenuPizzaTopping(MenuPizzaToppingModel menuPizzaToppingModel)
        {
            string sql = @"insert into dbo.MenuPizzaTopping (AvailableForPurchase, Name, PriceLight, PriceRegular, PriceExtra,
                           PizzaToppingType, Description, HasMenuIcon, MenuIconFile, HasPizzaBuilderImage, PizzaBuilderImageFile)
                           output Inserted.Id values (@AvailableForPurchase, @Name, @PriceLight, @PriceRegular, @PriceExtra,
                           @PizzaToppingType, @Description, @HasMenuIcon, @MenuIconFile, @HasPizzaBuilderImage, @PizzaBuilderImageFile);";

            return SqlDataAccess.SaveNewRecord(sql, menuPizzaToppingModel);
        }

        public static List<MenuPizzaSauceModel> LoadMenuPizzaSauces()
        {
            string sql = @"select Id, AvailableForPurchase, Name, PriceLight, PriceRegular, PriceExtra,
                           Description, HasMenuIcon, MenuIconFile, HasPizzaBuilderImage, PizzaBuilderImageFile from dbo.MenuPizzaSauce;";

            return SqlDataAccess.LoadData<MenuPizzaSauceModel>(sql);
        }

        public static int UpdateMenuPizzaSauce(MenuPizzaSauceModel menuPizzaSauceModel)
        {
            string sql = @"update dbo.MenuPizzaSauce set AvailableForPurchase = @AvailableForPurchase, Name = @Name, PriceLight = @PriceLight,
                           PriceRegular = @PriceRegular, PriceExtra = @PriceExtra, Description = @Description, HasMenuIcon = @HasMenuIcon,
                           MenuIconFile = @MenuIconFile, HasPizzaBuilderImage = @HasPizzaBuilderImage, PizzaBuilderImageFile = @PizzaBuilderImageFile where Id = @Id;";

            return SqlDataAccess.UpdateRecord(sql, menuPizzaSauceModel);
        }

        public static int AddMenuPizzaSauce(MenuPizzaSauceModel menuPizzaSauceModel)
        {
            string sql = @"insert into dbo.MenuPizzaSauce (AvailableForPurchase, Name, PriceLight, PriceRegular, PriceExtra,
                           Description, HasMenuIcon, MenuIconFile, HasPizzaBuilderImage, PizzaBuilderImageFile)
                           output Inserted.Id values (@AvailableForPurchase, @Name, @PriceLight, @PriceRegular, @PriceExtra,
                           @Description, @HasMenuIcon, @MenuIconFile, @HasPizzaBuilderImage, @PizzaBuilderImageFile);";

            return SqlDataAccess.SaveNewRecord(sql, menuPizzaSauceModel);
        }

        public static List<MenuPizzaCrustFlavorModel> LoadMenuPizzaCrustFlavors()
        {
            string sql = @"select Id, AvailableForPurchase, Name, Description, HasMenuIcon, MenuIconFile, HasPizzaBuilderImage, PizzaBuilderImageFile from dbo.MenuPizzaCrustFlavor;";

            return SqlDataAccess.LoadData<MenuPizzaCrustFlavorModel>(sql);
        }

        public static int UpdateMenuPizzaCrustFlavor(MenuPizzaCrustFlavorModel menuPizzaCrustFlavorModel)
        {
            string sql = @"update dbo.MenuPizzaCrustFlavor set AvailableForPurchase = @AvailableForPurchase, Name = @Name, HasMenuIcon = @HasMenuIcon,
                           MenuIconFile = @MenuIconFile, HasPizzaBuilderImage = @HasPizzaBuilderImage, PizzaBuilderImageFile = @PizzaBuilderImageFile, Description = @Description where Id = @Id;";

            return SqlDataAccess.UpdateRecord(sql, menuPizzaCrustFlavorModel);
        }

        public static int AddMenuPizzaCrustFlavor(MenuPizzaCrustFlavorModel menuPizzaCrustFlavorModel)
        {
            string sql = @"insert into dbo.MenuPizzaCrustFlavor (AvailableForPurchase, Name, HasMenuIcon, MenuIconFile, HasPizzaBuilderImage, PizzaBuilderImageFile, Description)
                           output Inserted.Id values (@AvailableForPurchase, @Name, @HasMenuIcon, @MenuIconFile, @HasPizzaBuilderImage, @PizzaBuilderImageFile, @Description);";

            return SqlDataAccess.SaveNewRecord(sql, menuPizzaCrustFlavorModel);
        }

        public static List<MenuPizzaCrustModel> LoadMenuPizzaCrusts()
        {
            string sql = @"select Id, AvailableForPurchase, Name, PriceSmall, PriceMedium, PriceLarge, Description, HasMenuIcon, MenuIconFile, HasPizzaBuilderImage, PizzaBuilderImageFile from dbo.MenuPizzaCrust;";

            return SqlDataAccess.LoadData<MenuPizzaCrustModel>(sql);
        }

        public static int UpdateMenuPizzaCrust(MenuPizzaCrustModel menuPizzaCrustModel)
        {
            string sql = @"update dbo.MenuPizzaCrust set AvailableForPurchase = @AvailableForPurchase, Name = @Name, PriceSmall = @PriceSmall, PriceMedium = @PriceMedium, PriceLarge = @PriceLarge,
                           Description = @Description, HasMenuIcon = @HasMenuIcon, MenuIconFile = @MenuIconFile, HasPizzaBuilderImage = @HasPizzaBuilderImage, PizzaBuilderImageFile = @PizzaBuilderImageFile where Id = @Id;";

            return SqlDataAccess.UpdateRecord(sql, menuPizzaCrustModel);
        }

        public static int AddMenuPizzaCrust(MenuPizzaCrustModel menuPizzaCrustModel)
        {
            string sql = @"insert into dbo.MenuPizzaCrust (AvailableForPurchase, Name, PriceSmall, PriceMedium, PriceLarge, Description, HasMenuIcon, MenuIconFile, HasPizzaBuilderImage, PizzaBuilderImageFile)
                           output Inserted.Id values (@AvailableForPurchase, @Name, @PriceSmall, @PriceMedium, @PriceLarge, @Description, @HasMenuIcon, @MenuIconFile, @HasPizzaBuilderImage, @PizzaBuilderImageFile);";

            return SqlDataAccess.SaveNewRecord(sql, menuPizzaCrustModel);
        }

        public static List<MenuPizzaCheeseModel> LoadMenuPizzaCheeses()
        {
            string sql = @"select Id, AvailableForPurchase, Name, PriceLight, PriceRegular, PriceExtra, Description, HasMenuIcon, MenuIconFile, HasPizzaBuilderImage, PizzaBuilderImageFile from dbo.MenuPizzaCheese;";

            return SqlDataAccess.LoadData<MenuPizzaCheeseModel>(sql);
        }

        public static int UpdateMenuPizzaCheese(MenuPizzaCheeseModel menuPizzaCheeseModel)
        {
            string sql = @"update dbo.MenuPizzaCheese set AvailableForPurchase = @AvailableForPurchase, Name = @Name, PriceLight = @PriceLight, PriceRegular = @PriceRegular, PriceExtra = @PriceExtra,
                           Description = @Description, HasMenuIcon = @HasMenuIcon, MenuIconFile = @MenuIconFile, HasPizzaBuilderImage = @HasPizzaBuilderImage, PizzaBuilderImageFile = @PizzaBuilderImageFile where Id = @Id;";

            return SqlDataAccess.UpdateRecord(sql, menuPizzaCheeseModel);
        }

        public static int AddMenuPizzaCheese(MenuPizzaCheeseModel menuPizzaCheeseModel)
        {
            string sql = @"insert into dbo.MenuPizzaCheese (AvailableForPurchase, Name, PriceLight, PriceRegular, PriceExtra, Description, HasMenuIcon, MenuIconFile, HasPizzaBuilderImage, PizzaBuilderImageFile)
                           output Inserted.Id values (@AvailableForPurchase, @Name, @PriceLight, @PriceRegular, @PriceExtra, @Description, @HasMenuIcon, @MenuIconFile, @HasPizzaBuilderImage, @PizzaBuilderImageFile);";

            return SqlDataAccess.SaveNewRecord(sql, menuPizzaCheeseModel);
        }
    }
}