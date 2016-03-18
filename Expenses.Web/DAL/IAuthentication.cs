using System;

namespace Expenses.Web.DAL
{
    public interface IAuthentication
    {
        bool Login(string login, string password);

        void Logout ();

        bool IsLoggedIn { get; }
    }
}

