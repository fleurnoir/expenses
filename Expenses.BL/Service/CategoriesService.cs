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
    }
}

