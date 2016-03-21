using System;

namespace Expenses.Common.Utils
{
    public interface IServiceProvider<TService>
    {
        TService GetService();
    }
}

