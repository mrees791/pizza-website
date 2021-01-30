using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Menu
{
    public class MenuWingsModel : MenuItemWithIconModel
    {
        public decimal Price6Piece { get; set; }
        public decimal Price12Piece { get; set; }
        public decimal Price18Piece { get; set; }
        public string Description { get; set; }

        public MenuWingsModel(int id, bool availableForPurchase, string name, bool hasMenuIcon, string menuIconFile,
            decimal price6Piece, decimal price12Piece, decimal price18Piece, string description) :
            base(id, availableForPurchase, name, hasMenuIcon, menuIconFile)
        {
            Price6Piece = price6Piece;
            Price12Piece = price12Piece;
            Price18Piece = price18Piece;
            Description = description;
        }
    }
}
