using System;
using Expenses.BL.Entities;
using System.Data.Common;
using Expenses.BL.Service;

namespace Expenses.Web
{
    public interface IDatabaseManager
    {
        DbConnection CreateUsersDbConnection ();
        bool DatabaseExists(User dbUser);
        void CreateDatabase(User dbUser);
        IConnectionProvider GetConnectionProvider(User dbUser);
    }
}

