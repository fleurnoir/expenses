using System;
using Expenses.BL.Entities;
using System.Web.Mvc;
using System.Linq;
using Expenses.Web.Models;
using Expenses.Common.Utils;
using System.Collections.Generic;

namespace Expenses.Web.Controllers
{
    public class ExpenseItemController : ExpensesController<ExpenseItem, ExpenseItemViewData>
    {
        protected override IEnumerable<ExpenseItemViewData> FillUpViewItems (IEnumerable<ExpenseItemViewData> items)
        {
            var categories = Service.GetCategories (null).ToDictionary(item=>item.Id, item=>item.Name);
            return items.Select (
                item => { 
                    item.ExpenseCategoryName = categories.SafeGet (item.ExpenseCategoryId);
                    return item;
                });
        }

        protected override ExpenseItemViewData FillUpViewItem (ExpenseItemViewData item)
        {
            item.ExpenseCategoryName = Service.GetCategory (item.ExpenseCategoryId).Name;
            return item;
        }

        protected override void PopulateSelectLists (ExpenseItemViewData item)
        {
            var categories = Service.GetCategories (null);
            ViewBag.ExpenseCategoryId = new SelectList (categories, nameof(ExpenseCategory.Id), nameof(ExpenseCategory.Name), item?.ExpenseCategoryId ?? categories.FirstOrDefault()?.Id);
        }
    }
}

