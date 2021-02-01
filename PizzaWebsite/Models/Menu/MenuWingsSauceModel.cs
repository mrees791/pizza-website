using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.Menu
{
    public class MenuWingsSauceModel : MenuItemModel
    {
        [Display(Name = "Description")]
        [Required]
        public string Description { get; set; }

        public MenuWingsSauceModel()
        {
        }

        /// <summary>
        /// Creates a model from the database library model.
        /// </summary>
        /// <param name="databaseModel"></param>
        public MenuWingsSauceModel(DataLibrary.Models.Menu.MenuWingsSauceModel databaseModel)
        {
            Id = databaseModel.Id;
            AvailableForPurchase = databaseModel.AvailableForPurchase;
            Description = databaseModel.Description;
            Name = databaseModel.Name;
        }

        /// <summary>
        /// Converts the model to a database library model.
        /// </summary>
        /// <returns></returns>
        public DataLibrary.Models.Menu.MenuWingsSauceModel ToDatabaseModel()
        {
            DataLibrary.Models.Menu.MenuWingsSauceModel databaseModel = new DataLibrary.Models.Menu.MenuWingsSauceModel();
            databaseModel.Id = Id;
            databaseModel.AvailableForPurchase = AvailableForPurchase;
            databaseModel.Description = Description;
            databaseModel.Name = Name;

            return databaseModel;
        }
    }
}