using System;
using Expenses.BL.Service;
using Expenses.BL.Entities;

namespace Expenses.BL.Service
{
    public interface IAuthenticationContextProvider
    {
        AuthenticationContext CreateContext();
        IDataContextProvider GetProviderForUser(long userId);
    }
}

