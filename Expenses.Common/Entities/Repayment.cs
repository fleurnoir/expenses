using System;

namespace Expenses.BL.Entities
{
    public class Repayment : OperationBase, IAmount
    {
        public long DebtId { get; set; }
        public double Amount { get; set; }

        public override void CheckFields ()
        {
            if (Amount < 0.005)
                throw new ArgumentException ($"{nameof(Amount)} must be greater than 0.009");
            if (DebtId <= 0)
                throw new ArgumentException ($"{nameof(DebtId)} cannot be empty");
        }
    }
}

