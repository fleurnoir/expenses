using System;
using Expenses.BL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Expenses.BL.Service
{
    public class CategoriesService : EntityService<Category>
    {
        public CategoriesService(IDataContextProvider provider) : base(provider){
        }

        public override IList<Category> Select ()
        {
            using(var db = CreateContext())
                return db.Categories.OrderBy(c=>c.Name).ToList();
        }

        public IList<Category> Select (CategoryType? categoryType)
        {
            using (var context = CreateContext ()) {
                IQueryable<Category> query = context.Categories;
                if (categoryType != null)
                    query = query.Where (c => c.Type == (CategoryType)categoryType);
                return query.OrderBy(c=>c.Name).ToList ();
            }
        }
    }
}

