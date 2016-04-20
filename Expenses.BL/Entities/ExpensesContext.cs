using System;
using System.Data.Entity;
using System.Data.Common;

namespace Expenses.BL.Entities
{
    public class ExpensesContext : DbContext
    {
        public ExpensesContext() : base(){}

        public ExpensesContext(string nameOrConnectionString) : base(nameOrConnectionString) {}

        public ExpensesContext (DbConnection connection) : base (connection, true){}

        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Exchange> Exchanges { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<KeyValuePair> KeyValuePairs { get; set; }
        public DbSet<Debt> Debts { get; set; }
        public DbSet<Repayment> Repayments { get; set; }
    }
}

