using System;
using System.Data.Entity;

namespace Expenses.BL.Common
{
    public class DbContextTransactionScope : IDisposable
    {
        private readonly DbContextTransaction m_transaction;
        private bool m_completed;

        public DbContextTransactionScope (DbContextTransaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException ("transaction");
            m_transaction = transaction;
        }

        public void Commit()
        {
            if (!m_completed) {
                m_transaction.Commit ();
                m_completed = true;
            }
        }

        public void Dispose()
        {
            if (!m_completed) {
                m_transaction.Rollback ();
                m_completed = true;
            }
        }
    }
}

