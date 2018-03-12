using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace refactor_me.Data.Interface
{
    public interface IDbContext : IDisposable
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();
        ObjectContext GetObjectContext();

        DbEntityEntry<T> Entry<T>(T entity) where T : class;
    }
}
