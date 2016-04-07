using System;
using Expenses.BL.Entities;
using System.Linq;
using System.Collections.Generic;

namespace Expenses.BL.Service
{
    public class RepaymentsService : OperationsServiceBase<Repayment>
    {
        public RepaymentsService(IDataContextProvider provider, long userId) : base(provider, userId){}

        protected override void BeforeUpdate (ExpensesContext db, Repayment operation)
        {
            var existing = db.Repayments.Find (operation.Id);
            if (operation.DebtId != existing.DebtId)
                throw new InvalidOperationException ($"{nameof(Repayment.DebtId)} cannot be changed");
            base.BeforeUpdate (db, operation);
        }

        protected override void CommitOperation (ExpensesContext db, Repayment operation, bool rollback)
        {
            var da = (from d in db.Debts
                join a in db.Accounts on d.AccountId equals a.Id
                where d.Id == operation.DebtId
                select new {debt = d, account = a}).First ();
            OperationsService.CommitAndRound (operation, da.account, da.debt.Type == DebtType.Lend ? OperationType.Income : OperationType.Expense, rollback);
            var debtWrapper = new AmountWrapper (() => da.debt.RepayedAmount, amount=>da.debt.RepayedAmount = amount);
            OperationsService.CommitAndRound (operation, debtWrapper, OperationType.Income, rollback);
        }

        public IList<Repayment> GetRepayments(long debtId) {
            using (var db = CreateContext ())
                return db.Repayments.Where (r => r.DebtId == debtId).OrderByDescending (r => r.OperationTime).ToList ();
        }
    }
}

