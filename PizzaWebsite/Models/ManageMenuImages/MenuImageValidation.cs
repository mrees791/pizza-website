using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.ManageMenuImages
{
    /// <summary>
    /// Sets the required dimensions for menu images.
    /// Controllers for managing menu images will validate the uploaded image with this model.
    /// </summary>
    public class MenuImageValidation
    {
        public int RequiredWidth { get; set; }
        public int RequiredHeight { get; set; }
    }
}