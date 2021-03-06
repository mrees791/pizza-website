﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.ManageUsers
{
    public class ManageUserViewModel
    {
        [Display(Name = "Id")]
        public string Id { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Banned")]
        public bool IsBanned { get; set; }
        public string UrlSafeId { get; set; }
    }
}