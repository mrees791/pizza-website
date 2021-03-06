﻿using Dapper;
using DataLibrary.Models.JoinLists.BaseClasses;
using DataLibrary.Models.QueryFilters;
using DataLibrary.Models.Sql;
using DataLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models.JoinLists
{
    public class EmployeeLocationOnStoreLocationJoinList : JoinListBase<EmployeeLocation, StoreLocation>
    {
        public async Task LoadListByEmployeeIdAsync(string employeeId, PizzaDatabase pizzaDb)
        {
            string whereClause = "WHERE l.EmployeeId = @EmployeeId";
            object parameters = new
            {
                EmployeeId = employeeId
            };
            await LoadListAsync(whereClause, parameters, false, "s.Name", SortOrder.Ascending, pizzaDb);
        }

        // Paged lists
        public async Task LoadPagedListByEmployeeIdAsync(string employeeId, StoreLocationFilter searchFilter, int pageNumber, int rowsPerPage, PizzaDatabase pizzaDb)
        {
            List<WhereClauseItem> whereClauseList = new List<WhereClauseItem>()
            {
                new WhereClauseItem("l.EmployeeId", "EmployeeId", employeeId, ComparisonType.Equals),
                new WhereClauseItem("s.Name", "StoreName", searchFilter.Name, ComparisonType.Like),
                new WhereClauseItem("s.PhoneNumber", "PhoneNumber", searchFilter.PhoneNumber, ComparisonType.Like)
            };
            object parameters = new
            {
                EmployeeId = employeeId,
                StoreName = searchFilter.Name,
                PhoneNumber = searchFilter.PhoneNumber,
                CurrentOffset = pagedListServices.GetOffset(pageNumber, rowsPerPage),
                RowsPerPage = rowsPerPage
            };
            string whereClause = sqlServices.CreateWhereClause(whereClauseList);
            string offsetClause = sqlServices.CreateOffsetClause();
            await LoadListAsync(whereClause, parameters, false, "s.Name", SortOrder.Ascending, pizzaDb, offsetClause: offsetClause);
        }

        public async Task<int> GetNumberOfResultsByEmployeeIdAsync(string employeeId, StoreLocationFilter searchFilter, int rowsPerPage, PizzaDatabase pizzaDb)
        {
            List<WhereClauseItem> whereClauseList = new List<WhereClauseItem>()
            {
                new WhereClauseItem("l.EmployeeId", "EmployeeId", employeeId, ComparisonType.Equals),
                new WhereClauseItem("s.Name", "StoreName", searchFilter.Name, ComparisonType.Like),
                new WhereClauseItem("s.PhoneNumber", "PhoneNumber", searchFilter.PhoneNumber, ComparisonType.Like)
            };
            string whereClause = sqlServices.CreateWhereClause(whereClauseList);
            string sql = $@"SELECT COUNT(l.Id)
                            From EmployeeLocation l
                            INNER JOIN StoreLocation s
                            ON l.StoreId = s.Id
                            {whereClause}";
            object parameters = new
            {
                EmployeeId = employeeId,
                StoreName = searchFilter.Name,
                PhoneNumber = searchFilter.PhoneNumber
            };
            return await pizzaDb.Connection.ExecuteScalarAsync<int>(sql, parameters);
        }

        public async Task<int> GetNumberOfPagesByEmployeeIdAsync(string employeeId, StoreLocationFilter searchFilter, int rowsPerPage, PizzaDatabase pizzaDb)
        {
            int resultCount = await GetNumberOfResultsByEmployeeIdAsync(employeeId, searchFilter, rowsPerPage, pizzaDb);
            return pagedListServices.GetNumberOfPages(rowsPerPage, resultCount);
        }

        protected override string GetSqlJoinQuery(bool onlySelectFirst)
        {
            return $@"SELECT {sqlServices.CreateTopClause(onlySelectFirst)}
                      l.Id, l.EmployeeId, l.StoreId,
                      s.Id, s.Name, s.StreetAddress, s.City, s.State, s.ZipCode, s.PhoneNumber, s.isActiveLocation
                      FROM EmployeeLocation l
                      INNER JOIN StoreLocation s
                      ON l.StoreId = s.Id";
        }
    }
}