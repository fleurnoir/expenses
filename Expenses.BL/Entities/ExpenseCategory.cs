using System;

namespace Expenses.BL.Entities
{
    public class ExpenseCategory : IUnique
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ExpenseCategoryType Type { get; set; }

        public string Comment { get; set; }
    }
}

