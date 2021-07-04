using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.ManageMenuImages
{
    public class ManagePizzaMenuToppingImagesViewModel
    {
        public string ViewTitle { get; set; }
        public int Id { get; set; }
        public UploadMenuImageFormViewModel MenuIconVm { get; set; }
        public UploadMenuImageFormViewModel PizzaBuilderImageVm { get; set; }
        public UploadMenuImageFormViewModel PizzaBuilderLeftImageVm { get; set; }
        public UploadMenuImageFormViewModel PizzaBuilderRightImageVm { get; set; }
        public UploadMenuImageFormViewModel PizzaBuilderExtraImageVm { get; set; }
        public UploadMenuImageFormViewModel PizzaBuilderExtraLeftImageVm { get; set; }
        public UploadMenuImageFormViewModel PizzaBuilderExtraRightImageVm { get; set; }
    }
}