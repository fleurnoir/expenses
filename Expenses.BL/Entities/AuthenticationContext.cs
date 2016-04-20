using System;
using System.Data.Entity;
using Expenses.BL.Entities;
using System.Data.Common;

namespace Expenses.BL.Entities
{
    public class AuthenticationContext : DbContext
    {
        public AuthenticationContext ():base()
        {
        }

        public AuthenticationContext(string nameOrConnectionString) : base(nameOrConnectionString) {}

        public AuthenticationContext(DbConnection connection) : base(connection, true) {}

        public DbSet<User> Users { get; set; }
    }
}

