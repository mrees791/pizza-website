using DataLibrary.DataAccess;
using DataLibrary.Models.Carts;
using DataLibrary.Models.Menu;
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
    public static class DatabaseCartProcessor
    {
        public static int AddPizzaToCart(int cartId, PizzaModel pizzaModel, int quantity)
        {
            int cartPizzaId = 0;

            using (IDbConnection connection = new SqlConnection(SqlDataAccess.GetConnectiongString()))
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        cartPizzaId = AddPizzaToCart(cartId, pizzaModel, quantity, connection, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return cartPizzaId;
        }

        internal static int AddPizzaToCart(int cartId, PizzaModel pizzaModel, int quantity, IDbConnection connection, IDbTransaction transaction)
        {
            string insertSql = @"insert into dbo.CartPizza (CartId, PizzaId, PricePerItem, Quantity) output Inserted.Id values(@CartId, @PizzaId, @PricePerItem, @Quantity);";

            // Save pizza record
            pizzaModel.Id = DatabasePizzaProcessor.AddPizza(pizzaModel, connection, transaction);

            object queryParameters = new 
            {
                CartId = cartId,
                PizzaId = pizzaModel.Id,
                PricePerItem = pizzaModel.GetPrice(),
                Quantity = quantity
            };

            // Save cart pizza record
            return SqlDataAccess.SaveNewRecord(insertSql, queryParameters, connection, transaction);
        }

        internal static int AddDrinkToCart(int cartId, MenuDrinkModel menuDrink, string size, int quantity, IDbConnection connection, IDbTransaction transaction)
        {
            string insertSql = @"insert into dbo.CartDrink (CartId, MenuDrinkId, PricePerItem, Size, Quantity)
                                 output Inserted.Id values(@CartId, @MenuDrinkId, @PricePerItem, @Size, @Quantity);";

            decimal pricePerItem = 0.0m;

            switch (size)
            {
                case DrinkSizes.Size20Oz:
                    pricePerItem = menuDrink.Price20Oz;
                    break;
                case DrinkSizes.Size2Liter:
                    pricePerItem = menuDrink.Price2Liter;
                    break;
                case DrinkSizes.Size2Pack12Oz:
                    pricePerItem = menuDrink.Price2Pack12Oz;
                    break;
                case DrinkSizes.Size6Pack12Oz:
                    pricePerItem = menuDrink.Price6Pack12Oz;
                    break;
                default:
                    throw new Exception($"Drink size not found: {size}");
            }

            object queryParameters = new
            {
                CartId = cartId,
                MenuDrinkId = menuDrink.Id,
                PricePerItem = pricePerItem,
                Size = size,
                Quantity = quantity
            };

            // Save cart drink record
            return SqlDataAccess.SaveNewRecord(insertSql, queryParameters, connection, transaction);
        }

        internal static int AddWingsToCart(int cartId, MenuWingsModel menuWings, MenuWingsSauceModel menuWingsSauce, int pieceAmount, int quantity, IDbConnection connection, IDbTransaction transaction)
        {
            string insertSql = @"insert into dbo.CartWings (CartId, MenuWingsId, MenuWingsSauceId, PieceAmount, PricePerItem, Quantity)
                                 output Inserted.Id values(@CartId, @MenuWingsId, @MenuWingsSauceId, @PieceAmount, @PricePerItem, @Quantity);";

            decimal pricePerItem = 0.0m;

            switch(pieceAmount)
            {
                case 6:
                    pricePerItem = menuWings.Price6Piece;
                    break;
                case 12:
                    pricePerItem = menuWings.Price12Piece;
                    break;
                case 18:
                    pricePerItem = menuWings.Price18Piece;
                    break;
                default:
                    throw new Exception($"Price not found for wing amount: {pieceAmount}");
            }

            object queryParameters = new
            {
                CartId = cartId,
                MenuWingsId = menuWings.Id,
                MenuWingsSauceId = menuWingsSauce.Id,
                PieceAmount = pieceAmount,
                PricePerItem = pricePerItem,
                Quantity = quantity
            };

            // Save cart wings record
            return SqlDataAccess.SaveNewRecord(insertSql, queryParameters, connection, transaction);
        }

        internal static int AddDipToCart(int cartId, MenuDipModel menuDip, int quantity, IDbConnection connection, IDbTransaction transaction)
        {
            string insertSql = @"insert into dbo.CartDip (CartId, MenuDipId, PricePerItem, Quantity)
                                 output Inserted.Id values(@CartId, @MenuDipId, @PricePerItem, @Quantity);";

            object queryParameters = new
            {
                CartId = cartId,
                MenuDipId = menuDip.Id,
                PricePerItem = menuDip.Price,
                Quantity = quantity
            };

            // Save cart dip record
            return SqlDataAccess.SaveNewRecord(insertSql, queryParameters, connection, transaction);
        }

        internal static int AddDessertToCart(int cartId, MenuDessertModel menuDessert, int quantity, IDbConnection connection, IDbTransaction transaction)
        {
            string insertSql = @"insert into dbo.CartDessert (CartId, MenuDessertId, PricePerItem, Quantity)
                                 output Inserted.Id values(@CartId, @MenuDessertId, @PricePerItem, @Quantity);";

            object queryParameters = new
            {
                CartId = cartId,
                MenuDessertId = menuDessert.Id,
                PricePerItem = menuDessert.Price,
                Quantity = quantity
            };

            // Save cart dessert record
            return SqlDataAccess.SaveNewRecord(insertSql, queryParameters, connection, transaction);
        }

        internal static int AddPastaToCart(int cartId, MenuPastaModel menuPasta, int quantity, IDbConnection connection, IDbTransaction transaction)
        {
            string insertSql = @"insert into dbo.CartPasta (CartIt, MenuPastaId, PricePerItem, Quantity)
                                 output Inserted.Id values(@CartIt, @MenuPastaId, @PricePerItem, @Quantity);";

            object queryParameters = new
            {
                CartId = cartId,
                MenuPastaId = menuPasta.Id,
                PricePerItem = menuPasta.Price,
                Quantity = quantity
            };

            // Save cart pasta record
            return SqlDataAccess.SaveNewRecord(insertSql, queryParameters, connection, transaction);
        }

        internal static int AddNewCart(IDbConnection connection, IDbTransaction transaction)
        {
            string insertSql = @"insert into dbo.Cart output Inserted.Id default values;";

            // Save new cart record
            return SqlDataAccess.SaveNewRecord(insertSql, new { }, connection, transaction);
        }
    }
}
