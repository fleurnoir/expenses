using System;
using Expenses.BL.Entities;
using System.Web.Mvc;
using System.Linq;
using Expenses.Web.Models;
using Expenses.Common.Utils;
using System.Collections.Generic;

namespace Expenses.Web.Controllers
{
    public class ExpenseCategoryController : ExpensesController<ExpenseCategory, ExpenseCategoryViewData>
    {
        private static IDictionary<ExpenseCategoryType, string> m_categoryNames = new Dictionary<ExpenseCategoryType, string>
        {
            {ExpenseCategoryType.Expense, "Expense"},
            {ExpenseCategoryType.Income, "Income"}
        };

        protected override IEnumerable<ExpenseCategoryViewData> FillUpViewItems (IEnumerable<ExpenseCategoryViewData> items)
        {
            return items.Select (
                item => { 
                    item.TypeName = m_categoryNames.SafeGet(item.Type);
                    return item;
                });
        }

        protected override ExpenseCategoryViewData FillUpViewItem (ExpenseCategoryViewData item)
        {
            item.TypeName = m_categoryNames.SafeGet (item.Type);
            return item;
        }

        protected override void PopulateSelectLists (ExpenseCategoryViewData category)
        {
            ViewBag.Type = new SelectList (m_categoryNames, nameof(KeyValuePair<int,int>.Key), nameof(KeyValuePair<int,int>.Value), category?.Type ?? ExpenseCategoryType.Expense);
        }
    }
}

