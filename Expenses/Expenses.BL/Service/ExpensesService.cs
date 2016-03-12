using System;

namespace Expenses.BL.Service
{
    public class ExpensesService : IExpensesService
    {
        IDataContextProvider m_contextProvider;

        public ExpensesService (IDataContextProvider contextProvider)
        {
            m_contextProvider = contextProvider;     
        }
    }
}

