using System;
using Expenses.BL.Entities;
using Expenses.Common.Utils;

namespace Expenses.Web.Models
{
    public class CategoryViewData : Category
    {
        public CategoryViewData(){}

        public CategoryViewData(Category category)
        {
            Cloner.Clone (category, this);
        }

        public string TypeName { get; set; }
    }
}

