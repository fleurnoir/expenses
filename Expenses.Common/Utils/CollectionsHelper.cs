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

        public static IDictionary<TKey, IList<TValue>> ToOneToManyMap<TSource, TKey, TValue>(
            this IEnumerable<TSource> source, 
            Func<TSource,TKey> keySelector, 
            Func<TSource,TValue> valueSelector)
        {
            var result = new Dictionary<TKey, IList<TValue>> ();
            foreach (var item in source) 
            {
                var key = keySelector (item);
                IList<TValue> list;
                if (!result.TryGetValue (key, out list)) 
                {
                    list = new List<TValue> (1);
                    result.Add (key, list);
                }
                list.Add (valueSelector(item));
            }
            return result;
        }

        public static IDictionary<TKey, IList<TSource>> ToOneToManyMap<TSource, TKey> (
            this IEnumerable<TSource> source, 
            Func<TSource,TKey> keySelector) => source.ToOneToManyMap(keySelector, item=>item);
    }
}

