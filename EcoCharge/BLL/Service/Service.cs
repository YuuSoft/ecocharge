using BLL.Factory;
using BLL.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BLL.Service
{
    public class Service<T> : IService<T> where T : AbstractEntity
    {
        private IFactory<T> factory;

        public Service()
        {
            factory = new Factory<T>();
        }

        public T Save(T entity)
        {
            try
            {
                factory.StartTransaction();

                entity = factory.Save(entity);

                factory.CommitTransaction();

                return entity;

            } catch (Exception exception)
            {
                factory.RollbackTransaction();

                throw exception;
            }
        }

        public void Delete(T entity)
        {
            try
            {
                factory.StartTransaction();

                factory.Delete(entity);

                factory.CommitTransaction();
            }
            catch (Exception exception)
            {
                factory.RollbackTransaction();

                throw exception;
            }
        }

        public void Dispose()
        {
            factory.Dispose();
        }

        public IList<T> ExecuteSqlCommand(string sql)
        {
            try
            {
                factory.StartTransaction();

                var result = factory.ExecuteSqlCommand(sql);

                factory.CommitTransaction();

                return result;
            }
            catch (Exception exception)
            {
                factory.RollbackTransaction();

                throw exception;
            }
        }

        public bool ExecuteSqlCommand(string sqlStatement, IList<SqlParameter> parameterList)
        {
            try
            {
                factory.StartTransaction();

                var result = factory.ExecuteSqlCommand(sqlStatement, parameterList);

                factory.CommitTransaction();

                return result;
            }
            catch (Exception exception)
            {
                factory.RollbackTransaction();

                throw exception;
            }
        }

        public IList<object> ExecuteSqlField(string sql)
        {
            try
            {
                factory.StartTransaction();

                var result = factory.ExecuteSqlField(sql);

                factory.CommitTransaction();

                return result;
            }
            catch (Exception exception)
            {
                factory.RollbackTransaction();

                throw exception;
            }
        }

        public T FindById(T entity)
        {
            return factory.FindById(entity);
        }

        public IQueryable<T> GetRepository()
        {
            return factory.GetRepository();
        }

        public T Reload(T entity)
        {
            return factory.Reload(entity);
        }
    }
}