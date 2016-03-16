using System;
using Expenses.BL.Entities;
using Expenses.BL.Service;

namespace Expenses.Web.DAL
{
    public class ContextProvider : IDataContextProvider
    {
        public ExpensesContext CreateContext ()
        {
            return new ExpensesContext ("name=expenses");
        }
    }
}

