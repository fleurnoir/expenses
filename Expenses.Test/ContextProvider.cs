using System;
using Expenses.BL.Service;
using Expenses.BL.Entities;

namespace Expenses.Test
{
    public class ContextProvider : IDataContextProvider
    {
        public DataContext CreateContext ()
        {
            return new DataContext ("name=expenses");
        }
    }
}

