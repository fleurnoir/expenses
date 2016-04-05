using System;

namespace Expenses.BL.Entities
{
    public class Debt : OperationBase, IAmount
    {
        public string AgentName { get; set; }
        public double Amount { get; set; }
		public double RepayedAmount { get; set; }
        public long AccountId { get; set; }
        public DebtType Type { get; set; }
        public bool Repayed { get; set; }

        public override void CheckFields ()
        {
            if (String.IsNullOrEmpty (AgentName))
                throw new ArgumentException ($"{nameof(AgentName)} cannot be empty");
            if (Amount < 0.005)
                throw new ArgumentException ($"{nameof(Amount)} must be greater than 0.009");
            if (Type != DebtType.Lend && Type != DebtType.Borrow)
                throw new ArgumentException ("Unknown debt type");
            if (AccountId <= 0)
                throw new ArgumentException ($"{nameof(AccountId)} cannot be empty");
        }
    }
}

