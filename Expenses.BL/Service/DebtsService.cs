using System;
using Expenses.BL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Expenses.BL.Service
{
    public class DebtsService : OperationsServiceBase<Debt>
    {
        public DebtsService(IDataContextProvider provider, long userId) : base(provider, userId){}

        protected override void CommitOperation (ExpensesContext db, Debt operation, bool rollback)
        {
            var wrapper = new AmountWrapper (()=>operation.Amount-operation.RepayedAmount, amount => operation.Amount = amount + operation.RepayedAmount);
            OperationsService.CommitAndRound (wrapper, db.Accounts.Find (operation.AccountId), operation.Type == DebtType.Borrow ? OperationType.Income : OperationType.Expense, rollback);
        }

        protected override void BeforeAdd (ExpensesContext db, Debt operation)
        {
            if (operation.RepayedAmount != 0.0)
                throw new InvalidOperationException ($"{nameof(Debt.RepayedAmount)} must be zero for new Debt record");
            base.BeforeAdd (db, operation);
        }

        protected override void BeforeUpdate (ExpensesContext db, Debt operation)
        {
            var existing = db.Debts.Find (operation.Id);
            if (Math.Abs (operation.RepayedAmount - existing.RepayedAmount) > 0.00001)
                throw new InvalidOperationException ("{nameof(Debt.RepayedAmount)} cannot be edited");
            base.BeforeUpdate (db, operation);
        }

        public IList<Debt> GetDebts(DebtType? type) {
            using (var db = CreateContext ()) {
                IQueryable<Debt> query = db.Debts;
                if (type != null)
                    query = query.Where (debt=>debt.Type == type);
                return query.ToList ();
            }
        }
    }
}

