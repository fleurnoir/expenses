using System;
using Expenses.BL.Service;

namespace Expenses.Web.DAL
{
    public class ExpensesServiceFactory
    {
        public IExpensesService Create()
        {
            return new ExpensesService (new ContextProvider());
        }
    }
}

