﻿using System;
using Expenses.BL.Entities;

namespace Expenses.BL.Service
{
    public interface IExpensesService
    {
        Operation AddOperation(Operation operation);
        Operation UpdateOperation(Operation operation);
        void DeleteOperation(int operationId);
        Operation GetOperation(int operationId);

        ExpenseItem AddExpense(ExpenseItem expense);
        ExpenseItem UpdateExpense(ExpenseItem expense);
        void DeleteExpense(int expenseId);
        ExpenseItem GetExpense(int expenseId);

        ExpenseCategory AddCategory(ExpenseCategory category);
        ExpenseCategory UpdateCategory(ExpenseCategory category);
        void DeleteCategory(int categoryId);
        ExpenseCategory GetCategory(int categoryId);

        Currency AddCurrency(Currency currency);
        Currency UpdateCurrency(Currency currency);
        void DeleteCurrency(int currencyId);
        Currency GetCurrency(int currencyId);

        Account AddAccount(Account account);
        Account UpdateAccount(Account account);
        void DeleteAccount(int accountId);
        Account GetAccount(int accountId);
    }
}
