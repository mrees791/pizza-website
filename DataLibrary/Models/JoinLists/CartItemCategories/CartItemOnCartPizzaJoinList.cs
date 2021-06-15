using System.Threading.Tasks;
using DataLibrary.Models.JoinLists.BaseClasses;
using DataLibrary.Models.Tables;

namespace DataLibrary.Models.JoinLists.CartItemCategories
{
    public class CartItemOnCartPizzaJoinList : CartItemJoinListBase<CartPizza>
    {
        public async Task LoadFirstOrDefaultAsync(int cartItemId, PizzaDatabase pizzaDb)
        {
            string whereClause = "WHERE c.Id = @Id";
            object parameters = new
            {
                Id = cartItemId
            };
            await LoadListAsync(whereClause, parameters, true, "c.Id", SortOrder.Ascending, pizzaDb);
        }

        public override async Task LoadListByCartIdAsync(int cartId, PizzaDatabase pizzaDb)
        {
            string whereClause = "WHERE c.CartId = @CartId";
            object parameters = new
            {
                CartId = cartId
            };
            await LoadListAsync(whereClause, parameters, false, "c.Id", SortOrder.Ascending, pizzaDb);
        }

        protected override string GetSqlJoinQuery(bool onlySelectFirst)
        {
            return $@"SELECT {SqlServices.CreateTopClause(onlySelectFirst)}
                      c.Id, c.CartId, c.UserId, c.Price, c.PricePerItem, c.Quantity, c.ProductCategory, c.Quantity,
                      p.CartItemId, p.CheeseAmount, p.MenuPizzaCheeseId, p.MenuPizzaCrustFlavorId, p.MenuPizzaCrustId, p.MenuPizzaSauceId, p.SauceAmount, p.size
	                  FROM CartItem c
	                  INNER JOIN CartPizza p
                      ON c.Id = p.CartItemId";
        }
    }
}