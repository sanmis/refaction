using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using refactor_me.Data.Interface;

namespace refactor_me.Data
{
    public class PersistanceFactory : IPersistanceFactory
    {
        private RefactorMeDataContext _refactorMeDataContext;

        protected RefactorMeDataContext RefactorMeDataContext
            => _refactorMeDataContext ?? (_refactorMeDataContext = new RefactorMeDataContext());

        public void Dispose()
        {
            _refactorMeDataContext?.Dispose();
        }

        public IGenericRepository<T> BuildRefactorMeRepository<T>() where T : class
        {
            return new GenericRepository<T>(RefactorMeDataContext);
        }

        public IUnitOfWork RefactorMetUnitOfWork()
        {
            return new UnitOfWork(RefactorMeDataContext);
        }
    }
}
