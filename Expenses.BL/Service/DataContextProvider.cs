using System;
using Expenses.BL.Entities;
using System.Data.Common;

namespace Expenses.BL.Service
{
    public class DataContextProvider : IDataContextProvider
    {
        private string m_nameOrConnectionString;
        private IConnectionProvider m_connectionProvider;

        private DataContextProvider (string nameOrConnectionString)
        {
            if (String.IsNullOrEmpty(nameOrConnectionString))
                throw new ArgumentNullException ("nameOrConnectionString");
            m_nameOrConnectionString = nameOrConnectionString;
        }

        private DataContextProvider (IConnectionProvider connectionProvider)
        {
            if (connectionProvider == null)
                throw new ArgumentNullException ("connectionProvider");
            m_connectionProvider = connectionProvider;
        }

        public static DataContextProvider FromConnectionStringName(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException ("name");
            return new DataContextProvider ($"name={name}");
        }

        public static DataContextProvider FromConnectionString(string connectionString)
        {
            return new DataContextProvider (connectionString);
        }

        public static DataContextProvider FromConnectionProvider(IConnectionProvider connectionProvider)
        {
            return new DataContextProvider (connectionProvider);
        }

        public ExpensesContext CreateContext ()
        {
            if (m_connectionProvider != null)
                return new ExpensesContext (m_connectionProvider.CreateConnection ());
            return new ExpensesContext(m_nameOrConnectionString);
        }

    }
}

