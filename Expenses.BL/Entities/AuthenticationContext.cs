using System;
using System.Data.Entity;
using Expenses.BL.Entities;

namespace Expenses.BL.Entities
{
    public class AuthenticationContext : DbContext
    {
        public AuthenticationContext ():base()
        {
        }

        public AuthenticationContext(string nameOrConnectionString) : base(nameOrConnectionString) {}
        public DbSet<User> Users { get; set; }
    }
}

