using System;
using Expenses.BL.Entities;
using Expenses.Common.Utils;

namespace Expenses.Web.Models
{
    public class ExpenseCategoryViewData : ExpenseCategory
    {
        public ExpenseCategoryViewData(){}

        public ExpenseCategoryViewData(ExpenseCategory category)
        {
            Cloner.Clone (category, this);
        }

        public string TypeName { get; set; }
    }
}

