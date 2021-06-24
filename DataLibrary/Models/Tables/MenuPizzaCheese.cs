using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace DataLibrary.Models.Tables
{
    [Table("MenuPizzaCheese")]
    public class MenuPizzaCheese : MenuCategoryRecord
    {
        [Key]
        public int Id { get; set; }

        public int SortOrder { get; set; }
        public bool AvailableForPurchase { get; set; }
        public string Name { get; set; }
        public decimal PriceLight { get; set; }
        public decimal PriceRegular { get; set; }
        public decimal PriceExtra { get; set; }
        public string Description { get; set; }

        public override dynamic GetId()
        {
            return Id;
        }

        public override MenuCategory GetMenuCategoryType()
        {
            return MenuCategory.PizzaCheese;
        }

        internal override async Task<dynamic> InsertAsync(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            int? id = await pizzaDb.Connection.InsertAsync(this, transaction);
            Id = id.Value;
            return Id;
        }

        internal override bool InsertRequiresTransaction()
        {
            return false;
        }

        internal override async Task MapEntityAsync(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            await Task.FromResult(0);
        }

        internal override async Task<int> UpdateAsync(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            return await pizzaDb.Connection.UpdateAsync(this, transaction);
        }

        internal override bool UpdateRequiresTransaction()
        {
            return false;
        }
    }
}