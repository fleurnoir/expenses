using System;
using Expenses.BL.Entities;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Security;

namespace Expenses.BL.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        IDataContextProvider m_contextProvider;

        public AuthenticationService (IDataContextProvider contextProvider)
        {
            if (contextProvider == null)
                throw new ArgumentNullException ("contextProvider");
            this.m_contextProvider = contextProvider;
        }

        private string ComputeHash (string password) => Convert.ToBase64String (new SHA512CryptoServiceProvider ()
            .ComputeHash (Encoding.UTF8.GetBytes (password ?? String.Empty)));

        public IExpensesService Login (string login, string password)
        {
            using (var context = m_contextProvider.CreateContext ()) 
            {
                var user = context.Users.FirstOrDefault(u => u.Login == login);
                if (user != null && ComputeHash (password) != user.PasswordHash)
                    return null;
                return new ExpensesService(m_contextProvider, user.Id);
            }
        }

        public User RegisterUser (User user, string password)
        {
            if (user == null)
                throw new ArgumentNullException (nameof(user));
            user.CheckFields ();
            using (var context = m_contextProvider.CreateContext ()) 
            {
                if (context.Users.Any(u => u.Login == user.Login))
                    throw new SecurityException ($"User {user.Login} already exists");
                user.PasswordHash = ComputeHash (password);
                context.Users.Add (user);
                context.SaveChanges ();
                return user;
            }
        }

        void IDisposable.Dispose ()
        {
        }
    }
}

