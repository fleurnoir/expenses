using System;
using Expenses.BL.Entities;
using Expenses.Common.Utils;

namespace Expenses.Web.Models
{
    public class ExpenseItemViewData : ExpenseItem
    {
        public ExpenseItemViewData(){}

        public ExpenseItemViewData (ExpenseItem item)
        {
            Cloner.Clone (item, this);
        }

        public string ExpenseCategoryName { get; set; }
    }
}

