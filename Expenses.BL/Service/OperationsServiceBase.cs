using System;
using Expenses.BL.Entities;
using Expenses.BL.Common;
using System.Data;
using System.Linq;
using Expenses.Common.Utils;
using System.Collections.Generic;

namespace Expenses.BL.Service
{
    public abstract class OperationsServiceBase<TOperation> : EntityService<TOperation> where TOperation : OperationBase, new()
    {
        private long m_userId;

        public OperationsServiceBase (IDataContextProvider provider, long userId):base(provider)
        {
            m_userId = userId;
        }

        protected static void CommitAndRound(IAmount operation, IAmount destination, OperationType type,  bool rollback)
        {
            var accountAmount = Math.Round (destination.Amount, 2);
            var operationAmount = Math.Round (operation.Amount, 2);
            var sign1 = type == OperationType.Income ? 1.0 : -1.0;
            var sign2 = rollback ? -1.0 : 1.0;
            operation.Amount = operationAmount;
            destination.Amount = Math.Round(accountAmount + sign1 * sign2 * operationAmount, 2);
        }

        protected override void BeforeAdd (ExpensesContext context, TOperation operation) {
            operation.UserId = m_userId;
            operation.OperationTime = DateTime.Now;
            operation.CheckFields ();
            CommitOperation (context, operation);
        }

        protected override TOperation AddCore(ExpensesContext context, TOperation operation)
        {
            using (var transaction = context.BeginTransaction (IsolationLevel.Serializable)) {
                var result = base.AddCore (context, operation);
                transaction.Commit ();
                return result;
            }
        }

        private void RollbackOperation(ExpensesContext db, TOperation operation)
        {
            CommitOperation (db, operation, true);
        }

        private void CommitOperation (ExpensesContext db, TOperation operation)
        {
            CommitOperation (db, operation, false);
        }

        protected abstract void CommitOperation (ExpensesContext db, TOperation operation, bool rollback);

        protected override void BeforeUpdate (ExpensesContext db, TOperation operation)
        {
            operation.UserId = m_userId;
            operation.CheckFields ();
            var dbOperation = db.Set<TOperation>().Find (operation.Id);

            //Operation time must stay unchanged
            operation.OperationTime = dbOperation.OperationTime;
            RollbackOperation (db, dbOperation);
            CommitOperation (db, operation);
            Cloner.Clone (operation, dbOperation);
        }

        protected override TOperation UpdateCore(ExpensesContext db, TOperation operation)
        {
            using (var transaction = db.BeginTransaction(IsolationLevel.Serializable))    
            {
                BeforeUpdate (db, operation);
                db.SaveChanges ();
                transaction.Commit();
                return operation;
            }
        }

        public override void Delete (long itemId)
        {
            using (var db = CreateContext ())
            using (var transaction = db.BeginTransaction(IsolationLevel.Serializable))    
            {
                var operation = db.Set<TOperation>().Find (itemId);
                RollbackOperation (db, operation);
                db.Set<TOperation>().Remove (operation);
                db.SaveChanges ();
                transaction.Commit ();
            }
        }

        public override IList<TOperation> Select ()
        {
            using (var context = CreateContext ())
                return context.Set<TOperation>().OrderByDescending (o=>o.Id).ToList();
        }
    }
}

