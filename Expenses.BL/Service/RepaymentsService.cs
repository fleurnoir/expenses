using System;
using Expenses.BL.Entities;
using System.Linq;
using System.Collections.Generic;

namespace Expenses.BL.Service
{
    public class RepaymentsService : OperationsServiceBase<Repayment>
    {
        public RepaymentsService(IDataContextProvider provider, long userId) : base(provider, userId){}

        private class RepaymentOperation : SimpleOperation<Repayment>
        {
            public RepaymentOperation(Repayment operation) : base(operation){} 

            public override Account GetAccount(ExpensesContext db) {
                return (from rep in db.Repayments
                    join d in db.Debts on rep.DebtId equals d.Id
                    join a in db.Accounts on d.AccountId equals a.Id
                    where rep.Id == m_operation.Id
                    select a).First();
            }

            public override OperationType GetOperationType(ExpensesContext db) {
                return (from rep in db.Repayments
                    join d in db.Debts on rep.DebtId equals d.Id
                    where rep.Id == m_operation.Id
                    select d.Type).First() == DebtType.Lend ? OperationType.Income : OperationType.Expense;
            }
        }

        protected override void CommitOperation (ExpensesContext db, Repayment operation, bool rollback)
        {
            OperationsService.CommitSimpleOperation(db, new RepaymentOperation(operation), rollback);
        }

        public IList<Repayment> GetRepayments(long debtId) {
            using (var db = CreateContext ())
                return db.Repayments.Where (r => r.DebtId == debtId).ToList();
        }
    }
}

