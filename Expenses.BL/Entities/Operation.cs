using System;

namespace Expenses.BL.Entities
{
    public class Operation : Entity
    {
        public DateTime OperationTime { get; set; }

        public long UserId { get; set; }

        public long AccountId { get; set; }

        public double Amount { get; set; }

        public long ExpenseItemId { get; set; }
    }
}

