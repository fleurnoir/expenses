using System;
using Expenses.BL.Entities;

namespace Expenses.Web.Models
{
    public class ExpenseViewData : Operation
    {
        public string UserLogin { get; set; }

        public string AccountName { get; set; }

        public string ExpenseItemName { get; set; }

        public string ExpenseCategoryName { get; set; }
    }
}

