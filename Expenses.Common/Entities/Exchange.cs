using System;

namespace Expenses.BL.Entities
{
    public class Exchange : OperationBase
    {
        public long SourceAccountId { get; set; }
        public double SourceAmount { get; set; }
        public long DestAccountId { get; set; }
        public double DestAmount { get; set; }

        public override void CheckFields ()
        {
            if (SourceAmount < 0.01 || DestAmount < 0.01)
                throw new ArgumentException ("Amounts must be >= 0.01");
            if (SourceAccountId <= 0 || DestAccountId <= 0)
                throw new ArgumentException ("Account not chosen");
        }
    }
}

