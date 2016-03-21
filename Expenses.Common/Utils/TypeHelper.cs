using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Expenses.Common.Utils
{
    public static class TypeHelper
    {
        public static IEnumerable<Type> GetTypesHierarchy(this Type type)
        {
            while (type != null) 
            {
                yield return type;
                type = type.BaseType;
            }
        }

        public static IEnumerable<PropertyInfo> GetPublicInstanceProperties(this Type type)
        {
            return type.GetTypesHierarchy().
                SelectMany(t=>t.GetProperties(BindingFlags.Instance | BindingFlags.Public));
        }
    }
}

