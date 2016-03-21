using System;
using System.Linq;

namespace Expenses.BL.Entities
{
    public class ExpenseCategory : Entity
    {
        public string Name { get; set; }

        public ExpenseCategoryType Type { get; set; }

        public override void CheckFields ()
        {
            if (String.IsNullOrEmpty (Name))
                throw new ArgumentException ($"The field {nameof(Name)} cannot be empty");
            if (!Enum.GetValues(typeof(ExpenseCategoryType)).Cast<ExpenseCategoryType>().Any(type=>Type==type))
                throw new ArgumentException ("Unknown category type");
        }
    }
}

