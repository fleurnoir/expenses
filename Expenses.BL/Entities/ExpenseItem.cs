using System;

namespace Expenses.BL.Entities
{
    public class ExpenseItem : IUnique
    {
        public long Id { get; set; }

        public long ExpenseCategoryId { get; set; }

        public string Name { get; set; }

        public string Comment { get; set; }
    }
}

