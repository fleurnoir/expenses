using System;

namespace Expenses.BL.Entities
{
    public class ExpenseCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ExpenseCategoryType Type { get; set; }
    }
}

