using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using refactor_me.Data.Interface;
using refactor_me.Data.Mapping;

namespace refactor_me.Data
{
    public class RefactorMeDataContext : DbContext, IDbContext
    {
        public RefactorMeDataContext()
            : base("Name=RefactorMeDataContext")
        {
            Database.SetInitializer<RefactorMeDataContext>(null);
        }

        public RefactorMeDataContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ProductMapping());
            modelBuilder.Configurations.Add(new ProductOptionMapping());
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {

            return base.Set<TEntity>();
        }

        public ObjectContext GetObjectContext()
        {
            return ((IObjectContextAdapter)this).ObjectContext;
        }
    }
}
