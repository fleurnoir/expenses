using System;

namespace Expenses.BL.Entities
{
    public class ExpenseItem : Entity
    {
        public long ExpenseCategoryId { get; set; }

        public string Name { get; set; }

        public override void CheckFields ()
        {
            if (String.IsNullOrEmpty (Name))
                throw new ArgumentException ($"The field {nameof(Name)} cannot be empty");
            if (ExpenseCategoryId <= 0)
                throw new ArgumentException ("Category not set");
        }
    }
}

