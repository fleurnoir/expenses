using System;
using System.Data.Common;

namespace Expenses.BL.Service
{
    public interface IConnectionProvider
    {
        DbConnection CreateConnection();
    }
}

