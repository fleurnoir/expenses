﻿using System;
using Expenses.BL.Entities;
using System.Collections.Generic;
using Expenses.Common.Service;

namespace Expenses.BL.Service
{
    public interface IExpensesService : IDisposable
    {
        IEntityService<TEntity> GetEntityService<TEntity> () where TEntity : Entity, new(); 

        Operation AddOperation(Operation operation);
        Operation UpdateOperation(Operation operation);
        void DeleteOperation(long operationId);
        Operation GetOperation(long operationId);
        IList<Operation> GetOperations (DateTime? startTime = null, DateTime? endTime = null, long? subcategoryId = null, long? categoryId = null, long? accountId = null);
        IList<StatsItem> GetStatistics (DateTime? startTime = null, DateTime? endTime = null, long? subcategoryId = null, long? categoryId = null, long? accountId = null);

        Exchange AddExchange(Exchange operation);
        Exchange UpdateExchange(Exchange operation);
        void DeleteExchange(long exchangeId);
        Exchange GetExchange(long exchangeId);
        IList<Exchange> GetExchanges();

        Subcategory AddSubcategory(Subcategory expense);
        Subcategory UpdateSubcategory(Subcategory expense);
        void DeleteSubcategory(long expenseId);
        Subcategory GetSubcategory(long expenseId);
        IList<Subcategory> GetSubcategories(long? categoryId = null);
        OperationType GetCategoryType (long subcategoryId);

        Category AddCategory(Category category);
        Category UpdateCategory(Category category);
        void DeleteCategory(long categoryId);
        Category GetCategory(long categoryId);
        IList<Category> GetCategories (OperationType? type = null);

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

        Debt AddDebt(Debt debt);
        Debt UpdateDebt(Debt debt);
        void DeleteDebt(long debtId);
        Debt GetDebt(long debtId);
        IList<Debt> GetDebts (DebtType? type = null);

        Repayment AddRepayment(Repayment repayment);
        Repayment UpdateRepayment(Repayment repayment);
        void DeleteRepayment(long repaymentId);
        Repayment GetRepayment(long repaymentId);
        IList<Repayment> GetRepayments (long debtId);

        string GetValue (string key);
        void StoreValue (string key, string value);
    }
}

