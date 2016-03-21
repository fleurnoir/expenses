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

        public override void CheckFields ()
        {
            if (Amount < 0.01)
                throw new ArgumentException ("Operation amount must be >= 0.01");
            if (AccountId <= 0)
                throw new ArgumentException ("Account not chosen");
            if (ExpenseItemId <= 0)
                throw new ArgumentException ("ExpenseItem not chosen");
        }
    }
}

