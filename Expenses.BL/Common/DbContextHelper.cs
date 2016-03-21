using System;
using System.Data.Entity;
using System.Data;

namespace Expenses.BL.Common
{
    public static class DbContextHelper
    {
        public static DbContextTransactionScope BeginTransaction(this DbContext context)
        {
            return new DbContextTransactionScope (context.Database.BeginTransaction());
        }

        public static DbContextTransactionScope BeginTransaction(this DbContext context, IsolationLevel isolationLevel)
        {
            return new DbContextTransactionScope (context.Database.BeginTransaction(isolationLevel));
        }
    }
}

