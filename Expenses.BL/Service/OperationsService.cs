using System;
using Expenses.BL.Entities;
using Expenses.BL.Common;
using System.Linq;
using System.Data;
using Expenses.Common.Utils;
using System.Collections.Generic;

namespace Expenses.BL.Service
{
    public class OperationsService : EntityService<Operation>
    {
        private long m_userId;

        public OperationsService (IDataContextProvider provider, long userId):base(provider)
        {
            m_userId = userId;
        }

        public override Operation Add(Operation operation)
        {
            operation.UserId = m_userId;
            operation.OperationTime = DateTime.Now;
            operation.CheckFields ();
            using (var context = CreateContext ())
            using (var transaction = context.BeginTransaction(IsolationLevel.Serializable))    
            {
                CommitOperation (context, operation);
                operation = context.Operations.Add (operation);
                context.SaveChanges ();
                transaction.Commit ();
            }
            return operation;
        }

        private static void RollbackOperation(ExpensesContext context, Operation operation)
        {
            CommitOperation (context, operation, true);
        }

        private static void CommitOperation(ExpensesContext context, Operation operation, bool backwards = false)
        {
            var account = context.Accounts.Find (operation.AccountId);
            var type = 
                (from ei in context.ExpenseItems
                    join ec in context.ExpenseCategories on ei.ExpenseCategoryId equals ec.Id
                    where ei.Id == operation.ExpenseItemId
                    select ec.Type).First ();
            var accountAmount = Math.Round (account.Amount, 2);
            var operationAmount = Math.Round (operation.Amount, 2);
            var sign1 = type == ExpenseCategoryType.Income ? 1.0 : -1.0;
            var sign2 = backwards ? -1.0 : 1.0;
            operation.Amount = operationAmount;
            account.Amount = accountAmount + sign1 * sign2 * operationAmount;
        }

        public override Operation Update(Operation operation)
        {
            operation.UserId = m_userId;
            //operation.OperationTime = DateTime.Now;
            operation.CheckFields ();
            using (var context = CreateContext ())
            using (var transaction = context.BeginTransaction(IsolationLevel.Serializable))    
            {
                var dbOperation = context.Operations.Find (operation.Id);
                //Operation time must stay unchanged
                operation.OperationTime = dbOperation.OperationTime;
                RollbackOperation (context, dbOperation);
                CommitOperation (context, operation);
                Cloner.Clone (operation, dbOperation);
                context.SaveChanges ();
                transaction.Commit ();
                return dbOperation;
            }
        }

        public override void Delete (long itemId)
        {
            using (var context = CreateContext ())
            using (var transaction = context.BeginTransaction(IsolationLevel.Serializable))    
            {
                var operation = context.Operations.Find (itemId);
                RollbackOperation (context, operation);
                context.Operations.Remove (operation);
                context.SaveChanges ();
                transaction.Commit ();
            }
        }

        public override IList<Operation> Select ()
        {
            using (var context = CreateContext ())
                return context.Operations.OrderByDescending (o=>o.Id).ToList();
        }
    }
}

