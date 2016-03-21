using System;
using Expenses.BL.Entities;
using Expenses.BL.Service;

namespace Expenses.Common.Service
{
    public static class ExpensesServiceHelper
    {
        public static Currency GetCurrency(this Account account, IExpensesService service)
        {
            if (service == null)
                throw new ArgumentNullException ("service");
            if (account == null)
                throw new ArgumentNullException ("account");
            return service.GetCurrency (account.CurrencyId);
        }
    }
}

