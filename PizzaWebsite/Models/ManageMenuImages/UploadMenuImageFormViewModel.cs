using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.ManageMenuImages
{
    public class UploadMenuImageFormViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        // HTML element ID's
        public string DropAreaId { get; set; }
        public string ErrorMessageId { get; set; }
        public string ImageId { get; set; }
    }
}