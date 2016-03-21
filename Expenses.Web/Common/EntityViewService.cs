using System;
using Expenses.Web.Models;
using Expenses.BL.Service;
using Expenses.Common.Utils;
using Expenses.BL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Expenses.Web.Common
{
    public class EntityViewService<TEntity, TView> : IEntityService<TView> where TView : Entity, new() where TEntity : Entity, new()
    {
        IEntityService<TEntity> m_service;

        public EntityViewService (IEntityService<TEntity> service)
        {
            if (service == null)
                throw new ArgumentNullException ("service");
            m_service = service;
        }

        private static TView ToView (TEntity entity) => Cloner.Clone<TEntity,TView>(entity);
        private static TEntity ToEntity (TView view) => Cloner.Clone<TView, TEntity>(view);

        public TView Add (TView item)
        {
            return ToView(m_service.Add(ToEntity(item)));
        }

        public TView Update (TView item)
        {
            return  ToView(m_service.Update (ToEntity(item)));
        }

        public void Delete (long itemId)
        {
            m_service.Delete(itemId);
        }

        public TView Select (long id)
        {
            return ToView(m_service.Select(id));
        }

        public IList<TView> Select ()
        {
            var cloner = Cloner.Get<TEntity,TView> ();
            return m_service.Select().Select(item=> cloner.Clone(item)).ToList();
        }
    }
}

