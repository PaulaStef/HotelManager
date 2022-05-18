using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess
{
    public interface IRepository<TEntity> where TEntity : class
    {
        List<TEntity> GetAll();
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        TEntity GetById(object id);
        IQueryable<TEntity> GetAllQueryable();
    }
}
