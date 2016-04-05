using System;
using System.Linq;
using Expenses.BL.Entities;
using System.Data.Entity;

namespace Expenses.BL.Service
{
    public class SubcategoriesService : EntityService<Subcategory>
    {
        public SubcategoriesService(IDataContextProvider provider) : base(provider){}

        public override Subcategory Update (Subcategory item)
        {
            if (item == null)
                throw new ArgumentNullException (nameof(item));
            using (var db = CreateContext ()) {
                var oldCategoryType = GetCategoryType (db, item.Id);
                var newCategoryType = 
                    (from cat in db.Categories 
                     where cat.Id == item.CategoryId 
                     select cat.Type).First ();
                if (oldCategoryType != newCategoryType)
                    throw new InvalidOperationException ("Cannot convert subcategory from expense to income and vice versa");
                db.Entry (item).State = EntityState.Modified;
                db.SaveChanges ();
                return item;
            }
        }

        private static OperationType GetCategoryType (ExpensesContext db, long subcategoryId)
        {
            return (from sub in db.Subcategories
            join cat in db.Categories on sub.CategoryId equals cat.Id
            where sub.Id == subcategoryId
            select cat.Type).First ();
        }

        public OperationType GetCategoryType(long subcategoryId) {
            using (var db = CreateContext ())
                return GetCategoryType (db, subcategoryId);
        }
    }
}

