using System;
using Expenses.BL.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace Expenses.BL.Service
{
    public class EntityService<TEntity> : IEntityService<TEntity> where TEntity : Entity, new() 
    {
        private IDataContextProvider m_provider;
        protected ExpensesContext CreateContext() => m_provider.CreateContext();

        public EntityService (IDataContextProvider contextProvider)
        {
            if (contextProvider == null)
                throw new ArgumentNullException (nameof (contextProvider));
            m_provider = contextProvider;
        }

        public virtual TEntity Add(TEntity item)
        {
            if (item == null)
                throw new ArgumentNullException (nameof(item));
            item.CheckFields ();
            using (var context = CreateContext ()) 
            {
                item = context.Set<TEntity> ().Add (item);
                context.SaveChanges ();
                return item;
            }
        }

        public virtual TEntity Update(TEntity item)
        {
            if (item == null)
                throw new ArgumentNullException (nameof(item));
            item.CheckFields ();
            using (var context = CreateContext ()) 
            {
                context.Entry (item).State = EntityState.Modified;
                context.SaveChanges ();
                return item;
            }
        }

        public virtual void Delete(long itemId)
        {
            using (var context = CreateContext ()) 
            {
                context.Entry(new TEntity {Id=itemId}).State = EntityState.Deleted;
                context.SaveChanges ();
            }
        }

        public virtual TEntity Select(long id)
        {
            using (var context = CreateContext ())
                return context.Set<TEntity> ().FirstOrDefault (item=>item.Id == id);
        }

        public virtual IList<TEntity> Select ()
        {
            using (var context = CreateContext ())
                return context.Set<TEntity> ().ToList();
        }
    }
}

