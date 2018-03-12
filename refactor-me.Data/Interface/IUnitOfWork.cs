using System;
using refactor_me.Data.Results;

namespace refactor_me.Data.Interface
{
    public interface IUnitOfWork
    {
        #region Properties
        /// <summary>
        /// Gets a value indicating whether this instance is currently in a transaction.
        /// </summary>
        /// <value><c>true</c> if this instance is currently in a transaction; otherwise, <c>false</c>.</value>
        bool IsInTransaction { get; }
        #endregion

        #region Methods

        /// <summary>
        /// Saves the changes inside the unit of work, optionally using a transcation
        /// </summary>
        /// <param name="action"></param>
        /// <param name="useTransaction"></param>
        /// <returns></returns>
        CommitResult Commit(Action action, bool useTransaction = true);
        #endregion
    }
}
