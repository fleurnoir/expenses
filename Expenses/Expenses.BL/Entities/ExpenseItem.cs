using System;

namespace Expenses.BL.Entities
{
    public class ExpenseItem
    {
        public int Id { get; set; }

        public int ExpenseCategoryId { get; set; }

        public string Name { get; set; }
    }
}

