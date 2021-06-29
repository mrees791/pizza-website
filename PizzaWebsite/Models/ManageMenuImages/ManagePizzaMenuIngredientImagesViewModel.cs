using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.ManageMenuImages
{
    public class ManagePizzaMenuIngredientImagesViewModel
    {
        public int Id { get; set; }
        public string MenuIconDescription { get; set; }
        public string MenuIconUrl { get; set; }
        public string PizzaBuilderImageDescription { get; set; }
        public string PizzaBuilderImageUrl { get; set; }
    }
}