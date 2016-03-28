using System;
using Expenses.BL.Entities;
using Expenses.Common.Utils;

namespace Expenses.Web.Models
{
    public class SubcategoryViewData : Subcategory
    {
        public SubcategoryViewData(){}

        public SubcategoryViewData (Subcategory item)
        {
            Cloner.Clone (item, this);
        }

        public string CategoryName { get; set; }
    }
}

