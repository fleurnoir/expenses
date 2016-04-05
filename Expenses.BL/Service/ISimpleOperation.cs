using System;
using Expenses.BL.Entities;

namespace Expenses.BL.Service
{
    internal interface ISimpleOperation
    {
        double Amount { get; set; }
        Account GetAccount(ExpensesContext context);
        OperationType GetOperationType(ExpensesContext context);
    }

    internal abstract class SimpleOperation<TOperation> : ISimpleOperation where TOperation : IAmount
    {
        protected TOperation m_operation;
        public SimpleOperation(TOperation operation)
        {
            m_operation = operation;
        }
        public double Amount { get { return m_operation.Amount; } set { m_operation.Amount = value; }}
        public abstract Account GetAccount(ExpensesContext context);
        public abstract OperationType GetOperationType(ExpensesContext context);
    }
}

