using System;

namespace Expenses.BL.Entities
{
    public class Operation
    {
        public int Id { get; set; }

        public DateTime OperationTime { get; set; }

        public int UserId { get; set; }

        public int AccountId { get; set; }

        public double Amount { get; set; }

        public int ExpenseItemId { get; set; }
    }
}

