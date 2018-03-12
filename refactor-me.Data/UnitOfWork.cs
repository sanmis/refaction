using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using refactor_me.Data.Interface;
using refactor_me.Data.Results;

namespace refactor_me.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private DbTransaction _transaction;

        public UnitOfWork(IDbContext dataContext)
        {
            DbContext = dataContext;
        }

        protected ObjectContext ObjectContext => DbContext.GetObjectContext();

        public IDbContext DbContext { get; }

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this instance is currently in a transaction.
        /// </summary>
        /// <value><c>true</c> if this instance is currently in a transaction; otherwise, <c>false</c>.</value>
        public bool IsInTransaction => _transaction != null;

        #endregion

        #region Methods

        /// <summary>
        /// Begins a new transaction on the unit of work.
        /// </summary>
        /// <param name="isolationLevel">The isolation level.</param>
        /// <exception cref="InvalidOperationException">A transaction is already running.</exception>
        private void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            if (_transaction != null)
            {
                const string error = "Cannot begin a new transaction while an existing transaction is still running. " +
                                     "Please commit or rollback the existing transaction before starting a new one.";

                throw new InvalidOperationException(error);
            }

            OpenConnection();

            _transaction = ObjectContext.Connection.BeginTransaction(isolationLevel);
        }

        /// <summary>
        /// Rolls back all the changes inside a transaction.
        /// </summary>
        /// <exception cref="InvalidOperationException">No transaction is currently running.</exception>
        private void RollBackTransaction()
        {
            if (_transaction == null)
            {
                const string error = "Cannot roll back a transaction when there is no transaction running.";

                throw new InvalidOperationException(error);
            }

            if (IsInTransaction)
            {
                _transaction.Rollback();
                ReleaseCurrentTransaction();
            }
        }

        /// <summary>
        /// Commits all the changes inside a transaction.
        /// </summary>
        /// <exception cref="InvalidOperationException">No transaction is currently running.</exception>
        private void CommitTransaction()
        {
            if (_transaction == null)
            {
                const string error = "Cannot commit a transaction when there is no transaction running.";

                throw new InvalidOperationException(error);
            }

            try
            {
                ObjectContext.SaveChanges();
                _transaction.Commit();
                ReleaseCurrentTransaction();
            }
            catch
            {
                RollBackTransaction();
                throw;
            }
        }


        /// <summary>
        /// Saves the changes inside the unit of work.
        /// </summary>
        /// <exception cref="InvalidOperationException">A transaction is running. Call CommitTransaction instead.</exception>
        private void Commit()
        {
            if (IsInTransaction)
            {
                const string error = "A transaction is running. Call CommitTransaction instead.";

                throw new InvalidOperationException(error);
            }

            ObjectContext.SaveChanges();
        }

        /// <summary>
        /// Opens the connection to the database.
        /// </summary>
        private void OpenConnection()
        {
            if (ObjectContext.Connection.State != ConnectionState.Open)
            {
                ObjectContext.Connection.Open();
            }
        }


        /// <summary>
        /// Releases the current transaction
        /// </summary>
        private void ReleaseCurrentTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        #endregion

        public virtual CommitResult Commit(Action action, bool useTransaction = true)
        {
            try
            {
                if (useTransaction)
                    BeginTransaction();

                action();

                if (IsInTransaction)
                    CommitTransaction();
                else
                    Commit();

                return new CommitResult();
            }
            catch (Exception e)
            {
                if (IsInTransaction)
                    RollBackTransaction();

                return new CommitResult(e);
            }
        }
    }
}
