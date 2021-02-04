using DataLibrary.BusinessLogic.Pizzas;
using DataLibrary.DataAccess;
using DataLibrary.Models.Menus.Pizzas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary.Models.Pizzas;
using Dapper;

namespace DataLibrary.BusinessLogic.Menus
{
    public static class DatabaseMenuPizzaProcessor
    {
        public static int UpdateMenuPizza(MenuPizzaModel menuPizzaModel)
        {
            int menuPizzaRowsUpdated = 0;

            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Update pizza record
                        int pizzaRecordsUpdated = DatabasePizzaProcessor.UpdatePizza(menuPizzaModel.Pizza, connection, transaction);

                        // Update menu pizza record
                        string updateMenuPizzaSql = @"update dbo.MenuPizza set CategoryName = @CategoryName, AvailableForPurchase = @AvailableForPurchase, PizzaName = @PizzaName,
                                                      Description = @Description where Id = @Id;";

                        object queryParameters = new
                        {
                            Id = menuPizzaModel.Id,
                            CategoryName = menuPizzaModel.CategoryName,
                            AvailableForPurchase = menuPizzaModel.AvailableForPurchase,
                            PizzaName = menuPizzaModel.PizzaName,
                            Description = menuPizzaModel.Description
                        };

                        menuPizzaRowsUpdated = SqlDataAccess.UpdateRecord(updateMenuPizzaSql, queryParameters, connection, transaction);

                        if (menuPizzaRowsUpdated == 0)
                        {
                            throw new Exception($"Unable to update menu pizza category. ID: {menuPizzaModel.Id}");
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return menuPizzaRowsUpdated;
        }

        public static List<MenuPizzaModel> LoadMenuPizzas()
        {
            List<MenuPizzaModel> menuPizzas = new List<MenuPizzaModel>();
            List<PizzaModel> pizzaList = DatabasePizzaProcessor.LoadPizzas();

            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                string selectMenuPizzaQuerySql = @"select Id, PizzaId, CategoryName, AvailableForPurchase, PizzaName, Description from dbo.MenuPizza;";
                List<dynamic> queryList = connection.Query<dynamic>(selectMenuPizzaQuerySql).ToList();

                foreach (var item in queryList)
                {
                    menuPizzas.Add(new MenuPizzaModel()
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

            return menuPizzas;
        }

        public static int DeleteMenuPizza(MenuPizzaModel menuPizzaModel)
        {
            int menuPizzaRowsDeleted = 0;

            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Delete menu pizza record
                        string deleteMenuPizzaSql = @"delete from dbo.MenuPizza where Id = @Id;";
                        menuPizzaRowsDeleted = SqlDataAccess.DeleteRecord(deleteMenuPizzaSql, menuPizzaModel, connection, transaction);

                        // Delete pizza record
                        int pizzaRecordsDeleted = DatabasePizzaProcessor.DeletePizza(menuPizzaModel.Pizza, connection, transaction);

                        if (pizzaRecordsDeleted == 0)
                        {
                            throw new Exception($"Unable to delete pizza record. Pizza ID: {menuPizzaModel.Pizza.Id}");
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            return menuPizzaRowsDeleted;
        }

        private static void SetDefaultMenuPizzaSettings(MenuPizzaModel menuPizza)
        {
            menuPizza.Pizza.Size = PizzaSize.Medium;
        }

        public static int AddMenuPizza(MenuPizzaModel menuPizzaModel)
        {
            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SetDefaultMenuPizzaSettings(menuPizzaModel);

                        // Add pizza record
                        int newPizzaRecordId = DatabasePizzaProcessor.AddPizza(menuPizzaModel.Pizza, connection, transaction);

                        // Add menu pizza record
                        string insertMenuPizzaSql = @"insert into dbo.MenuPizza (PizzaId, CategoryName, AvailableForPurchase, PizzaName, Description)
                           output Inserted.Id values (@PizzaId, @CategoryName, @AvailableForPurchase, @PizzaName, @Description);";

                        object queryParameters = new
                        {
                            PizzaId = menuPizzaModel.Pizza.Id,
                            CategoryName = menuPizzaModel.CategoryName,
                            AvailableForPurchase = menuPizzaModel.AvailableForPurchase,
                            PizzaName = menuPizzaModel.PizzaName,
                            Description = menuPizzaModel.Description
                        };

                        menuPizzaModel.Id = SqlDataAccess.SaveNewRecord(insertMenuPizzaSql, queryParameters, connection, transaction);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return menuPizzaModel.Id;
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
