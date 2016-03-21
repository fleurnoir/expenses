using System;
using Expenses.BL.Entities;
using Expenses.Common.Utils;

namespace Expenses.Web.Models
{
    public class OperationViewData : Operation
    {
        public OperationViewData() {}

        public OperationViewData(Operation operation)
        {
            Cloner.Clone (operation, this);
        }

        public string ExpenseItemName { get; set; }

        public long ExpenseCategoryId { get; set; }

        public string ExpenseCategoryName { get; set; }

        public string AccountName { get; set; }

        public string CurrencyName { get; set; }
    }
}

