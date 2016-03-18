using System;
using Expenses.BL.Entities;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;

namespace Expenses.BL.Service
{
    public class ExpensesService : IExpensesService
    {
        private IDataContextProvider m_provider;
        private long m_userId;

        private ExpensesContext CreateContext() => m_provider.CreateContext();

        public ExpensesService (IDataContextProvider contextProvider, long userId)
        {
            if (contextProvider == null)
                throw new ArgumentNullException (nameof (contextProvider));
            if (userId <= 0)
                throw new ArgumentNullException (nameof (userId));
            m_provider = contextProvider;
            m_userId = userId;
        }

        private TEntity Add<TEntity>(TEntity item) where TEntity : Entity
        {
            if (item == null)
                throw new ArgumentNullException (nameof(item));
            item.CheckFields ();
            using (var context = CreateContext ()) 
            {
                context.Set<TEntity> ().Add (item);
                context.SaveChanges ();
                return item;
            }
        }

        private TEntity Update<TEntity>(TEntity item) where TEntity : Entity
        {
            if (item == null)
                throw new ArgumentNullException (nameof(item));
            item.CheckFields ();
            using (var context = CreateContext ()) 
            {
                context.Entry (item).State = EntityState.Modified;
                context.SaveChanges ();
                return item;
            }
        }

        private void Delete<TEntity>(long itemId) where TEntity : class, IUnique, new()
        {
            using (var context = CreateContext ()) 
            {
                context.Set<TEntity>().Remove(new TEntity {Id=itemId});
                context.SaveChanges ();
            }
        }

        private TEntity Select<TEntity>(long id) where TEntity : class, IUnique
        {
            using (var context = CreateContext ())
                return context.Set<TEntity> ().FirstOrDefault (item=>item.Id == id);
        }

        public Operation AddOperation(Operation operation)
        {
            operation.UserId = m_userId;
            operation.OperationTime = DateTime.Now;
            return Add (operation);
        }

        public Operation UpdateOperation(Operation operation)
        {
            operation.UserId = m_userId;
            operation.OperationTime = DateTime.Now;
            return Update (operation);
        }

        public void DeleteOperation (long operationId) => Delete<Operation>(operationId);
        public Operation GetOperation (long operationId) => Select<Operation>(operationId);

        public ExpenseItem AddExpense (ExpenseItem expense) => Add(expense);
        public ExpenseItem UpdateExpense (ExpenseItem expense) => Update(expense);
        public void DeleteExpense (long expenseId) => Delete<ExpenseItem>(expenseId);
        public ExpenseItem GetExpense (long expenseId) => Select<ExpenseItem>(expenseId);

        public ExpenseCategory AddCategory (ExpenseCategory category) => Add(category);
        public ExpenseCategory UpdateCategory (ExpenseCategory category) => Update(category);
        public void DeleteCategory (long categoryId) => Delete<ExpenseCategory>(categoryId);
        public ExpenseCategory GetCategory (long categoryId) => Select<ExpenseCategory>(categoryId);

        public Currency AddCurrency(Currency currency) => Add(currency);
        public Currency UpdateCurrency(Currency currency) => Update(currency);
        public void DeleteCurrency(long currencyId) => Delete<Currency>(currencyId);
        public Currency GetCurrency(long currencyId) => Select<Currency>(currencyId);

        public Account AddAccount(Account account) => Add(account);
        public Account UpdateAccount(Account account) => Update(account);
        public void DeleteAccount(long accountId) => Delete<Account>(accountId);
        public Account GetAccount(long accountId) => Select<Account>(accountId);

        public IList<Operation> GetOperations ()
        {
            using (var context = m_provider.CreateContext ())
                return context.Operations.ToList ();
        }

        public IList<ExpenseItem> GetExpenseItems (long? categoryId = null)
        {
            using (var context = m_provider.CreateContext ()) {
                IQueryable<ExpenseItem> query = context.ExpenseItems;
                if (categoryId != null)
                    query = query.Where (c => c.ExpenseCategoryId == (long)categoryId);
                return query.ToList ();
            }
        }

        public IList<ExpenseCategory> GetCategories (ExpenseCategoryType? categoryType = null)
        {
            using (var context = m_provider.CreateContext ()) {
                IQueryable<ExpenseCategory> query = context.ExpenseCategories;
                if (categoryType != null)
                    query = query.Where (c => c.Type == (ExpenseCategoryType)categoryType);
                return query.ToList ();
            }
        }

        public IList<Currency> GetCurrencies ()
        {
            using (var context = m_provider.CreateContext ())
                return context.Currencies.ToList ();
        }

        public IList<Account> GetAccounts ()
        {
            using (var context = m_provider.CreateContext ())
                return context.Accounts.ToList ();
        }

        void IDisposable.Dispose ()
        {
        }
    }
}

