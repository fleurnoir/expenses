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
        private static IDictionary<CategoryType, string> m_categoryNames = new Dictionary<CategoryType, string>
        {
            {CategoryType.Expense, Strings.Expense},
            {CategoryType.Income, Strings.Income}
        };

        protected override IEnumerable<CategoryViewData> FillUpViewItems (IEnumerable<CategoryViewData> items)
        {
            return items.Select (
                item => { 
                    item.TypeName = m_categoryNames.SafeGet(item.Type);
                    return item;
                });
        }

        protected override CategoryViewData FillUpViewItem (CategoryViewData item)
        {
            item.TypeName = m_categoryNames.SafeGet (item.Type);
            return item;
        }

        protected override void PopulateSelectLists (CategoryViewData category)
        {
            ViewBag.Type = new SelectList (m_categoryNames, nameof(KeyValuePair<int,int>.Key), nameof(KeyValuePair<int,int>.Value), category?.Type ?? CategoryType.Expense);
        }
    }
}

