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

        public string SubcategoryName { get; set; }

        public long CategoryId { get; set; }

        public string CategoryName { get; set; }

        public OperationType Type { get; set; }

        public string AccountName { get; set; }

        public long CurrencyId { get; set; }

        public string CurrencyName { get; set; }
    }
}

