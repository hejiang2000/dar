using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using log4net;
using System.Reflection;

namespace MES.Biz.Db
{
    public class DbHelper : IDisposable
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        DbProviderFactory _provider;
        string _conn_str;
        DbConnection _conn;
        DbTransaction _trans;

        public DbHelper(string connStr, string providerName)
        {
            _provider = DbProviderFactories.GetFactory(providerName??"System.Data.SqlClient");
            _conn_str = connStr;

            Open();
        }
        protected void Open()
        {
            Close();

            DbConnection conn = _provider.CreateConnection();
            conn.ConnectionString = _conn_str;
            conn.Open();
            _conn = conn;
        }
        protected void Close()
        {
            Rollback();
            if (_conn != null)
            {
                DbConnection conn = _conn;
                _conn = null;
                conn.Close();
            }
        }

        public void BeginTransaction(System.Data.IsolationLevel isolationLevel
            = System.Data.IsolationLevel.Unspecified)
        {
            _trans = _conn.BeginTransaction(isolationLevel);
        }
        public void Commit()
        {
            if (_trans != null)
            {
                DbTransaction trans = _trans;
                _trans = null;
                trans.Commit();
            }
        }
        public void Rollback()
        {
            if (_trans != null)
            {
                DbTransaction trans = _trans;
                _trans = null;
                trans.Rollback();
            }
        }

        public DbCommand CreateCommand(string text, System.Data.CommandType type)
        {
            DbCommand cmd = _conn.CreateCommand();
            cmd.Connection = _conn;
            cmd.CommandText = text;
            cmd.CommandType = type;
            return cmd;
        }

        public void AddDbCommandParameter(DbCommand cmd, string name, System.Data.DbType type, System.Data.ParameterDirection direction, object value)
        {
            DbParameter param = cmd.CreateParameter();
            param.ParameterName = name;
            param.DbType = type;
            param.Direction = direction;
            param.Value = value;
            cmd.Parameters.Add(param);
        }

        public int ExecuteNonQuery(DbCommand cmd)
        {
            try
            {
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw ex;
            }
        }

        public object ExecuteScalar(DbCommand cmd)
        {
            try
            {
                object obj = cmd.ExecuteScalar();
                return obj;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw ex;
            }
        }

        public T[] ExecuteReader<T>(DbCommand cmd, Func<DbDataReader, T> converter)
        {
            try
            {
                using (DbDataReader dr = cmd.ExecuteReader())
                {
                    List<T> list = new List<T>();
                    while (dr.Read())
                    {
                        T t = converter(dr);
                        list.Add(t);
                    }
                    return list.ToArray();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw ex;
            }
        }

        public T[] ExecuteReader<T>(DbCommand cmd) where T : MES.Intf.Entity, new()
        {
            return ExecuteReader<T>(cmd, (dr) =>
            {
                T t = new T();
                for (int i = 0; i < dr.FieldCount; ++i)
                {
                    string name = dr.GetName(i);
                    object value = dr.GetValue(i);
                    t[name] = (value != DBNull.Value ? value : null);
                }
                return t;
            });
        }

        public void Dispose()
        {
            Close();
        }
    }
}
