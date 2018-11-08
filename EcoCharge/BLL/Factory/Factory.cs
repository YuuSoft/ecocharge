using BLL.Interfaces;
using DAL.DAO;
using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BLL.Factory
{
    public class Factory<T> : IFactory<T> where T : AbstractEntity
    {
        private IDataAccessObject<T> dao;

        public Factory()
        {
            dao = new DataAccessObject<T>();
        }

        public T Save(T entity)
        {
            return dao.Save(entity);
        }

        public void Delete(T entity)
        {
            dao.Delete(entity);
        }

        public void Dispose()
        {
            dao.Dispose();
        }

        public IList<T> ExecuteSqlCommand(string sql)
        {
            return dao.ExecuteSqlCommand(sql);
        }

        public bool ExecuteSqlCommand(string sqlStatement, IList<SqlParameter> parameterList)
        {
            return dao.ExecuteSqlCommand(sqlStatement, parameterList);
        }

        public IList<object> ExecuteSqlField(string sql)
        {
            return dao.ExecuteSqlField(sql);
        }

        public T FindById(T entity)
        {
            return dao.FindById(entity);
        }

        public IQueryable<T> GetRepository()
        {
            return dao.GetRepository();
        }

        public T Reload(T entity)
        {
            return dao.Reload(entity);
        }

        public void StartTransaction()
        {
            dao.StartTransaction();
        }

        public void CommitTransaction()
        {
            dao.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            dao.RollbackTransaction();
        }
    }
}