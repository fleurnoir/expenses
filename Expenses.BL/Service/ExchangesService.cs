using System;
using Expenses.BL.Entities;

namespace Expenses.BL.Service
{
    public class ExchangesService : OperationsServiceBase<Exchange>
    {
        public ExchangesService (IDataContextProvider provider, long userId) : base(provider, userId)
        {
        }

        protected override void CommitOperation (ExpensesContext db, Exchange exchange, bool rollback)
        {
            if (exchange.SourceAccountId == exchange.DestAccountId)
                throw new InvalidOperationException ("Cannot exchange from an account to itself");
            var source = db.Accounts.Find (exchange.SourceAccountId);
            var dest = db.Accounts.Find (exchange.DestAccountId);
            if (source.CurrencyId == dest.CurrencyId && Math.Abs (exchange.SourceAmount - exchange.DestAmount) > 0.0001)
                throw new InvalidOperationException ("Exchange operation between accounts with the same currency must have equal amounts");
            exchange.SourceAmount = Math.Round (exchange.SourceAmount, 2);
            exchange.DestAmount = Math.Round (exchange.DestAmount, 2);
            double sign = rollback ? -1.0 : 1.0;
            source.Amount = Math.Round (source.Amount - sign*exchange.SourceAmount);
            dest.Amount = Math.Round (dest.Amount + sign*exchange.DestAmount);
        }
    }
}

