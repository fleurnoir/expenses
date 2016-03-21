using System;
using System.Collections.Generic;
using System.Threading;
using System.Collections.Concurrent;

namespace Expenses.Common.Utils
{
    public class Services
    {
        private static IDictionary<Type,object> m_dictionary;

        private static IDictionary<Type,object> Dictionary
        {
            get 
            {
                if (m_dictionary != null)
                    return m_dictionary;
                Interlocked.CompareExchange (ref m_dictionary, new ConcurrentDictionary<Type,object> (), null);
                return m_dictionary;
            }
        }

        public static void Initialize(bool threadSafe)
        {
            var oldValue = m_dictionary != null ? m_dictionary : 
                Interlocked.CompareExchange (ref m_dictionary, threadSafe ? 
                    (IDictionary<Type,object>) new ConcurrentDictionary<Type,object> () : 
                    new Dictionary<Type,object> (), null);
            if (oldValue != null)
                throw new InvalidOperationException ("Services are already initialized");
        }

        public static bool IsInitialized => m_dictionary != null;

        public static bool IsThreadSafe => Dictionary is ConcurrentDictionary<Type, object>;

        public static TService TryGet<TService>() where TService : class
        {
            var result = Dictionary.SafeGet(typeof(TService));
            return result as TService ?? (result as IServiceProvider<TService>)?.GetService();
        }

        public static TService Get<TService>() where TService : class
        {
            var result = TryGet<TService>();
            if(result == null)
                throw new ArgumentException($"Sevice of type {typeof(TService)} is not registered");
            return result;
        }

        public static void Register<TService>(TService service)
        {
            Dictionary.Add(typeof(TService), service);
        }

        public static void RegisterProvider<TService>(IServiceProvider<TService> provider)
        {
            Dictionary.Add(typeof(TService), provider);
        }
    }
}

