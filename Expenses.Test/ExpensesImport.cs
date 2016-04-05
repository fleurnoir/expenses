using System;
using System.Collections;
using System.Collections.Generic;
using Expenses.BL.Entities;
using System.Linq;
using Expenses.BL.Common;
using Expenses.Common.Utils;

namespace Expenses.Test
{
    public static class ExpensesImport
    {
        public static void Import(IList<Expense> expenses, string connectionName)
        {
            using (var db = new ExpensesContext ("name="+connectionName)) 
            using (var transaction = db.BeginTransaction())
            {
                var accounts = new Dictionary<string,Account> ();
                var currencies = new Dictionary<string,Currency> ();
                var categories = new Dictionary<string,Category> ();
                var subcategories = new Dictionary<string,Subcategory> ();
                var user = db.Users.First ();

                foreach (var expense in expenses.OrderBy(e=>e.Date)) {
                    var tuple = GetSum (expense);
                    db.Operations.Add (new Operation{
                        AccountId = GetAccount(db, expense.Account, tuple.Item2, accounts, currencies).Id,
                        Amount = tuple.Item1,
                        SubcategoryId = GetSubcategory(db, expense.Subcategory, expense.Category, subcategories, categories).Id,
                        OperationTime = expense.Date,
                        UserId = user.Id,
                        Comment = expense.Comment
                    });
                }

                transaction.Commit();
            }
        }

        private static Tuple<double, string> GetSum(Expense expense)
        {
            if (expense.Som > 0.0)
                return Tuple.Create (expense.Som, "KGS");
            if (expense.Euro > 0.0)
                return Tuple.Create (expense.Euro, "EUR");
            if (expense.Dollar > 0.0)
                return Tuple.Create (expense.Dollar, "USD");
            if (expense.Rouble > 0.0)
                return Tuple.Create (expense.Rouble, "RUB");
            throw new ArgumentException ("Expense has zero sum");
        }

        private static Account GetAccount(ExpensesContext db, string name, string currency, IDictionary<string, Account> accounts, IDictionary<string, Currency> currencies)
        {
            name = $"{name} {currency}";
            var result = accounts.SafeGet (name);
            if (result != null)
                return result;
            result = db.Accounts.FirstOrDefault (item=>item.Name == name);
            if (result == null) {
                result = db.Accounts.Add (new Account{ Name = name, CurrencyId = GetCurrency (db, currency, currencies).Id });
                db.SaveChanges ();
            }
            accounts.Add (name, result);
            return result;
        }

        private static Currency GetCurrency(ExpensesContext db, string name, IDictionary<string, Currency> currencies)
        {
            var result = currencies.SafeGet (name);
            if (result != null)
                return result;
            result = db.Currencies.FirstOrDefault (item=>item.ShortName == name);
            if (result == null) {
                result = db.Currencies.Add (new Currency{Name = name, ShortName = name});
                db.SaveChanges ();
            }
            currencies.Add (name, result);
            return result;
        }

        private static Category GetCategory(ExpensesContext db, string name, IDictionary<string, Category> categories)
        {
            var result = categories.SafeGet (name);
            if (result != null)
                return result;
            result = db.Categories.FirstOrDefault (item=>item.Name == name);
            if (result == null) {
                result = db.Categories.Add (new Category{Name = name, Type = OperationType.Expense});
                db.SaveChanges ();
            }
            categories.Add (name, result);
            return result;
        }

        private static Subcategory GetSubcategory(ExpensesContext db, string name, string category, IDictionary<string, Subcategory> subcategories, IDictionary<string, Category> categories)
        {
            var fullname = $"{name} {category}";
            var result = subcategories.SafeGet (fullname);
            if (result != null)
                return result;
            var cat = GetCategory(db, category, categories);
            result = db.Subcategories.FirstOrDefault (item=>item.Name == name && item.CategoryId == cat.Id);
            if (result == null) {
                result = db.Subcategories.Add (new Subcategory{ Name = name, CategoryId = cat.Id });
                db.SaveChanges ();
            }
            subcategories.Add (fullname, result);
            return result;
        }
    }
}

