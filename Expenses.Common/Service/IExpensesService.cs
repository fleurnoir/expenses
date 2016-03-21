using System;
using Expenses.BL.Entities;
using System.Collections.Generic;

namespace Expenses.BL.Service
{
    public interface IExpensesService : IDisposable
    {
        IEntityService<TEntity> GetEntityService<TEntity> () where TEntity : Entity, new(); 

        Operation AddOperation(Operation operation);
        Operation UpdateOperation(Operation operation);
        void DeleteOperation(long operationId);
        Operation GetOperation(long operationId);
        IList<Operation> GetOperations (DateTime? startTime, DateTime? endTime, long? expenseItemId, long? expenseCategoryId);

        ExpenseItem AddExpenseItem(ExpenseItem expense);
        ExpenseItem UpdateExpenseItem(ExpenseItem expense);
        void DeleteExpenseItem(long expenseId);
        ExpenseItem GetExpenseItem(long expenseId);
        IList<ExpenseItem> GetExpenseItems(long? expenseCategoryId = null);

        ExpenseCategory AddCategory(ExpenseCategory category);
        ExpenseCategory UpdateCategory(ExpenseCategory category);
        void DeleteCategory(long categoryId);
        ExpenseCategory GetCategory(long categoryId);
        IList<ExpenseCategory> GetCategories (ExpenseCategoryType? categoryType = null);

        Currency AddCurrency(Currency currency);
        Currency UpdateCurrency(Currency currency);
        void DeleteCurrency(long currencyId);
        Currency GetCurrency(long currencyId);
        IList<Currency> GetCurrencies ();

        Account AddAccount(Account account);
        Account UpdateAccount(Account account);
        void DeleteAccount(long accountId);
        Account GetAccount(long accountId);
        IList<Account> GetAccounts ();
    }
}

