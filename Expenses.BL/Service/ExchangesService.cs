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
            CommitAndRound(new AmountWrapper(()=>exchange.SourceAmount, amount=>exchange.SourceAmount = amount), 
                source, OperationType.Expense, rollback);
            CommitAndRound(new AmountWrapper(()=>exchange.DestAmount, amount=>exchange.DestAmount = amount), 
                dest, OperationType.Income, rollback);
        }
    }
}

