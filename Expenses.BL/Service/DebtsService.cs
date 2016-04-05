using System;
using Expenses.BL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Expenses.BL.Service
{
    public class DebtsService : OperationsServiceBase<Debt>
    {
        public DebtsService(IDataContextProvider provider, long userId) : base(provider, userId){}

        private class DebtOperation : SimpleOperation<Debt>
        {
            public DebtOperation(Debt operation) : base(operation) {
            }

            public override Account GetAccount(ExpensesContext db) {
                return db.Accounts.Find (m_operation.AccountId);
            }

            public override OperationType GetOperationType(ExpensesContext db) {
                return m_operation.Type == DebtType.Borrow ? OperationType.Income : OperationType.Expense;
            }
        }

        protected override void CommitOperation (ExpensesContext db, Debt operation, bool rollback)
        {
            OperationsService.CommitSimpleOperation(db, new DebtOperation(operation), rollback);
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

