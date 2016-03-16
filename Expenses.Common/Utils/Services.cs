using System;
using System.Collections.Concurrent;

namespace Expenses.Common.Utils
{
    public class Services
    {
        private static ConcurrentDictionary<Type,object> m_services = new ConcurrentDictionary<Type,object>();

        public static TService Get<TService>()
        {
            m_services.SafeGet(typeof(TService))
        }
    }
}

