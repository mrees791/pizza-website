﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaWebsite.Models.ManageWebsite
{
    public class EmployeeRosterItemViewModel
    {
        public string EmployeeId { get; set; }
        public string UserId { get; set; }
        public bool IsManager { get; set; }
    }
}