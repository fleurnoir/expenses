using System;
using System.Data.Entity;

namespace Expenses.BL.Entities
{
    public class DataContext : DbContext
    {
        public DbSet<ExpenseItem> ExpenseItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<Operation> Operations { get; set; }
    }
}

