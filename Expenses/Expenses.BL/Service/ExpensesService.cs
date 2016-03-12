using System;
using Expenses.BL.Entities;
using System.Data.Entity;
using System.Linq;

namespace Expenses.BL.Service
{
    public class ExpensesService : IExpensesService
    {
        private IDataContextProvider m_provider;
        private User m_user;

        private DataContext CreateContext() => m_provider.CreateContext();

        public ExpensesService (IDataContextProvider contextProvider, User user)
        {
            if (contextProvider == null)
                throw new ArgumentNullException (nameof (contextProvider));
            if (user == null)
                throw new ArgumentNullException (nameof (user));
            m_provider = contextProvider;
            m_user = user;
        }

        private TEntity Add<TEntity>(TEntity item) where TEntity : class
        {
            using (var context = CreateContext ()) 
            {
                context.Set<TEntity> ().Add (item);
                context.SaveChanges ();
                return item;
            }
        }

        private TEntity Update<TEntity>(TEntity item) where TEntity : class
        {
            using (var context = CreateContext ()) 
            {
                context.Entry (item).State = EntityState.Modified;
                context.SaveChanges ();
                return item;
            }
        }

        private void Delete<TEntity>(int itemId) where TEntity : class, IUnique, new()
        {
            using (var context = CreateContext ()) 
            {
                context.Set<TEntity>().Remove(new TEntity {Id=itemId});
                context.SaveChanges ();
            }
        }

        private TEntity Select<TEntity>(int id) where TEntity : class, IUnique
        {
            using (var context = CreateContext ())
                return context.Set<TEntity> ().FirstOrDefault (item=>item.Id == id);
        }

        public Operation AddOperation(Operation operation)
        {
            operation.UserId = m_user.Id;
            operation.OperationTime = DateTime.Now;
            return Add (operation);
        }

        public Operation UpdateOperation(Operation operation)
        {
            operation.UserId = m_user.Id;
            operation.OperationTime = DateTime.Now;
            return Update (operation);
        }

        public void DeleteOperation (int operationId) => Delete<Operation>(operationId);

        public Operation GetOperation (int operationId) => Select<Operation>(operationId);


        public ExpenseItem AddExpense (ExpenseItem expense) => Add(expense);

        public ExpenseItem UpdateExpense (ExpenseItem expense) => Update(expense);

        public void DeleteExpense (int expenseId) => Delete<ExpenseItem>(expenseId);

        public ExpenseItem GetExpense (int expenseId) => Select<ExpenseItem>(expenseId);


        public ExpenseCategory AddCategory (ExpenseCategory category) => Add(category);

        public ExpenseCategory UpdateCategory (ExpenseCategory category) => Update(category);

        public void DeleteCategory (int categoryId) => Delete<ExpenseCategory>(categoryId);

        public ExpenseCategory GetCategory (int categoryId) => Select<ExpenseCategory>(categoryId);

    }
}

