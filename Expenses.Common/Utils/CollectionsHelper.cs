using System;
using System.Collections.Generic;

namespace Expenses.Common.Utils
{
    public static class CollectionsHelper
    {
        public static TValue SafeGet<TKey, TValue>(this IDictionary<TKey,TValue> source, TKey key) where TValue : class
        {
            TValue value;
            return source.TryGetValue (key, out value) ? value : null;
        }
    }
}

