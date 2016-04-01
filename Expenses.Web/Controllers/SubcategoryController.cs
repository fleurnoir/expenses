using System;
using Expenses.BL.Entities;
using System.Web.Mvc;
using System.Linq;
using Expenses.Web.Models;
using Expenses.Common.Utils;
using System.Collections.Generic;

namespace Expenses.Web.Controllers
{
    public class SubcategoryController : ExpensesController<Subcategory, SubcategoryViewData>
    {
        protected override IEnumerable<SubcategoryViewData> FillUpViewItems (IEnumerable<SubcategoryViewData> items)
        {
            var categories = Service.GetCategories (null).ToDictionary(item=>item.Id, item=>item.Name);
            return items.Select (
                item => { 
                    item.CategoryName = categories.SafeGet (item.CategoryId);
                    return item;
                });
        }

        protected override SubcategoryViewData FillUpViewItem (SubcategoryViewData item)
        {
            item.CategoryName = Service.GetCategory (item.CategoryId).Name;
            return item;
        }

        protected override void PopulateSelectLists (SubcategoryViewData item)
        {
            var type = item == null || item.CategoryId <= 0 ? 
                (CategoryType?)null : 
                Service.GetCategory (item.CategoryId).Type;
            var categories = Service.GetCategories (type);
            ViewBag.CategoryId = new SelectList (categories, nameof(Category.Id), nameof(Category.Name), item?.CategoryId ?? categories.FirstOrDefault()?.Id);
        }
    }
}

