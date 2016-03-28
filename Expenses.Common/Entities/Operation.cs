using System;

namespace Expenses.BL.Entities
{
    public class Operation : OperationBase
    {
        public long AccountId { get; set; }

        public double Amount { get; set; }

        public long SubcategoryId { get; set; }

        public override void CheckFields ()
        {
            if (Amount < 0.01)
                throw new ArgumentException ("Operation amount must be >= 0.01");
            if (AccountId <= 0)
                throw new ArgumentException ("Account not chosen");
            if (SubcategoryId <= 0)
                throw new ArgumentException ("Subcategory not chosen");
        }
    }
}

