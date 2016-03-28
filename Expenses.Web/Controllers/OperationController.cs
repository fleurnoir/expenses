﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Expenses.BL.Entities;
using Expenses.Web.Models;
using Expenses.BL.Service;
using Expenses.Web.Common;
using Expenses.Common.Service;
using PagedList;
using Expenses.Common.Utils;

namespace Expenses.Web.Controllers
{
    public class OperationController : ExpensesControllerBase<Operation, OperationViewData>
    {
        public ActionResult Index(string dateFrom, string dateTo, long? categoryId, long? subcategoryId, int? page)
        {
            var format = "yyyy-MM-dd";
            ViewBag.CategoryId = new SelectList (new[] {new Category{Id = 0, Name=Strings.All}}.Concat(Service.GetCategories()), nameof (Category.Id), nameof (Category.Name), categoryId ?? 0);
            if (categoryId == 0)
                categoryId = null;
            if (subcategoryId == 0)
                subcategoryId = null;
            ViewBag.SubcategoryId = new SelectList (
                GetFilterSubcategoriesCore(categoryId),
                nameof (Subcategory.Id), nameof (Subcategory.Name), subcategoryId ?? 0);
            var items = FillUpViewItems (
                Service.GetOperations (dateFrom.ToDateTime (format), dateTo.ToDateTime (format), subcategoryId, categoryId)
                .Select (item => new OperationViewData (item))).ToList ();
            var stats = GetStatistics (dateFrom.ToDateTime (format), dateTo.ToDateTime (format), subcategoryId, categoryId);
            var income = stats.Where (s => s.Type == CategoryType.Income).ToList();
            if (income.Count > 0)
                ViewBag.Income = income;
            var expense = stats.Where (s => s.Type == CategoryType.Expense).ToList();
            if (expense.Count > 0)
                ViewBag.Expense = expense;
            
            return View(items.ToPagedList(page ?? 1, 20));
        }

        private IList<StatsItemViewData> GetStatistics(DateTime? startTime, DateTime? endTime, long? subcategoryId, long? categoryId)
        {
            var currencies = Service.GetCurrencies ().ToDictionary(c=>c.Id);
            return Service.GetStatistics (startTime, endTime, subcategoryId, categoryId)
                .Select (item => new StatsItemViewData (item)
                    { CurrencyName = currencies [item.CurrencyId].ShortName }).ToList();
        }

        private static long GetKey(long currencyId, CategoryType type)
        {
            switch (type) {
            case CategoryType.Expense:
                return currencyId * 2;
            case CategoryType.Income:
                return currencyId * 2 + 1;
            default:
                throw new ArgumentException ("Unknown category type");
            }
        }


        private IEnumerable<Subcategory> GetFilterSubcategoriesCore(long? categoryId)
        {
            return categoryId == null 
                ? new []{ new Subcategory{ Id = 0, Name = Strings.ChooseCategory } }
                : new []{ new Subcategory{ Id = 0, Name = Strings.All } }
                .Concat (Service.GetSubcategories (categoryId));
        }

        [HttpPost]
        public JsonResult GetFilterSubcategories(long? categoryId)
        {
            if (categoryId == 0)
                categoryId = null;
            return Json (GetFilterSubcategoriesCore(categoryId).Select(item=>new{id=item.Id, name=item.Name}));
        }

        protected override IEnumerable<OperationViewData> FillUpViewItems (IEnumerable<OperationViewData> items)
        {
            var subcategories = Service.GetSubcategories (null).ToDictionary(item=>item.Id);
            var accounts = Service.GetAccounts ().ToDictionary(item=>item.Id);
            var categories = Service.GetCategories ().ToDictionary(item=>item.Id);
            var currencies = Service.GetCurrencies().ToDictionary(item=>item.Id); 

            return items.Select (item => {
                var subcategory = subcategories[item.SubcategoryId];
                var account = accounts[item.AccountId];
                var currency = currencies[account.CurrencyId];
                var category = categories[subcategory.CategoryId];

                item.AccountName = account.Name;
                item.SubcategoryName = subcategory.Name;
                item.CategoryId = subcategory.CategoryId;
                item.CategoryName = category.Name;
                item.Type = category.Type;
                item.CurrencyId = currency.Id;
                item.CurrencyName = currency.ShortName;
                return item;
            });
        }

        protected override OperationViewData FillUpViewItem (OperationViewData item)
        {
            var subcategory = Service.GetSubcategory (item.SubcategoryId);
            var account = Service.GetAccount (item.AccountId);
            var currency = Service.GetCurrency (account.CurrencyId);
            var category = Service.GetCategory (subcategory.CategoryId);

            item.AccountName = account.Name;
            item.SubcategoryName = subcategory.Name;
            item.CategoryId = subcategory.CategoryId;
            item.CategoryName = category.Name;
            item.Type = category.Type;
            item.CurrencyId = currency.Id;
            item.CurrencyName = currency.ShortName;
            return item;
        }

        protected override void PopulateSelectLists (OperationViewData entity)
        {
            if (entity == null)
                entity = new OperationViewData ();

            var accounts = Service.GetAccounts ();
            var categories = Service.GetCategories ();

            long categoryId;
            if (entity.SubcategoryId <= 0 && entity.CategoryId <= 0) {
                categoryId = (categories.FirstOrDefault (item => item.Type == CategoryType.Expense)?.Id).GetValueOrDefault();
            } else if (entity.CategoryId <= 0 && entity.SubcategoryId > 0) {
                categoryId = Service.GetSubcategory (entity.SubcategoryId).CategoryId;
            } else
                categoryId = entity.CategoryId;

            var subcategories = Service.GetSubcategories (categoryId > 0 ? (long?)categoryId : null);

            long subcategoryId = entity.SubcategoryId;
            if (subcategoryId <= 0 || !subcategories.Any (item => item.Id == subcategoryId))
                subcategoryId = (subcategories.FirstOrDefault ()?.Id).GetValueOrDefault();
                    
            long accountId = entity.AccountId > 0 ? entity.AccountId : (accounts.FirstOrDefault ()?.Id).GetValueOrDefault();

            ViewBag.AccountId = new SelectList (accounts, nameof (Account.Id), nameof (Account.Name), accountId);
            ViewBag.SubcategoryId = new SelectList (subcategories, nameof(Subcategory.Id), nameof(Subcategory.Name), subcategoryId);
            ViewBag.CategoryId = new SelectList (categories, nameof (Category.Id), nameof (Category.Name), categoryId);

            if (accountId > 0)
                ViewBag.CurrencyName = Service.GetCurrency (accounts.First (account => account.Id == accountId).CurrencyId).ShortName;
            else
                ViewBag.CurrencyName = null;
        }

        [HttpPost]
        public JsonResult GetSubcategories(long categoryId)
        {
            var result = Json (Service.GetSubcategories (categoryId).Select (item => new {id = item.Id, name = item.Name}).ToList ());
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
            var result = Service.AddCategory (new Category {
                Name = categoryName,
                Type = CategoryType.Expense
            });
            return Json (new { id = result.Id, name = result.Name });
        }

        [HttpPost]
        public JsonResult AddSubcategory(string subcategoryName, long categoryId)
        {
            var result = Service.AddSubcategory (new Subcategory {
                Name = subcategoryName,
                CategoryId = categoryId
            });
            return Json (new { id = result.Id, name = result.Name });
        }
    }
}