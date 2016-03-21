using System;
using Expenses.BL.Service;
using Expenses.Common.Utils;

namespace Expenses.Web.Common
{
    public interface IAuthentication : IServiceProvider<IExpensesService>
    {
        bool Login(string login, string password);

        void Logout ();

        bool IsLoggedIn { get; }
    }
}

