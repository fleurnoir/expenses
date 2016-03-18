using System;
using Expenses.BL.Entities;

namespace Expenses.BL.Service
{
    public class DataContextProvider : IDataContextProvider
    {
        private string m_nameOrConnectionString;

        private DataContextProvider (string nameOrConnectionString)
        {
            if (String.IsNullOrEmpty(nameOrConnectionString))
                throw new ArgumentNullException ("nameOrConnectionString");
            m_nameOrConnectionString = nameOrConnectionString;
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

        public ExpensesContext CreateContext ()
        {
            return new ExpensesContext(m_nameOrConnectionString);
        }

    }
}

