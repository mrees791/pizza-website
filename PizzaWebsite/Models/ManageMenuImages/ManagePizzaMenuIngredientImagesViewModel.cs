using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace PizzaWebsite.Models.ManageMenuImages
{
    public class ManagePizzaMenuIngredientImagesViewModel
    {
        public int Id { get; set; }
        public UploadMenuImageFormViewModel MenuIconVm { get; set; }
        public UploadMenuImageFormViewModel PizzaBuilderImageVm { get; set; }
    }
}