using System;

namespace Expenses.BL.Entities
{
    public class Operation : IUnique
    {
        public long Id { get; set; }

        public DateTime OperationTime { get; set; }

        public long UserId { get; set; }

        public long AccountId { get; set; }

        public double Amount { get; set; }

        public long ExpenseItemId { get; set; }

        public string Comment { get; set; }
    }
}

