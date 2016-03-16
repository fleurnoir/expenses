using System;
using System.Data.Entity;

namespace Expenses.BL.Entities
{
    public class ExpensesContext : DbContext
    {
        public ExpensesContext() : base(){}

        public ExpensesContext(string nameOrConnectionString) : base(nameOrConnectionString) {}

        public DbSet<ExpenseItem> ExpenseItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Currency> Currencies { get; set; }
    }
}

