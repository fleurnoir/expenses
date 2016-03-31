using System;
using Expenses.BL.Service;
using Expenses.BL.Entities;
using Expenses.Common.Utils;

namespace Expenses.Web.Common
{
    public static class DefaultsHelper
    {
        private const string Prefix = "Web.Defaults.";

        public static long? GetDefaultId<TEntity>(this IExpensesService service)
        {
            return service.GetValue (Prefix + typeof(TEntity).Name)?.ToInt64();
        }

        public static void SetDefaultId<TEntity>(this IExpensesService service, long? id)
        {
            service.StoreValue (Prefix + typeof(TEntity).Name, id?.ToString());
        }

        public static long? GetDefaultSubcategoryId(this IExpensesService service, long categoryId)
        {
            return service.GetValue ($"{Prefix}{typeof(Subcategory)}.{categoryId}")?.ToInt64();
        }

        public static void SetDefaultSubcategoryId(this IExpensesService service, long categoryId, long? subcategoryId)
        {
            service.StoreValue ($"{Prefix}{typeof(Subcategory)}.{categoryId}", subcategoryId?.ToString());
        }
    }
}

