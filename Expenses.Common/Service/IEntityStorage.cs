using System;
using Expenses.BL.Entities;
using Expenses.BL.Service;
using System.Collections.Generic;
using System.Collections;
using Expenses.Common.Utils;
using System.Linq;

namespace Expenses.Common.Service
{
    public interface IEntityStorage
    {
        TEntity Get<TEntity> (long id) where TEntity : Entity, new();
    }

    public class EntityStorage : IEntityStorage
    {
        protected readonly IExpensesService m_service;

        public EntityStorage(IExpensesService service)
        {
            if (service == null)
                throw new ArgumentNullException (nameof(service));
            m_service = service;
        }

        public virtual TEntity Get<TEntity> (long id) where TEntity : Entity, new()
        {
            return m_service.GetEntityService<TEntity> ().Select (id);
        }
    }

    public class CachedEntityStorage : EntityStorage
    {
        private readonly IDictionary<Type, IEnumerable> m_cache = new Dictionary<Type, IEnumerable>();

        public CachedEntityStorage(IExpensesService service) : base(service){
        }

        public override TEntity Get<TEntity> (long id)
        {
            var cache = m_cache.SafeGet (typeof(TEntity)) as IDictionary<long, TEntity>;
            if (cache == null) {
                cache = m_service.GetEntityService<TEntity> ().Select ().ToDictionary (item=>item.Id);
                m_cache.Add (typeof(TEntity), cache);
            }
            return cache[id];
        }

    }

}

