using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MongoDB.Driver.Builders;
using Punch.Models;
using Punch.Utils;


namespace Punch.Data
{

    public class ExpenseDataManager
    {
        public static IEnumerable<ExpenseModel> GetExpenses(CategoryModel category)
        {
            var query = Query.EQ("Category._id", category.Id);
            var items = ModelCollection<ExpenseModel>.GetCollection(query);
            //return items.Count > 10 ? items.GetRange(0, 10) : items;
            return items;
        }

        public static ExpenseModel GetExpense(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            var expense = ModelCollection<ExpenseModel>.GetItem(id);
            return expense;
        }


        public static void DeleteItem(string id)
        {
            if (string.IsNullOrEmpty(id))
                return;

            ModelCollection<ExpenseModel>.DeleteItem(id);
        }

        public static void ToggleItem(string id)
        {
            var expense = ModelCollection<ExpenseModel>.GetItem(id);
            expense.IsCommon = !expense.IsCommon;
            ModelCollection<ExpenseModel>.UpdateItem(expense);
        }
            
        public static IEnumerable<ExpenseModel> GetExpenses(string user)
        {
            var query = Query.EQ("Owner", user);
            var items = ModelCollection<ExpenseModel>.GetCollection(query);
            return items.OrderByDescending(item => item.Date);
        }


        public static IEnumerable<ExpenseModel> GetExpenses(string user, DateTime from)
        {
            var query = Query.And(
                  Query.GTE("Date", from),
                  Query.EQ("Owner", user));
            var items = ModelCollection<ExpenseModel>.GetCollection(query);
            return items.OrderByDescending(item => item.Date);
        }

        public static void InsertItem(ExpenseModel expense)
        {
            ModelCollection<ExpenseModel>.InsertItem(expense);
        }

        public static void Insert(IEnumerable<ExpenseModel> expenses)
        {
            if (expenses == null)
                throw new ArgumentNullException("expenses");

            var filters = CategoryDataManager.GetFilters();
            foreach (var expense in expenses)
            {
                expense.Category = MapCategory(expense.Description, filters);
                InsertItem(expense); 
            }
        }

        public static bool UpdateItem(ExpenseModel expense)
        {
            try
            {
                ModelCollection<ExpenseModel>.UpdateItem(expense);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static CategoryModel MapCategory(string description, Dictionary<string, CategoryModel> filters)
        {
            foreach (string key in filters.Keys)
            {
                if (description.ContainsCaseInsensitive(key))
                    return filters[key];
            }
            return null;
        }
        
        public static decimal GetSum(string user)
        {
            var expenses = GetExpenses(user).ToList();

            return (decimal)expenses
                                 .Where(m => m.IsCommon)
                                 .Sum(m => m.Amount);
        }
    }
}
