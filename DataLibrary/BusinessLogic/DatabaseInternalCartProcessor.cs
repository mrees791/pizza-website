using DataLibrary.DataAccess;
using DataLibrary.Models.Menu;
using DataLibrary.Models.Pizzas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    internal static class DatabaseInternalCartProcessor
    {
        internal static int AddPizzaToCart(int cartId, PizzaModel pizza, int quantity, IDbConnection connection, IDbTransaction transaction)
        {
            string insertSql = @"insert into dbo.CartPizza (CartId, PizzaId, PricePerItem, Quantity) output Inserted.Id values(@CartId, @PizzaId, @PricePerItem, @Quantity);";

            // Save pizza record
            pizza.Id = DatabasePizzaProcessor.AddPizza(pizza, connection, transaction);

            object queryParameters = new
            {
                CartId = cartId,
                PizzaId = pizza.Id,
                PricePerItem = pizza.GetPrice(),
                Quantity = quantity
            };

            // Save cart pizza record
            return SqlDataAccess.SaveNewRecord(insertSql, queryParameters, connection, transaction);
        }

        internal static int UpdateCartPizzaQuantity(int cartPizzaId, int quantity, IDbConnection connection, IDbTransaction transaction)
        {
            string updateSql = @"update dbo.CartPizza set Quantity = @Quantity where Id = @Id;";

            object queryParameters = new
            {
                Id = cartPizzaId,
                Quantity = quantity
            };

            return SqlDataAccess.UpdateRecord(updateSql, queryParameters, connection, transaction);
        }

        internal static int UpdateCartPizza(int cartPizzaId, PizzaModel updatedPizza, int quantity, IDbConnection connection, IDbTransaction transaction)
        {
            string updateSql = @"update dbo.CartPizza set PricePerItem = @PricePerItem, Quantity = @Quantity where Id = @Id;";
            int pizzaRowsAffected = DatabasePizzaProcessor.UpdatePizza(updatedPizza, connection, transaction);

            object queryParameters = new
            {
                Id = cartPizzaId,
                PricePerItem = updatedPizza.GetPrice(),
                Quantity = quantity
            };

            return SqlDataAccess.UpdateRecord(updateSql, queryParameters, connection, transaction);
        }

        private static decimal GetDrinkPrice(string size, MenuDrinkModel menuDrink)
        {
            switch (size)
            {
                case DrinkSizes.Size20Oz:
                    return menuDrink.Price20Oz;
                case DrinkSizes.Size2Liter:
                    return menuDrink.Price2Liter;
                case DrinkSizes.Size2Pack12Oz:
                    return menuDrink.Price2Pack12Oz;
                case DrinkSizes.Size6Pack12Oz:
                    return menuDrink.Price6Pack12Oz;
                default:
                    throw new Exception($"Drink size not found: {size}");
            }
        }

        internal static int AddDrinkToCart(int cartId, MenuDrinkModel menuDrink, string size, int quantity, IDbConnection connection, IDbTransaction transaction)
        {
            string insertSql = @"insert into dbo.CartDrink (CartId, MenuDrinkId, PricePerItem, Size, Quantity)
                                 output Inserted.Id values(@CartId, @MenuDrinkId, @PricePerItem, @Size, @Quantity);";

            object queryParameters = new
            {
                CartId = cartId,
                MenuDrinkId = menuDrink.Id,
                PricePerItem = GetDrinkPrice(size, menuDrink),
                Size = size,
                Quantity = quantity
            };

            return SqlDataAccess.SaveNewRecord(insertSql, queryParameters, connection, transaction);
        }

        internal static int UpdateCartDrinkQuantity(int cartDrinkId, int quantity, IDbConnection connection, IDbTransaction transaction)
        {
            string updateSql = @"update dbo.CartDrink set Quantity = @Quantity where Id = @Id;";

            object queryParameters = new
            {
                Id = cartDrinkId,
                Quantity = quantity
            };

            return SqlDataAccess.UpdateRecord(updateSql, queryParameters, connection, transaction);
        }

        internal static int UpdateCartDrink(int cartDrinkId, MenuDrinkModel menuDrink, string size, int quantity, IDbConnection connection, IDbTransaction transaction)
        {
            string updateSql = @"update dbo.CartDrink set MenuDrinkId = @MenuDrinkId, PricePerItem = @PricePerItem, Size = @Size, Quantity = @Quantity where Id = @Id;";

            object queryParameters = new
            {
                Id = cartDrinkId,
                MenuDrinkId = menuDrink.Id,
                PricePerItem = GetDrinkPrice(size, menuDrink),
                Size = size,
                Quantity = quantity
            };

            return SqlDataAccess.UpdateRecord(updateSql, queryParameters, connection, transaction);
        }

        private static decimal GetWingsPrice(int pieceAmount, MenuWingsModel menuWings)
        {
            switch (pieceAmount)
            {
                case 6:
                    return menuWings.Price6Piece;
                case 12:
                    return menuWings.Price12Piece;
                case 18:
                    return menuWings.Price18Piece;
                default:
                    throw new Exception($"Price not found for wing amount: {pieceAmount}");
            }
        }

        internal static int AddWingsToCart(int cartId, MenuWingsModel menuWings, MenuWingsSauceModel menuWingsSauce, int pieceAmount, int quantity, IDbConnection connection, IDbTransaction transaction)
        {
            string insertSql = @"insert into dbo.CartWings (CartId, MenuWingsId, MenuWingsSauceId, PieceAmount, PricePerItem, Quantity)
                                 output Inserted.Id values(@CartId, @MenuWingsId, @MenuWingsSauceId, @PieceAmount, @PricePerItem, @Quantity);";

            object queryParameters = new
            {
                CartId = cartId,
                MenuWingsId = menuWings.Id,
                MenuWingsSauceId = menuWingsSauce.Id,
                PieceAmount = pieceAmount,
                PricePerItem = GetWingsPrice(pieceAmount, menuWings),
                Quantity = quantity
            };

            return SqlDataAccess.SaveNewRecord(insertSql, queryParameters, connection, transaction);
        }

        internal static int UpdateCartWingsQuantity(int cartWingsId, int quantity, IDbConnection connection, IDbTransaction transaction)
        {
            string updateSql = @"update dbo.CartWings set Quantity = @Quantity where Id = @Id;";

            object queryParameters = new
            {
                Id = cartWingsId,
                Quantity = quantity
            };

            return SqlDataAccess.UpdateRecord(updateSql, queryParameters, connection, transaction);
        }

        internal static int UpdateCartWings(int cartWingsId, MenuWingsModel menuWings, MenuWingsSauceModel menuWingsSauce, int pieceAmount, int quantity, IDbConnection connection, IDbTransaction transaction)
        {
            string updateSql = @"update dbo.CartWings set MenuWingsId = @MenuWingsId, MenuWingsSauceId = @MenuWingsSauceId, PieceAmount = @PieceAmount, PricePerItem = @PricePerItem, Quantity = @Quantity where Id = @Id;";

            object queryParameters = new
            {
                Id = cartWingsId,
                MenuWingsId = menuWings.Id,
                MenuWingsSauceId = menuWingsSauce.Id,
                PieceAmount = pieceAmount,
                PricePerItem = GetWingsPrice(pieceAmount, menuWings),
                Quantity = quantity
            };

            return SqlDataAccess.UpdateRecord(updateSql, queryParameters, connection, transaction);
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

        internal static int UpdateCartDipQuantity(int cartDipId, int quantity, IDbConnection connection, IDbTransaction transaction)
        {
            string updateSql = @"update dbo.CartDip set MenuDipId = @MenuDipId, PricePerItem = @PricePerItem, Quantity = @Quantity where Id = @Id;";

            object queryParameters = new
            {
                Id = cartDipId,
                Quantity = quantity
            };

            return SqlDataAccess.UpdateRecord(updateSql, queryParameters, connection, transaction);
        }

        internal static int UpdateCartDip(int cartDipId, MenuDipModel menuDip, int quantity, IDbConnection connection, IDbTransaction transaction)
        {
            string updateSql = @"update dbo.CartDip set MenuDipId = @MenuDipId, PricePerItem = @PricePerItem, Quantity = @Quantity where Id = @Id;";

            object queryParameters = new
            {
                Id = cartDipId,
                MenuDipId = menuDip.Id,
                PricePerItem = menuDip.Price,
                Quantity = quantity
            };

            return SqlDataAccess.UpdateRecord(updateSql, queryParameters, connection, transaction);
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

            return SqlDataAccess.SaveNewRecord(insertSql, queryParameters, connection, transaction);
        }

        internal static int UpdateCartDessertQuantity(int cartDessertId, MenuDessertModel menuDessert, int quantity, IDbConnection connection, IDbTransaction transaction)
        {
            string updateSql = @"update dbo.CartDessert set Quantity = @Quantity where Id = @Id;";

            object queryParameters = new
            {
                Id = cartDessertId,
                Quantity = quantity
            };

            return SqlDataAccess.UpdateRecord(updateSql, queryParameters, connection, transaction);
        }

        internal static int UpdateCartDessert(int cartDessertId, MenuDessertModel menuDessert, int quantity, IDbConnection connection, IDbTransaction transaction)
        {
            string updateSql = @"update dbo.CartDessert set MenuDessertId = @MenuDessertId, PricePerItem = @PricePerItem, Quantity = @Quantity where Id = @Id;";

            object queryParameters = new
            {
                Id = cartDessertId,
                MenuDessertId = menuDessert.Id,
                PricePerItem = menuDessert.Price,
                Quantity = quantity
            };

            return SqlDataAccess.UpdateRecord(updateSql, queryParameters, connection, transaction);
        }

        internal static int AddPastaToCart(int cartId, MenuPastaModel menuPasta, int quantity, IDbConnection connection, IDbTransaction transaction)
        {
            string insertSql = @"insert into dbo.CartPasta (CartId, MenuPastaId, PricePerItem, Quantity)
                                 output Inserted.Id values(@CartId, @MenuPastaId, @PricePerItem, @Quantity);";

            object queryParameters = new
            {
                CartId = cartId,
                MenuPastaId = menuPasta.Id,
                PricePerItem = menuPasta.Price,
                Quantity = quantity
            };

            return SqlDataAccess.SaveNewRecord(insertSql, queryParameters, connection, transaction);
        }

        internal static int UpdateCartPastaQuantity(int cartDessertId, int quantity, IDbConnection connection, IDbTransaction transaction)
        {
            string updateSql = @"update dbo.CartPasta set Quantity = @Quantity where Id = @Id;";

            object queryParameters = new
            {
                Id = cartDessertId,
                Quantity = quantity
            };

            return SqlDataAccess.UpdateRecord(updateSql, queryParameters, connection, transaction);
        }

        internal static int UpdateCartPasta(int cartDessertId, MenuPastaModel menuPasta, int quantity, IDbConnection connection, IDbTransaction transaction)
        {
            string updateSql = @"update dbo.CartPasta set MenuPastaId = @MenuPastaId, PricePerItem = @PricePerItem, Quantity = @Quantity where Id = @Id;";

            object queryParameters = new
            {
                Id = cartDessertId,
                MenuPastaId = menuPasta.Id,
                PricePerItem = menuPasta.Price,
                Quantity = quantity
            };

            return SqlDataAccess.UpdateRecord(updateSql, queryParameters, connection, transaction);
        }

        internal static int AddSideToCart(int cartId, MenuSideModel menuSide, int quantity, IDbConnection connection, IDbTransaction transaction)
        {
            string insertSql = @"insert into dbo.CartSide (CartId, MenuSideId, PricePerItem, Quantity)
                                 output Inserted.Id values(@CartId, MenuSideId, PricePerItem, Quantity);";

            object queryParameters = new
            {
                CartId = cartId,
                MenuSideId = menuSide.Id,
                PricePerItem = menuSide.Price,
                Quantity = quantity
            };

            return SqlDataAccess.SaveNewRecord(insertSql, queryParameters, connection, transaction);
        }

        internal static int UpdateCartSideQuantity(int cartSideId, int quantity, IDbConnection connection, IDbTransaction transaction)
        {
            string updateSql = @"update dbo.CartSide set Quantity = @Quantity where Id = @Id;";

            object queryParameters = new
            {
                Id = cartSideId,
                Quantity = quantity
            };

            return SqlDataAccess.UpdateRecord(updateSql, queryParameters, connection, transaction);
        }

        internal static int UpdateCartSide(int cartSideId, MenuSideModel menuSide, int quantity, IDbConnection connection, IDbTransaction transaction)
        {
            string updateSql = @"update dbo.CartSide set MenuSideId = @MenuSideId, PricePerItem = @PricePerItem, Quantity = @Quantity where Id = @Id;";

            object queryParameters = new
            {
                Id = cartSideId,
                MenuSideId = menuSide.Id,
                PricePerItem = menuSide.Price,
                Quantity = quantity
            };

            return SqlDataAccess.UpdateRecord(updateSql, queryParameters, connection, transaction);
        }

        internal static int AddSauceToCart(int cartId, MenuSauceModel menuSauce, int quantity, IDbConnection connection, IDbTransaction transaction)
        {
            string insertSql = @"insert into dbo.CartSauce (CartId, MenuSauceId, PricePerItem, Quantity)
                                 output Inserted.Id values(@CartId, @MenuSauceId, @PricePerItem, @Quantity);";

            object queryParameters = new
            {
                CartId = cartId,
                MenuSauceId = menuSauce.Id,
                PricePerItem = menuSauce.Price,
                Quantity = quantity
            };

            return SqlDataAccess.SaveNewRecord(insertSql, queryParameters, connection, transaction);
        }

        internal static int UpdateCartSauceQuantity()
        {
            throw new NotImplementedException();
        }

        internal static int UpdateCartSauce()
        {
            throw new NotImplementedException();
        }

        internal static int AddNewCart(IDbConnection connection, IDbTransaction transaction)
        {
            string insertSql = @"insert into dbo.Cart output Inserted.Id default values;";

            // Save new cart record
            return SqlDataAccess.SaveNewRecord(insertSql, new { }, connection, transaction);
        }
    }
}
