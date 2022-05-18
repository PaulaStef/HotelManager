using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly WeatherDbContext _dbContext;
        private DbSet<TEntity> dbSet;
        public GenericRepository(WeatherDbContext dbContext)
        {
            this._dbContext = dbContext;
            this.dbSet = _dbContext.Set<TEntity>();
        }
        public void Add(TEntity entity)
        {
            _dbContext.Add(entity);
        }

        public void Update(TEntity entity)
        {
            dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            dbSet.Remove(entityToDelete);
        }

        public void Delete(TEntity entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }
            _dbContext.Remove(entity);
        }

        public List<TEntity> GetAll()
        {
            return dbSet.ToList();
        }

        public IQueryable<TEntity> GetAllQueryable()
        {
            return dbSet.AsQueryable();
        }

        public virtual TEntity GetById(object id)
        {
            return dbSet.Find(id);
        }
    }
}
