using Npgsql;
using RepoDb;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BusinessLayer
{
    public class DbRepository : IDisposable
    {
        private NpgsqlConnection connection;
        public NpgsqlTransaction transaction;

        public NpgsqlConnection Connection { get => connection; set => connection = value; }

        public DbRepository(string connectionString)
        {
            this.connection = new NpgsqlConnection(connectionString);
            connection.Open();
            this.transaction = connection.BeginTransaction();
        }

        public IEnumerable<TEntity> Query<TEntity>(Expression<Func<TEntity, bool>> where) where TEntity: class
        {
            return connection.Query<TEntity>(where, transaction: transaction);
        }

        public IEnumerable<TEntity> QueryAll<TEntity>() where TEntity : class
        {
            return connection.QueryAll<TEntity>(transaction: transaction);
        }

        public void Save<TEntity>(TEntity entity) where TEntity : class
        {
            connection.Merge<TEntity>(entity, transaction: transaction);
        }

        public long CountAll<TEntity>() where TEntity : class
        {
            return connection.CountAll<TEntity>(transaction: transaction);
        }

        public void Delete<TEntity>(Guid guid) where TEntity : class
        {
            connection.Delete<TEntity>(guid, transaction: transaction);
        }

        public IEnumerable<TEntity> ExecuteQuery<TEntity>(string commandText, object param = null) where TEntity:class
        {
            return connection.ExecuteQuery<TEntity>(commandText, param:param, transaction: transaction);
        }

        public void Commit()
        {
            this.transaction.Commit();
            this.transaction.Dispose();
            this.transaction = connection.BeginTransaction();
        }

        public void Dispose()
        {
            transaction.Rollback();
            transaction.Dispose();
            connection.Dispose();
        }
    }
}
