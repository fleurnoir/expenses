using System;

namespace Expenses.Common.Utils
{
    public interface IServiceFactory<TService>
    {
        TService CreateService();
    }
}

