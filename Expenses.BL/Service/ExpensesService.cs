using System;
using Expenses.BL.Entities;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using Expenses.Common.Utils;
using Expenses.Common.Service;

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
            RegisterFactory<Account> ((provider,userId)=>new AccountsService(provider));
            RegisterFactory<Subcategory> ();
            RegisterFactory<Category> ((provider,userId)=>new CategoriesService(provider));
            RegisterFactory<Operation> ((provider,userId)=>new OperationsService(provider, userId));
            RegisterFactory<Exchange> ((provider,userId)=>new ExchangesService(provider, userId));
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

        public Exchange AddExchange(Exchange operation) => Add(operation);
        public Exchange UpdateExchange(Exchange operation) => Update(operation);
        public void DeleteExchange(long exchangeId) => Delete<Exchange>(exchangeId);
        public Exchange GetExchange(long exchangeId) => Select<Exchange>(exchangeId);
        public IList<Exchange> GetExchanges() => Select<Exchange>();

        public Subcategory AddSubcategory (Subcategory expense) => Add(expense);
        public Subcategory UpdateSubcategory (Subcategory expense) => Update(expense);
        public void DeleteSubcategory (long expenseId) => Delete<Subcategory>(expenseId);
        public Subcategory GetSubcategory (long expenseId) => Select<Subcategory>(expenseId);

        public Category AddCategory (Category category) => Add(category);
        public Category UpdateCategory (Category category) => Update(category);
        public void DeleteCategory (long categoryId) => Delete<Category>(categoryId);
        public Category GetCategory (long categoryId) => Select<Category>(categoryId);

        public Currency AddCurrency(Currency currency) => Add(currency);
        public Currency UpdateCurrency(Currency currency) => Update(currency);
        public void DeleteCurrency(long currencyId) => Delete<Currency>(currencyId);
        public Currency GetCurrency(long currencyId) => Select<Currency>(currencyId);

        public Account AddAccount(Account account) => Add(account);
        public Account UpdateAccount(Account account) => Update(account);
        public void DeleteAccount(long accountId) => Delete<Account>(accountId);
        public Account GetAccount(long accountId) => Select<Account>(accountId);

        public IList<Operation> GetOperations (DateTime? startTime, DateTime? endTime, long? subcategoryId, long? categoryId)
        {
            return ((OperationsService)GetEntityService<Operation>()).Select(startTime, endTime, subcategoryId, categoryId);        
        }

        public IList<StatsItem> GetStatistics (DateTime? startTime = default(DateTime?), DateTime? endTime = default(DateTime?), long? subcategoryId = default(long?), long? categoryId = default(long?))
        {
            return ((OperationsService)GetEntityService<Operation>()).GetStatistics(startTime, endTime, subcategoryId, categoryId);
        }

        public IList<Subcategory> GetSubcategories (long? categoryId = null)
        {
            using (var context = m_provider.CreateContext ()) {
                IQueryable<Subcategory> query = context.Subcategories;
                if (categoryId != null)
                    query = query.Where (c => c.CategoryId == (long)categoryId);
                return query.OrderBy(c=>c.Name).ToList ();
            }
        }

        public IList<Category> GetCategories (CategoryType? categoryType = null)
        {
            return ((CategoriesService)GetEntityService<Category> ()).Select (categoryType);
        }

        public IList<Currency> GetCurrencies () => Select<Currency>();
        public IList<Account> GetAccounts () => Select<Account>();

        void IDisposable.Dispose ()
        {
        }
    }
}

