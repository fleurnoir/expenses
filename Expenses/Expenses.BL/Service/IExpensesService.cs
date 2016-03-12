using System;
using Expenses.BL.Entities;

namespace Expenses.BL.Service
{
    public interface IExpensesService
    {
        Operation AddOperation(Operation operation);
        void UpdateOperation(Operation operation);
        void DeleteOperation(int operationId);
        Operation GetOperation(int operationId);

        ExpenseItem AddExpense(ExpenseItem expense);
        void UpdateExpense(ExpenseItem expense);
        void DeleteExpense(int expenseId);
        ExpenseItem GetExpense(int expenseId);

        ExpenseCategory AddCategory(ExpenseCategory category);
        void UpdateCategory(ExpenseCategory category);
        void DeleteCategory(int categoryId);
        ExpenseCategory GetCategory(int categoryId);
    }
}

