using System;
using Expenses.BL.Entities;
using System.Collections.Generic;

namespace Expenses.BL.Service
{
    public interface IExpensesService : IDisposable
    {
        Operation AddOperation(Operation operation);
        Operation UpdateOperation(Operation operation);
        void DeleteOperation(long operationId);
        Operation GetOperation(long operationId);
        IList<Operation> GetOperations();

        ExpenseItem AddExpense(ExpenseItem expense);
        ExpenseItem UpdateExpense(ExpenseItem expense);
        void DeleteExpense(long expenseId);
        ExpenseItem GetExpense(long expenseId);
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

