using System;
using Expenses.BL.Entities;
using Expenses.BL.Service;
using Expenses.Common.Utils;

namespace Expenses.BL.Service
{
    public class ExpensesServiceFactory : IServiceFactory<IExpensesService>
    {
        IDataContextProvider m_contextProvider;

        ICurrentUserProvider m_userProvider;

        public ExpensesServiceFactory(IDataContextProvider contextProvider, ICurrentUserProvider userProvider)
        {
            if (contextProvider == null)
                throw new ArgumentNullException (nameof(contextProvider));
            if (userProvider == null)
                throw new ArgumentNullException (nameof(userProvider));
            m_userProvider = userProvider;
            m_contextProvider = contextProvider;
        }

        public IExpensesService CreateService () => new ExpensesService(m_contextProvider, m_userProvider.GetUserId());
    }
}

