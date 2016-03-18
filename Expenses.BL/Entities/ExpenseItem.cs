using System;

namespace Expenses.BL.Entities
{
    public class ExpenseItem : Entity
    {
        public long ExpenseCategoryId { get; set; }

        public string Name { get; set; }
    }
}

