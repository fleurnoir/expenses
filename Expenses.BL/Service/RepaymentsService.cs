using System;
using Expenses.BL.Entities;
using System.Linq;
using System.Collections.Generic;

namespace Expenses.BL.Service
{
    public class RepaymentsService : OperationsServiceBase<Repayment>
    {
        public RepaymentsService(IDataContextProvider provider, long userId) : base(provider, userId){}

        protected override void CommitOperation (ExpensesContext db, Repayment operation, bool rollback)
        {
            var da = (from rep in db.Repayments
                join d in db.Debts on rep.DebtId equals d.Id
                join a in db.Accounts on d.AccountId equals a.Id
                where rep.Id == operation.Id
                select new {debt = d, account = a}).First ();
            OperationsService.CommitAndRound (operation, da.account, da.debt.Type == DebtType.Lend ? OperationType.Income : OperationType.Expense, rollback);
            var debtWrapper = new AmountWrapper (() => da.debt.RepayedAmount, amount=>da.debt.RepayedAmount = amount);
            OperationsService.CommitAndRound (operation, debtWrapper, OperationType.Income, rollback);
        }

        public IList<Repayment> GetRepayments(long debtId) {
            using (var db = CreateContext ())
                return db.Repayments.Where (r => r.DebtId == debtId).ToList();
        }
    }
}

