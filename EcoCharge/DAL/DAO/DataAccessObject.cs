using DAL.DAO;
using BLL.Interfaces;
using DAL.DB;
using Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;

namespace DAL.DAO
{
    public class DataAccessObject<T> : IDataAccessObject<T> where T : AbstractEntity 
    {
        protected AccessDB contextBD;
        private DbContextTransaction dbContextTransaction;

        public DataAccessObject()
        {
            contextBD = new AccessDB();
        }

        public virtual T Save(T entity)
        {
            entity.PrepareSave();

            if (entity.Id == 0)
            {
                contextBD.Set<T>().Add(entity);
                contextBD.Entry(entity).State = EntityState.Added;
            }
            else
            {
                contextBD.Set<T>().Attach(entity);
                contextBD.Entry(entity).State = EntityState.Modified;
            }

            contextBD.SaveChanges();

            return entity;
        }

        public virtual void Delete(T entity)
        {            
            contextBD.Set<T>().Remove(entity);
            contextBD.Entry(entity).State = EntityState.Deleted; // or EntityState.Modified for edit etc.
            contextBD.SaveChanges();
        }

        public virtual T FindById(T entity)
        {
            entity = contextBD.Set<T>().Find(entity.Id);
            return entity;
        }

        public virtual IQueryable<T> GetRepository()
        {
            return contextBD.Set<T>().AsNoTracking().OrderBy(obj => obj.Id);
        }

        public virtual IList<T> ExecuteSqlCommand(string sql)
        {
             var sequenceQueryResult = contextBD.Database.SqlQuery<T>(sql).ToList();

            return sequenceQueryResult.ToList<T>();
        }
        
        public virtual Boolean ExecuteSqlCommand(string sqlStatement, IList<SqlParameter> parameterList)
        {

            IList<NpgsqlParameter> list = new List<NpgsqlParameter>();

            foreach (SqlParameter p in parameterList)
            {
                NpgsqlParameter obj = new NpgsqlParameter(p.ParameterName, p.Value);
                
                list.Add(obj);
            }

            contextBD.Database.ExecuteSqlCommand(sqlStatement, list.ToArray());
            
            return true;

        }

        public virtual IList<object> ExecuteSqlField(string sql)
        {
            var sequenceQueryResult = contextBD.Database.SqlQuery<object>(sql).ToList();

            return sequenceQueryResult.ToList<object>();
        }

        public T Reload(T entity)
        {
            ((IObjectContextAdapter)contextBD).ObjectContext.Detach(entity);
            return contextBD.Set<T>().FirstOrDefault(x => x.Id == entity.Id);
        }

        public void Dispose()
        {
            this.contextBD.Dispose();
        }

        public void StartTransaction()
        {            
            dbContextTransaction = contextBD.Database.BeginTransaction();                    
        }

        public void CommitTransaction()
        {
            if (dbContextTransaction.UnderlyingTransaction != null)
                if (dbContextTransaction.UnderlyingTransaction.Connection.State == System.Data.ConnectionState.Open)
                {
                    dbContextTransaction.Commit();
                    contextBD.Database.Connection.Close();
                }
        }

        public void RollbackTransaction()
        {
            if (dbContextTransaction.UnderlyingTransaction != null)
                if (dbContextTransaction.UnderlyingTransaction.Connection.State == System.Data.ConnectionState.Open)
                {
                    dbContextTransaction.Rollback();
                    contextBD.Database.Connection.Close();
                }
        }
    }
}
