using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace refactor_me.Data.Interface
{
    public interface IPersistanceFactory : IDisposable
    {
        /// <summary>
        /// Create a new instance of the generic repository and associates it with the current context so all repositories share the same context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IGenericRepository<T> BuildRefactorMeRepository<T>() where T : class;

        /// <summary>
        /// Creates a new instance of the unit of work with the current context.
        /// </summary>
        /// <returns></returns>
        IUnitOfWork RefactorMetUnitOfWork();

    }
}
