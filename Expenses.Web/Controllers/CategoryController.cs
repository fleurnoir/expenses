using System;
using Expenses.BL.Entities;
using System.Web.Mvc;
using System.Linq;
using Expenses.Web.Models;
using Expenses.Common.Utils;
using System.Collections.Generic;

namespace Expenses.Web.Controllers
{
    public class CategoryController : ExpensesController<Category, CategoryViewData>
    {
        private static IDictionary<OperationType, string> m_typeNames = new Dictionary<OperationType, string>
        {
            {OperationType.Expense, Strings.Expense},
            {OperationType.Income, Strings.Income}
        };

        protected override IEnumerable<CategoryViewData> FillUpViewItems (IEnumerable<CategoryViewData> items)
        {
            return items.Select (
                item => { 
                    item.TypeName = m_typeNames.SafeGet(item.Type);
                    return item;
                });
        }

        protected override CategoryViewData FillUpViewItem (CategoryViewData item)
        {
            item.TypeName = m_typeNames.SafeGet (item.Type);
            return item;
        }

        protected override void PopulateSelectLists (CategoryViewData category)
        {
            ViewBag.Type = new SelectList (m_typeNames, nameof(KeyValuePair<int,int>.Key), nameof(KeyValuePair<int,int>.Value), category?.Type ?? OperationType.Expense);
        }
    }
}

