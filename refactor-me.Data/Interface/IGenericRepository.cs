using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace refactor_me.Data.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Gets a IIQueryable list of entities for a certain type
        /// </summary>
        /// <param name="includes">optional includes list to help Entity Framework</param>
        /// <returns></returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Gets a list of entities for a certain type based on a filter
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets a single entity for a certain type based on a filter
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        T Single(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets a single entity for a certain type based on a filter
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        T SingleOrDefault(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets a single entity for a certain type based on a filter
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        T FirstOrDefault(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Adds a new entity to the database
        /// </summary>
        /// <param name="entity"></param>
        void Add(T entity);

        /// <summary>
        /// sets the state of the entity to be modified
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);

        /// <summary>
        /// deletes the entity from the database
        /// </summary>
        /// <param name="entity"></param>
        void Delete(T entity);

        /// <summary>
        /// deletes entities from the database based on a filter
        /// </summary>
        /// <param name="where"></param>
        void Delete(Expression<Func<T, Boolean>> predicate);

        /// <summary>
        /// commits all the changes made to the database
        /// </summary>
        void SaveChanges();

        void Dispose();
    }
}
