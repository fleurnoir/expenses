using System;
using Expenses.BL.Entities;
using Expenses.BL.Common;
using System.Linq;
using System.Data;
using Expenses.Common.Utils;
using System.Collections.Generic;
using Expenses.Common.Service;

namespace Expenses.BL.Service
{
    public class OperationsService : OperationsServiceBase<Operation>
    {
        public OperationsService (IDataContextProvider provider, long userId):base(provider, userId)
        {
        }

        internal static void CommitAndRound(IAmount operation, IAmount destination, OperationType type,  bool rollback)
        {
            var accountAmount = Math.Round (destination.Amount, 2);
            var operationAmount = Math.Round (operation.Amount, 2);
            var sign1 = type == OperationType.Income ? 1.0 : -1.0;
            var sign2 = rollback ? -1.0 : 1.0;
            operation.Amount = operationAmount;
            destination.Amount = Math.Round(accountAmount + sign1 * sign2 * operationAmount, 2);
        }

        protected override void CommitOperation(ExpensesContext db, Operation operation, bool rollback)
        {
            CommitAndRound (
                operation, 
                db.Accounts.Find (operation.AccountId), 
                (from ei in db.Subcategories
                             join ec in db.Categories on ei.CategoryId equals ec.Id
                             where ei.Id == operation.SubcategoryId
                             select ec.Type).First (), 
                rollback);
        }

        public IList<Operation> Select (DateTime? startTime, DateTime? endTime, long? subcategoryId, long? categoryId)
        {
            using (var db = CreateContext ())
                return GetQuery (db, startTime, endTime, subcategoryId, categoryId).OrderByDescending (item => item.Id).ToList ();        
        }

        private static IQueryable<Operation> GetQuery (ExpensesContext db, DateTime? startTime, DateTime? endTime, long? subcategoryId, long? categoryId)
        {
            IQueryable<Operation> query = db.Operations;
            if (categoryId != null) {
                query = from op in db.Operations
                    join sub in db.Subcategories on op.SubcategoryId equals sub.Id
                        where sub.CategoryId == categoryId
                    select op;
            }
            if (subcategoryId != null)
                query = query.Where (op => op.SubcategoryId == subcategoryId);
            if (startTime != null)
                query = query.Where (op => op.OperationTime >= startTime);
            if (endTime != null)
                query = query.Where (op => op.OperationTime <= endTime);
            return query;
        }

        public IList<StatsItem> GetStatistics (DateTime? startTime = default(DateTime?), DateTime? endTime = default(DateTime?), long? subcategoryId = default(long?), long? categoryId = default(long?))
        {
            using (var db = CreateContext ())
            {
                var query = 
                    from op in db.Operations
                    join sub in db.Subcategories on op.SubcategoryId equals sub.Id
                    join cat in db.Categories on sub.CategoryId equals cat.Id
                    join acc in db.Accounts on op.AccountId equals acc.Id
                    select new {op, sub, cat, acc};

                if (categoryId != null)
                    query = query.Where (i => i.cat.Id == categoryId);
                if (subcategoryId != null)
                    query = query.Where (i => i.sub.Id == subcategoryId);
                if (startTime != null)
                    query = query.Where (i => i.op.OperationTime >= startTime);
                if (endTime != null)
                    query = query.Where (i => i.op.OperationTime <= endTime);

                return query.GroupBy (i => new {i.acc.CurrencyId, i.cat.Type})
                    .Select (g => new StatsItem {
                        CurrencyId = g.Key.CurrencyId,
                        Type = g.Key.Type,
                        Amount = g.Sum (i => i.op.Amount)
                    }).ToList();
            }
        }
    }
}

