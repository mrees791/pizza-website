﻿using Dapper;
using DataLibrary.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.Tables
{
    [Table("SiteRole")]
    public class SiteRole : IRecord
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public dynamic GetId()
        {
            return Id;
        }

        public void Insert(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            Id = pizzaDb.Connection.Insert(this, transaction).Value;
        }

        public bool InsertRequiresTransaction()
        {
            return false;
        }

        public void MapEntity(PizzaDatabase pizzaDb)
        {
        }

        public int Update(PizzaDatabase pizzaDb, IDbTransaction transaction = null)
        {
            return pizzaDb.Connection.Update(this, transaction);
        }

        public bool UpdateRequiresTransaction()
        {
            return false;
        }
    }
}