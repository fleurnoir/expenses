using System;
using Expenses.BL.Entities;
using System.Collections.Generic;

namespace Expenses.BL.Service
{
    public interface IEntityService<TEntity> where TEntity : Entity, new()
    {
        TEntity Add(TEntity item);
 
        TEntity Update(TEntity item);

        void Delete(long itemId);

        TEntity Select(long id);

        IList<TEntity> Select();
    }
}

