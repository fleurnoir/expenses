using System;

namespace Expenses.BL.Entities
{
    public class ExpenseCategory : Entity
    {
        public string Name { get; set; }

        public ExpenseCategoryType Type { get; set; }
    }
}

