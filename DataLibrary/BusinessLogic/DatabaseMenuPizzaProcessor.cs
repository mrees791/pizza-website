using Dapper;
using DataLibrary.DataAccess;
using DataLibrary.Models.Menu.Pizzas;
using DataLibrary.Models.Pizzas;
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
        public static int UpdateMenuPizzaCategory(MenuPizzaCategoryModel menuPizzaCategoryModel)
        {
            throw new NotImplementedException();
        }

        public static List<MenuPizzaCategoryModel> LoadMenuPizzaCategories()
        {
            List<MenuPizzaCategoryModel> menuPizzaCategories = new List<MenuPizzaCategoryModel>();
            List<PizzaModel> pizzaList = DatabasePizzaProcessor.LoadPizzas();

            string sql = @"select Id, PizzaId, CategoryName, AvailableForPurchase, PizzaName, Description from dbo.MenuPizzaCategory;";

            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                List<dynamic> queryList = connection.Query<dynamic>(sql).ToList();

                foreach (var item in queryList)
                {
                    menuPizzaCategories.Add(new MenuPizzaCategoryModel()
                    {
                        Id = item.Id,
                        AvailableForPurchase = item.AvailableForPurchase,
                        Description = item.Description,
                        CategoryName = item.CategoryName,
                        PizzaName = item.PizzaName,
                        Pizza = pizzaList.Where(p => p.Id == item.PizzaId).First()
                    });
                }
            }

            return menuPizzaCategories;
        }

        public static int DeleteMenuPizzaCategory(MenuPizzaCategoryModel menuPizzaCategoryModel)
        {
            int menuPizzaCategorieRowsDeleted = 0;
            string deleteSql = @"delete from dbo.Pizza where Id = @Id;";

            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Delete pizza record
                        int pizzaRecordsDeleted = DatabasePizzaProcessor.DeletePizza(menuPizzaCategoryModel.Pizza, connection, transaction);

                        if (pizzaRecordsDeleted == 0)
                        {
                            throw new Exception($"Unable to delete pizza record. Pizza ID: {menuPizzaCategoryModel.Pizza.Id}");
                        }

                        // Delete menu pizza category record
                        menuPizzaCategorieRowsDeleted = SqlDataAccess.DeleteRecord(deleteSql, menuPizzaCategoryModel, connection, transaction);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            return menuPizzaCategorieRowsDeleted;
        }

        public static int AddMenuPizzaCategory(MenuPizzaCategoryModel menuPizzaCategoryModel)
        {
            string insertSql = @"insert into dbo.MenuPizzaCategory (PizzaId, CategoryName, AvailableForPurchase, PizzaName, Description)
                           output Inserted.Id values (@PizzaId, @CategoryName, @AvailableForPurchase, @PizzaName, @Description);";

            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Add pizza record
                        DatabasePizzaProcessor.AddPizza(menuPizzaCategoryModel.Pizza, connection, transaction);

                        // Add menu pizza category record
                        menuPizzaCategoryModel.Id = SqlDataAccess.SaveNewRecord(insertSql,
                            new
                            {
                                PizzaId = menuPizzaCategoryModel.Pizza.Id,
                                CategoryName = menuPizzaCategoryModel.CategoryName,
                                AvailableForPurchase = menuPizzaCategoryModel.AvailableForPurchase,
                                PizzaName = menuPizzaCategoryModel.PizzaName,
                                Description = menuPizzaCategoryModel.Description
                            },
                            connection, transaction);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return menuPizzaCategoryModel.Id;
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