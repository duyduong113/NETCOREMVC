using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Core
{
    public class SqlHelper
    {
        string connectionString = "";

        public SqlConnection Connection(string connectionString)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            return con;
        }

        private SqlConnection Connection()
        {
            SqlConnection connection = new SqlConnection(this.connectionString);
            if (connection.State != ConnectionState.Open)
                connection.Open();
            return connection;
        }

        protected DbCommand GetCommand(DbConnection connection, string commandText, CommandType commandType)
        {
            SqlCommand command = new SqlCommand(commandText, connection as SqlConnection);
            command.CommandType = commandType;
            return command;
        }

        protected SqlParameter GetParameter(string parameter, object value)
        {
            SqlParameter parameterObject = new SqlParameter(parameter, value != null ? value : DBNull.Value);
            parameterObject.Direction = ParameterDirection.Input;
            return parameterObject;
        }

        protected SqlParameter GetParameterOut(string parameter, SqlDbType type, object value = null, ParameterDirection parameterDirection = ParameterDirection.InputOutput)
        {
            SqlParameter parameterObject = new SqlParameter(parameter, type); ;

            if (type == SqlDbType.NVarChar || type == SqlDbType.VarChar || type == SqlDbType.NText || type == SqlDbType.Text)
            {
                parameterObject.Size = -1;
            }

            parameterObject.Direction = parameterDirection;

            if (value != null)
            {
                parameterObject.Value = value;
            }
            else
            {
                parameterObject.Value = DBNull.Value;
            }

            return parameterObject;
        }

        protected int ExecuteNonQuery(string procedureName, List<DbParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            int returnValue = -1;

            try
            {
                using (SqlConnection connection = this.Connection())
                {
                    DbCommand cmd = this.GetCommand(connection, procedureName, commandType);

                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }

                    returnValue = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                //LogException("Failed to ExecuteNonQuery for " + procedureName, ex, parameters);
                throw;
            }

            return returnValue;
        }

        protected object ExecuteScalar(string procedureName, List<SqlParameter> parameters)
        {
            object returnValue = null;

            try
            {
                using (DbConnection connection = this.Connection())
                {
                    DbCommand cmd = this.GetCommand(connection, procedureName, CommandType.StoredProcedure);

                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }

                    returnValue = cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                //LogException("Failed to ExecuteScalar for " + procedureName, ex, parameters);
                throw;
            }

            return returnValue;
        }

        protected DbDataReader GetDataReader(string procedureName, List<DbParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            DbDataReader ds;

            try
            {
                DbConnection connection = this.Connection();
                {
                    DbCommand cmd = this.GetCommand(connection, procedureName, commandType);
                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }

                    ds = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            catch (Exception ex)
            {
                //LogException("Failed to GetDataReader for " + procedureName, ex, parameters);
                throw;
            }

            return ds;
        }

        //public DataTable SelectQuery(string strSQL)
        //{
        //    DataTable dt = new DataTable();
        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        try
        //        {
        //            SqlCommand cmd = new SqlCommand(strSQL, con);
        //            SqlDataAdapter da = new SqlDataAdapter(cmd);
        //            da.Fill(dt);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    return dt;
        //}

        //public DataTable ExecuteQuery(string spName, List<SqlParameter> listpara)
        //{
        //    DataTable dt = new DataTable();
        //    using (SqlConnection con = Connection(connectionString))
        //    {
        //        try
        //        {
        //            SqlCommand command = con.CreateCommand();
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.CommandText = spName;
        //            if (spName != null)
        //            {
        //                foreach (SqlParameter para in listpara)
        //                {
        //                    command.Parameters.Add(para);
        //                }
        //            }
        //            SqlDataAdapter adapter = new SqlDataAdapter(command);
        //            adapter.Fill(dt);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    return dt;
        //}

        public int ExecuteNoneQuery(string spName, List<SqlParameter> listpara)
        {
            int n = -1;
            using (SqlConnection con = Connection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(spName, con);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 0;
                    if (listpara != null)
                    {
                        foreach (SqlParameter para in listpara)
                            command.Parameters.Add(para);
                    }
                    n = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return n;
        }

        public int ExecuteNoneQuery(string spName, List<SqlParameter> listpara, CommandType t)
        {
            int n = -1;
            using (SqlConnection con = Connection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(spName, con);
                    command.CommandType = t;
                    command.CommandTimeout = 0;
                    if (listpara != null)
                    {
                        foreach (SqlParameter para in listpara)
                            command.Parameters.Add(para);
                    }
                    n = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    con.Close();
                    throw ex;
                }
            }
            return n;
        }

        //public DataTable ExecuteString(string sql, List<SqlParameter> listpara)
        //{
        //    DataTable dt = new DataTable();
        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        try
        //        {
        //            SqlCommand command = con.CreateCommand();
        //            command.CommandType = CommandType.Text;
        //            command.CommandText = sql;
        //            if (sql != null)
        //            {
        //                foreach (SqlParameter para in listpara)
        //                {
        //                    command.Parameters.Add(para);
        //                }
        //            }
        //            SqlDataAdapter adapter = new SqlDataAdapter(command);
        //            adapter.Fill(dt);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    return dt;
        //}

        //public DataSet ExcuteQueryDataSet(string sp, List<SqlParameter> listpara)
        //{
        //    DataSet dts = new DataSet();
        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        try
        //        {
        //            SqlCommand cmd = new SqlCommand(sp, con);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandTimeout = 0;
        //            if (listpara != null)
        //            {
        //                foreach (SqlParameter para in listpara)
        //                    cmd.Parameters.Add(para);
        //            }
        //            SqlDataAdapter da = new SqlDataAdapter(cmd);
        //            da.Fill(dts);
        //        }
        //        catch (System.Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //    return dts;
        //}

        /// <summary>
        /// Thực thi 1 StoreProcedure trả về giá trị
        /// </summary>
        public static object getValueProcWithParameter(string NameStoreProcedure, SqlParameter[] param, SqlConnection connect)
        {
            using (connect)
            {
                object obj = null;
                try
                {
                    SqlCommand sqlCmd = new SqlCommand();
                    sqlCmd.CommandTimeout = 2000;
                    sqlCmd.Connection = connect;
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.CommandText = NameStoreProcedure;
                    obj = sqlCmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return obj;
            }
        }
    }
}
