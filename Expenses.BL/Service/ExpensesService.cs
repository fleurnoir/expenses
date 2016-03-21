using System;
using Expenses.BL.Entities;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using Expenses.Common.Utils;

namespace Expenses.BL.Service
{
    public class ExpensesService : IExpensesService
    {
        private static Dictionary<Type, Delegate> m_factories = new Dictionary<Type, Delegate> ();
        private static void RegisterFactory<TEntity>(Func<IDataContextProvider, long, IEntityService<TEntity>> factory) where TEntity : Entity, new()
            => m_factories.Add (typeof(TEntity), factory);
        private static void RegisterFactory<TEntity>() where TEntity : Entity, new()
            => RegisterFactory((provider, userId)=>new EntityService<TEntity>(provider));

        static ExpensesService()
        {
            RegisterFactory<Currency> ();
            RegisterFactory<Account> ();
            RegisterFactory<ExpenseItem> ();
            RegisterFactory<ExpenseCategory> ();
            RegisterFactory<Operation> ((provider,userId)=>new OperationsService(provider, userId));
        }

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

        public IEntityService<TEntity> GetEntityService<TEntity> () where TEntity : Entity, new()
        {
            var factory = m_factories.SafeGet(typeof(TEntity)) as Func<IDataContextProvider, long, IEntityService<TEntity>>;
            if (factory == null)
                throw new InvalidOperationException ($"Service is not available for type '{typeof(TEntity).Name}'");
            return factory (m_provider, m_userId);
        }

        private TEntity Add<TEntity> (TEntity item) where TEntity : Entity, new()
            => GetEntityService<TEntity>().Add(item);

        private TEntity Update<TEntity>(TEntity item) where TEntity : Entity, new()
           => GetEntityService<TEntity>().Update(item);

        private void Delete<TEntity>(long itemId) where TEntity : Entity, new()
            => GetEntityService<TEntity>().Delete(itemId);

        private TEntity Select<TEntity>(long itemId) where TEntity : Entity, new()
            => GetEntityService<TEntity>().Select(itemId);

        private IList<TEntity> Select<TEntity>() where TEntity : Entity, new()
            => GetEntityService<TEntity>().Select();

        public Operation AddOperation (Operation operation) => Add(operation);
        public Operation UpdateOperation (Operation operation) => Update(operation);
        public void DeleteOperation (long operationId) => Delete<Operation>(operationId);
        public Operation GetOperation (long operationId) => Select<Operation>(operationId);

        public ExpenseItem AddExpenseItem (ExpenseItem expense) => Add(expense);
        public ExpenseItem UpdateExpenseItem (ExpenseItem expense) => Update(expense);
        public void DeleteExpenseItem (long expenseId) => Delete<ExpenseItem>(expenseId);
        public ExpenseItem GetExpenseItem (long expenseId) => Select<ExpenseItem>(expenseId);

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

        public IList<Operation> GetOperations (DateTime? startTime, DateTime? endTime, long? expenseItemId, long? expenseCategoryId)
        {
            throw new NotImplementedException ();
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

        public IList<Currency> GetCurrencies () => Select<Currency>();
        public IList<Account> GetAccounts () => Select<Account>();

        void IDisposable.Dispose ()
        {
        }
    }
}

