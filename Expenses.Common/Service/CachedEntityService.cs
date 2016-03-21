using System;
using Expenses.BL.Service;
using System.Collections.Generic;
using Expenses.BL.Entities;
using Expenses.Common.Utils;
using System.Linq;

namespace Expenses.Common.Service
{
    public class CachedEntityService<TEntity> : IEntityService<TEntity> where TEntity : Entity, new()
    {
        private IDictionary<long, TEntity> m_cache;
        private IEntityService<TEntity> m_service;
        private int m_selectCounter;
        private int m_uncachedSelects;

        public CachedEntityService(IEntityService<TEntity> service, int uncachedSelects)
        {
            if (service == null)
                throw new ArgumentNullException (nameof(service));
            m_service = service;
            m_uncachedSelects = uncachedSelects;
        }

        public CachedEntityService(IEntityService<TEntity> service) : this(service, 1)
        {
        }

        public TEntity Add (TEntity item)
        {
            item = m_service.Add (item);
            InvalidateCache ();
            return item;
        }

        public TEntity Update (TEntity item)
        {
            item = m_service.Update (item);
            InvalidateCache ();
            return item;
        }

        public void Delete (long itemId)
        {
            m_service.Delete (itemId);
            InvalidateCache ();
        }

        public TEntity Select (long id)
        {
            if (m_cache == null && m_selectCounter >= m_uncachedSelects)
                m_cache = m_service.Select ().ToDictionary (item => item.Id);
            if (m_cache != null)
                return m_cache.SafeGet (id);
            m_selectCounter++;
            return m_service.Select (id);
        }

        public IList<TEntity> Select ()
        {
            var result = m_service.Select ();
            m_cache = result.ToDictionary (item=>item.Id);
            return result;
        }

        private void InvalidateCache()
        {
            m_cache = null;
            m_selectCounter = 0;
        }
    }
}

