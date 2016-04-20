using System;
using Expenses.BL.Service;
using Expenses.BL.Entities;
using System.Linq;

namespace Expenses.Web
{
    public class AuthenticationContextProvider : IAuthenticationContextProvider
    {
        private string m_usersDbConnectionString;

        private IDatabaseManager m_databaseManager;

        public AuthenticationContextProvider (string usersDbConnectionString, IDatabaseManager databaseManager)
        {
            if (databaseManager == null)
                throw new ArgumentNullException (nameof(databaseManager));
            if (usersDbConnectionString == null)
                throw new ArgumentNullException (nameof(usersDbConnectionString));
            m_databaseManager = databaseManager;
            m_usersDbConnectionString = usersDbConnectionString;
        }

        public AuthenticationContext CreateContext ()
        {
            return new AuthenticationContext(m_usersDbConnectionString);
        }

        public IDataContextProvider GetProviderForUser (long userId)
        {
            using (var context = CreateContext ()) {
                var user = context.Users.First (u => u.Id == userId);
                if (!m_databaseManager.DatabaseExists (user))
                    m_databaseManager.CreateDatabase (user);
                return DataContextProvider.FromConnectionString (m_databaseManager.GetConnectionString(user));
            }
        }

    }
}

