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

        public override TOperation Add(TOperation operation)
        {
            operation.UserId = m_userId;
            operation.OperationTime = DateTime.Now;
            operation.CheckFields ();
            using (var db = CreateContext ())
            using (var transaction = db.BeginTransaction(IsolationLevel.Serializable))    
            {
                CommitOperation (db, operation);
                operation = db.Set<TOperation>().Add (operation);
                db.SaveChanges ();
                transaction.Commit ();
            }
            return operation;
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

        public override TOperation Update(TOperation operation)
        {
            operation.UserId = m_userId;
            //operation.OperationTime = DateTime.Now;
            operation.CheckFields ();
            using (var db = CreateContext ())
            using (var transaction = db.BeginTransaction(IsolationLevel.Serializable))    
            {
                var dbOperation = db.Set<TOperation>().Find (operation.Id);
                //Operation time must stay unchanged
                operation.OperationTime = dbOperation.OperationTime;
                RollbackOperation (db, dbOperation);
                CommitOperation (db, operation);
                Cloner.Clone (operation, dbOperation);
                db.SaveChanges ();
                transaction.Commit ();
                return dbOperation;
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

