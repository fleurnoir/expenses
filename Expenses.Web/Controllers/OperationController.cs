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
using PagedList;
using Expenses.Common.Utils;

namespace Expenses.Web.Controllers
{
    public class OperationController : ExpensesControllerBase<Operation, OperationViewData>
    {
        public ActionResult Index(string dateFrom, string dateTo, long? categoryId, long? subcategoryId, long? accountId, int? page)
        {
            ViewBag.CategoryId = new SelectList (
                new[] {new Category{Id = 0, Name=Strings.All}}.Concat(Service.GetCategories()), 
                nameof (Category.Id), nameof (Category.Name), 
                categoryId ?? 0);
            if (categoryId == 0)
                categoryId = null;
            if (subcategoryId == 0)
                subcategoryId = null;
            ViewBag.SubcategoryId = new SelectList (
                GetFilterSubcategoriesCore(categoryId),
                nameof (Subcategory.Id), nameof (Subcategory.Name), subcategoryId ?? 0);
            if (accountId == 0)
                accountId = null;
            ViewBag.AccountId = new SelectList (
                new[] {new Account{Id = 0, Name=Strings.All}}.Concat(Service.GetAccounts()),
                nameof (Account.Id), nameof (Account.Name), accountId ?? 0);
            var items = FillUpViewItems (
                Service.GetOperations (ToStartDate(dateFrom), ToEndDate(dateTo), subcategoryId, categoryId, accountId)
                .Select (item => new OperationViewData (item))).ToList ();
            var stats = GetStatistics (ToStartDate(dateFrom), ToEndDate(dateTo), subcategoryId, categoryId, accountId);
            var income = stats.Where (s => s.Type == OperationType.Income).ToList();
            if (income.Count > 0)
                ViewBag.Income = income;
            var expense = stats.Where (s => s.Type == OperationType.Expense).ToList();
            if (expense.Count > 0)
                ViewBag.Expense = expense;
            
            return View(items.ToPagedList(page ?? 1, 20));
        }

        private static string DateFormat = "yyyy-MM-dd";

        private static DateTime? ToStartDate(string dateTime) {
            return dateTime?.ToDateTime (DateFormat)?.Date;
        }

        private static DateTime? ToEndDate(string dateTime) {
            return dateTime?.ToDateTime (DateFormat)?.Date.AddDays(1);
        }

        private void SaveDefaults(OperationViewData item) {
            if (item.CategoryId > 0) {
                Service.SetDefaultId<Category> (item.CategoryId);
                if (item.SubcategoryId > 0)
                    Service.SetDefaultSubcategoryId (item.CategoryId, item.SubcategoryId);
            }
            if (item.AccountId > 0)
                Service.SetDefaultId<Account> (item.AccountId);
        }

        protected override void OnSaved (OperationViewData item)
        {
            base.OnSaved (item);
            SaveDefaults (item);
        }

        private IList<StatsItemViewData> GetStatistics(DateTime? startTime, DateTime? endTime, long? subcategoryId, long? categoryId, long? accountId)
        {
            var currencies = Service.GetCurrencies ().ToDictionary(c=>c.Id);
            return Service.GetStatistics (startTime, endTime, subcategoryId, categoryId, accountId)
                .Select (item => new StatsItemViewData (item)
                    { CurrencyName = currencies [item.CurrencyId].ShortName }).ToList();
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
            var storage = new CachedEntityStorage (Service);
            return items.Select (item => FillUpViewItem(item, storage));
        }

        private OperationViewData FillUpViewItem (OperationViewData item, IEntityStorage storage)
        {
            var subcategory = storage.Get<Subcategory> (item.SubcategoryId);
            var account = storage.Get<Account> (item.AccountId);
            var currency = storage.Get<Currency> (account.CurrencyId);
            var category = storage.Get<Category> (subcategory.CategoryId);

            item.AccountName = account.Name;
            item.SubcategoryName = subcategory.Name;
            item.CategoryId = subcategory.CategoryId;
            item.CategoryName = category.Name;
            item.Type = category.Type;
            item.CurrencyId = currency.Id;
            item.CurrencyName = currency.ShortName;
            return item;
        }

        protected override OperationViewData FillUpViewItem (OperationViewData item)
        {
            return FillUpViewItem(item, new EntityStorage(Service));
        }

        private long ChooseDefaultValue<TEntity>(long selected, Func<long?> getRecentlySelected, ICollection<TEntity> available) 
            where TEntity : Entity 
        {
            if (selected > 0 && available.Any (item => item.Id == selected))
                return selected;
            var recentlySelected = getRecentlySelected () ?? 0;
            if (recentlySelected > 0 && available.Any (item => item.Id == recentlySelected))
                return recentlySelected;
            return available.FirstOrDefault ()?.Id ?? 0;
        }

        protected override void PopulateSelectLists (OperationViewData entity)
        {
            if (entity == null)
                entity = new OperationViewData ();

            var accounts = Service.GetAccounts ();
            var categories = Service.GetCategories ();
            if (entity.SubcategoryId > 0)
                entity.CategoryId = Service.GetSubcategory (entity.SubcategoryId)?.CategoryId ?? 0;

            long categoryId = ChooseDefaultValue(entity.CategoryId, ()=>Service.GetDefaultId<Category>(), categories);

            var subcategories = Service.GetSubcategories (categoryId > 0 ? (long?)categoryId : null);

            long subcategoryId = ChooseDefaultValue(entity.SubcategoryId, ()=>Service.GetDefaultSubcategoryId(categoryId), subcategories);
                    
            long accountId = ChooseDefaultValue(entity.AccountId, ()=>Service.GetDefaultId<Account>(), accounts);

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
                Type = OperationType.Expense
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