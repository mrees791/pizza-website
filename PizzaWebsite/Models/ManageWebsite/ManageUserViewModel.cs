using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.ManageWebsite
{
    public class ManageUserViewModel
    {
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Banned")]
        public bool IsBanned { get; set; }

        /// <summary>
        /// Replaces periods in the user's ID with (dot).
        /// This is needed by {id} in the MapRoute method of the RouteConfig class.
        /// The {id} section of the route won't work with periods so we use (dot) as a placeholder.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetUrlSafeId()
        {
            return Id.Replace(".", "(dot)");
        }
    }
}