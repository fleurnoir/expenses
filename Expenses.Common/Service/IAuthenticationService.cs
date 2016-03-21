using System;
using Expenses.BL.Entities;

namespace Expenses.BL.Service
{
    public interface IAuthenticationService : IDisposable
    {
        IExpensesService Login(string login, string password);
        User RegisterUser (User user, string password);
    }
}

