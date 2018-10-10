using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BLL.Interfaces
{
    public interface IService<T> : IDisposable where T : AbstractEntity
    {
        T Save(T entity);
        void Delete(T entity);
        IQueryable<T> GetRepository();
        T FindById(T entity);
        IList<T> ExecuteSqlCommand(string sql);
        Boolean ExecuteSqlCommand(string sqlStatement, IList<SqlParameter> parameterList);
        IList<object> ExecuteSqlField(string sql);
        T Reload(T entity);
    }
}