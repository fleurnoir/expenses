using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Expenses.Common.Utils
{
    public static class Cloner
    {
        public static void Clone<TSource, TDest> (TSource source, TDest dest)
        {
            Cloner<TSource, TDest>.Instance.Clone (source, dest);
        }

        public static TDest Clone<TSource, TDest> (TSource source) where TDest : new()
        {
            return Cloner<TSource, TDest>.Instance.Clone (source);
        }

        public static Cloner<TSource, TDest> Get<TSource,TDest> () => Cloner<TSource, TDest>.Instance;

        public static TDest Clone<TSource, TDest> (this Cloner<TSource, TDest> cloner, TSource source) where TDest : new()
        {
            var result = new TDest ();
            cloner.Clone (source, result);
            return result;
        }
    }

    public class Cloner<TSource, TDest>
    {
        public static readonly Cloner<TSource, TDest> Instance = new Cloner<TSource, TDest>(); 

        //private readonly IReadOnlyList<Tuple<PropertyInfo, PropertyInfo>> m_propertyPairs = GetPairs();

        private readonly Action<TSource, TDest> m_cloner = CreateAction();

        private static IReadOnlyList<Tuple<PropertyInfo, PropertyInfo>> GetPairs()
        {
            var result = new List<Tuple<PropertyInfo, PropertyInfo>> ();
            var destProperties = typeof(TDest).GetPublicInstanceProperties ()
                .Where (p => p.SetMethod != null).ToOneToManyMap (p=>p.Name);
            foreach (var source in 
                typeof(TSource).GetPublicInstanceProperties().Where(p=>p.GetMethod != null)) {
                var candidates = destProperties.SafeGet (source.Name);
                if (candidates == null)
                    continue;
                foreach (var dest in candidates)
                    if (dest.PropertyType.IsAssignableFrom (source.PropertyType)) {
                        result.Add (Tuple.Create (source, dest));
                        break;
                    }
            }
            return result;
        }

        private static Action<TSource,TDest> CreateAction()
        {
            var props = GetPairs ();
            var source = Expression.Parameter (typeof(TSource), "source");
            var dest = Expression.Parameter (typeof(TDest), "dest");
            var body = Expression.Block (
                props.Select (pair =>
                    Expression.Call (dest, pair.Item2.SetMethod, 
                        Expression.Call (source, pair.Item1.GetMethod))));
            return Expression.Lambda<Action<TSource, TDest>> (body, source, dest).Compile ();
        }

        private Cloner(){
        }

        public void Clone(TSource source, TDest dest)
        {
            m_cloner (source, dest);
        }
    }
}

