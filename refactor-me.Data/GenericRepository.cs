using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using refactor_me.Data.Interface;

namespace refactor_me.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private IDbContext _dataContext;
        private IDbSet<T> _dbSet;

        protected GenericRepository()
        {

        }

        public GenericRepository(IDbContext dataContext)
        {
            _dataContext = dataContext;
            _dbSet = DataContext.Set<T>();
        }

        public IDbContext DataContext => _dataContext;

        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public T Single(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).Single();
        }

        public T SingleOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).SingleOrDefault();
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).FirstOrDefault();
        }

        public void Add(T entity)
        {
           _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            _dataContext.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> objects = _dbSet.Where(predicate).AsEnumerable();
            foreach (T obj in objects)
            {
                _dbSet.Remove(obj);
            }
        }

        public void SaveChanges()
        {
            DataContext.SaveChanges();
        }

        public void Dispose()
        {
            _dataContext.Dispose();
        }
    }
}
