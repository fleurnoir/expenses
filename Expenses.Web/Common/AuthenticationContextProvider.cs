using System;
using Expenses.BL.Service;
using Expenses.BL.Entities;
using System.Linq;
using System.Data.Common;

namespace Expenses.Web
{
    public class AuthenticationContextProvider : IAuthenticationContextProvider
    {
        private IDatabaseManager m_databaseManager;

        public AuthenticationContextProvider (IDatabaseManager databaseManager)
        {
            if (databaseManager == null)
                throw new ArgumentNullException (nameof(databaseManager));
            m_databaseManager = databaseManager;
        }

        public AuthenticationContext CreateContext ()
        {
            return new AuthenticationContext(m_databaseManager.CreateUsersDbConnection());
        }

        public IDataContextProvider GetProviderForUser (long userId)
        {
            using (var context = CreateContext ()) {
                var user = context.Users.First (u => u.Id == userId);
                if (!m_databaseManager.DatabaseExists (user))
                    m_databaseManager.CreateDatabase (user);
                return DataContextProvider.FromConnectionProvider (m_databaseManager.GetConnectionProvider(user));
            }
        }

    }
}

