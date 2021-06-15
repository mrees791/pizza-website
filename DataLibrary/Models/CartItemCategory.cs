using System.Threading.Tasks;
using Dapper;

namespace DataLibrary.Models
{
    public abstract class CartItemCategory : Record
    {
        [Key]
        public int CartItemId { get; set; }

        public abstract void SetCartItemId(int cartItemId);
        public abstract Task<decimal> CalculateItemPriceAsync(PizzaDatabase pizzaDb);
    }
}