using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Expenses.BL.Entities;
using Expenses.Web.Models;
using Expenses.BL.Service;
using Expenses.Web.Common;
using Expenses.Common.Service;


namespace Expenses.Web.Controllers
{
    public class OperationController : ExpensesController<Operation, OperationViewData>
    {
        protected override IEnumerable<OperationViewData> FillUpViewItems (IEnumerable<OperationViewData> items)
        {
            var expenseItems = Service.GetExpenseItems (null).ToDictionary(item=>item.Id);
            var accounts = Service.GetAccounts ().ToDictionary(item=>item.Id);
            var categories = Service.GetCategories ().ToDictionary(item=>item.Id);
            var currencies = Service.GetCurrencies().ToDictionary(item=>item.Id); 

            return items.Select (item => {
                var expenseItem = expenseItems[item.ExpenseItemId];
                var account = accounts[item.AccountId];

                item.AccountName = account.Name;
                item.ExpenseItemName = expenseItem.Name;
                item.ExpenseCategoryId = expenseItem.ExpenseCategoryId;
                item.ExpenseCategoryName = categories [expenseItem.ExpenseCategoryId].Name;
                item.CurrencyName = currencies [account.CurrencyId].ShortName;
                return item;
            });
        }

        protected override OperationViewData FillUpViewItem (OperationViewData item)
        {
            var expenseItem = Service.GetExpenseItem (item.ExpenseItemId);
            var account = Service.GetAccount (item.AccountId);

            item.AccountName = account.Name;
            item.ExpenseItemName = expenseItem.Name;
            item.ExpenseCategoryId = expenseItem.ExpenseCategoryId;
            item.ExpenseCategoryName = Service.GetCategory(expenseItem.ExpenseCategoryId).Name;
            item.CurrencyName = Service.GetCurrency(account.CurrencyId).ShortName;
            return item;
        }

        protected override void PopulateSelectLists (OperationViewData entity)
        {
            if (entity == null)
                entity = new OperationViewData ();

            var accounts = Service.GetAccounts ();
            var categories = Service.GetCategories ();

            long categoryId;
            if (entity.ExpenseItemId <= 0 && entity.ExpenseCategoryId <= 0) {
                categoryId = (categories.FirstOrDefault (item => item.Type == ExpenseCategoryType.Expense)?.Id).GetValueOrDefault();
            } else if (entity.ExpenseCategoryId <= 0 && entity.ExpenseItemId > 0) {
                categoryId = Service.GetExpenseItem (entity.ExpenseItemId).ExpenseCategoryId;
            } else
                categoryId = entity.ExpenseCategoryId;

            var expenseItems = Service.GetExpenseItems (categoryId > 0 ? (long?)categoryId : null);

            long expenseItemId = entity.ExpenseItemId;
            if (expenseItemId <= 0 || !expenseItems.Any (item => item.Id == expenseItemId))
                expenseItemId = (expenseItems.FirstOrDefault ()?.Id).GetValueOrDefault();
                    
            long accountId = entity.AccountId > 0 ? entity.AccountId : (accounts.FirstOrDefault ()?.Id).GetValueOrDefault();

            ViewBag.AccountId = new SelectList (accounts, nameof (Account.Id), nameof (Account.Name), accountId);
            ViewBag.ExpenseItemId = new SelectList (expenseItems, nameof(ExpenseItem.Id), nameof(ExpenseItem.Name), expenseItemId);
            ViewBag.ExpenseCategoryId = new SelectList (categories, nameof (ExpenseCategory.Id), nameof (ExpenseCategory.Name), categoryId);

            if (accountId > 0)
                ViewBag.CurrencyName = Service.GetCurrency (accounts.First (account => account.Id == accountId).CurrencyId).ShortName;
            else
                ViewBag.CurrencyName = null;
        }

        [HttpPost]
        public JsonResult GetExpenseItems(long categoryId)
        {
            var result = Json (Service.GetExpenseItems (categoryId).Select (item => new {id = item.Id, name = item.Name}).ToList ());
            return result;
        }

        [HttpPost]
        public JsonResult GetCurrencyName(long accountId)
        {
            return Json (new { currencyName = Service.GetAccount (accountId)?.GetCurrency (Service).ShortName });
        }

        [HttpPost]
        public JsonResult AddCategory(string categoryName)
        {
            var result = Service.AddCategory (new ExpenseCategory {
                Name = categoryName,
                Type = ExpenseCategoryType.Expense
            });
            return Json (new { id = result.Id, name = result.Name });
        }

        [HttpPost]
        public JsonResult AddExpenseItem(string itemName, long categoryId)
        {
            var result = Service.AddExpenseItem (new ExpenseItem {
                Name = itemName,
                ExpenseCategoryId = categoryId
            });
            return Json (new { id = result.Id, name = result.Name });
        }
    }
}