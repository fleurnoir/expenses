using System;
using Expenses.BL.Entities;

namespace Expenses.Web
{
    public interface IDatabaseManager
    {
        bool DatabaseExists(User dbUser);
        void CreateDatabase(User dbUser);
        string GetConnectionString(User dbUser);
    }
}

