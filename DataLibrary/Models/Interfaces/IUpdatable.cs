﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Interfaces
{
    public interface IUpdatable
    {
        int Update(PizzaDatabase pizzaDb, IDbTransaction transaction = null);
        bool UpdateRequiresTransaction();
    }
}
