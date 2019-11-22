using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Data
{
    public class Connection
    {
        #region Variables

        private readonly SqlConnection _con;
        private readonly SqlCommand _cmd;
        private SqlDataAdapter _dap;
        private DataSet _dt;
        private readonly string _strcon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        #endregion Variables

        #region Connection

        /// <summary>
        /// Initializes a new instance of the <see cref="Connection"/> class.
        /// </summary>
        protected Connection()
        {
            _con = new SqlConnection(_strcon);
            _cmd = _con.CreateCommand();
        }

        #endregion Connection

        #region Open Connection

        /// <summary>
        /// Opens the connection.
        /// </summary>
        private void OpenConnection()
        {
            if (_con.State != ConnectionState.Open)
                _con.Open();
        }

        #endregion Open Connection

        #region Close Connection

        /// <summary>
        /// Closes the connection.
        /// </summary>
        private void CloseConnection()
        {
            if (_con.State == ConnectionState.Open)
                _con.Close();
        }

        #endregion Close Connection

        #region ExecuteQueryDataSet

        /// <summary>
        /// Executes the query data set.
        /// </summary>
        /// <param name="strSql">The string SQL.</param>
        /// <param name="cmdT">The command t.</param>
        /// <param name="param">The parameter.</param>
        /// <returns></returns>
        protected DataSet ExecuteQueryDataSet(string strSql, CommandType cmdT, params SqlParameter[] param)
        {
            _cmd.Parameters.Clear();

            OpenConnection();
            _cmd.Connection = _con;
            _cmd.CommandTimeout = 200;
            _cmd.CommandType = cmdT;
            _cmd.CommandText = strSql;
            if (param != null)
            {
                _cmd.Parameters.Clear();
                foreach (var p in param)
                {
                    _cmd.Parameters.Add(p);
                }
            }
            _dap = new SqlDataAdapter(_cmd);
            _dt = new DataSet();
            _dap.Fill(_dt);

            CloseConnection();

            return _dt;
        }

        #endregion ExecuteQueryDataSet

        #region MyExecuteNonQuery

        /// <summary>
        /// Mies the execute non query.
        /// </summary>
        /// <param name="strSql">The string SQL.</param>
        /// <param name="cmdT">The command t.</param>
        /// <param name="err">The error.</param>
        /// <param name="param">The parameter.</param>
        /// <returns></returns>
        protected bool MyExecuteNonQuery(string strSql, CommandType cmdT, ref string err, params SqlParameter[] param)
        {
            _cmd.Parameters.Clear();

            bool f = false;
            OpenConnection();

            _cmd.Connection = _con;
            _cmd.CommandTimeout = 200;
            _cmd.Parameters.Clear();
            _cmd.CommandText = strSql;
            _cmd.CommandType = cmdT;
            if (param != null)
            {
                _cmd.Parameters.Clear();
                foreach (SqlParameter p in param)
                {
                    _cmd.Parameters.Add(p);
                }
            }
            try
            {
                err = _cmd.ExecuteNonQuery().ToString();
                f = true;
            }
            catch (SqlException se)
            {
                err = se.Message;
            }
            finally
            {
                CloseConnection();
            }

            return f;
        }

        #endregion MyExecuteNonQuery

        #region MyExecuteNonQuery

        /// <summary>
        /// Mies the execute non query. (return parameter output)
        /// </summary>
        /// <param name="strSql">The string SQL.</param>
        /// <param name="cmdT">The command t.</param>
        /// <param name="err">The error.</param>
        /// <param name="result">The result.</param>
        /// <param name="param">The parameter.</param>
        /// <returns></returns>
        protected bool MyExecuteNonQuery(string strSql, CommandType cmdT, ref string err, ref string result, params SqlParameter[] param)
        {
            _cmd.Parameters.Clear();

            bool f = false;
            OpenConnection();

            _cmd.Connection = _con;
            _cmd.CommandTimeout = 200;
            _cmd.Parameters.Clear();
            _cmd.CommandText = strSql;
            _cmd.CommandType = cmdT;
            if (param != null)
            {
                _cmd.Parameters.Clear();
                foreach (SqlParameter p in param)
                {
                    _cmd.Parameters.Add(p);
                }
            }

            try
            {
                _cmd.ExecuteReader();

                foreach (SqlParameter item in _cmd.Parameters.Cast<SqlParameter>().Where(item => item.Direction == ParameterDirection.Output))
                {
                    result = item.Value.ToString();
                    break;
                }

                f = true;
            }
            catch (SqlException se)
            {
                err = se.Message;
            }
            finally
            {
                CloseConnection();
            }

            return f;
        }

        #endregion MyExecuteNonQuery

        #region MyExecuteScalar

        /// <summary>
        /// Mies the execute scalar.
        /// </summary>
        /// <param name="strSql">The string SQL.</param>
        /// <param name="cmdT">The command t.</param>
        /// <param name="err">The error.</param>
        /// <param name="result">The result.</param>
        /// <param name="param">The parameter.</param>
        /// <returns></returns>
        protected bool MyExecuteScalar(string strSql, CommandType cmdT, ref string err, ref string result, params SqlParameter[] param)
        {
            _cmd.Parameters.Clear();

            bool f;
            OpenConnection();

            _cmd.Connection = _con;
            _cmd.CommandTimeout = 200;
            _cmd.Parameters.Clear();
            _cmd.CommandText = strSql;
            _cmd.CommandType = cmdT;
            if (param != null)
            {
                _cmd.Parameters.Clear();
                foreach (var p in param)
                {
                    _cmd.Parameters.Add(p);
                }
            }
            try
            {
                result = _cmd.ExecuteScalar().ToString();

                f = true;
            }
            catch (SqlException se)
            {
                err = se.Message;
                f = false;
            }
            finally
            {
                CloseConnection();
            }

            return f;
        }

        #endregion MyExecuteScalar

        #region MyExecuteScalar

        /// <summary>
        /// Mies the execute scalar.
        /// </summary>
        /// <param name="strSql">The string SQL.</param>
        /// <param name="cmdT">The command t.</param>
        /// <param name="err">The error.</param>
        /// <param name="result">The result.</param>
        /// <param name="idReturnBack">The identifier return back.</param>
        /// <param name="param">The parameter.</param>
        /// <returns></returns>
        protected bool MyExecuteScalar(string strSql, CommandType cmdT, ref string err, ref string result,
            ref string idReturnBack, params SqlParameter[] param)
        {
            _cmd.Parameters.Clear();

            bool f;
            OpenConnection();

            _cmd.Connection = _con;
            _cmd.CommandTimeout = 200;
            _cmd.Parameters.Clear();
            _cmd.CommandText = strSql;
            _cmd.CommandType = cmdT;
            if (param != null)
            {
                _cmd.Parameters.Clear();
                foreach (var p in param)
                {
                    _cmd.Parameters.Add(p);
                }
            }
            try
            {
                result = _cmd.ExecuteScalar().ToString();

                //Lay parameter output
                foreach (SqlParameter item in _cmd.Parameters)
                {
                    if (item.Direction == ParameterDirection.Output)
                    {
                        idReturnBack = item.Value.ToString();
                        break;
                    }
                }

                f = true;
            }
            catch (SqlException se)
            {
                err = se.Message;
                f = false;
            }
            finally
            {
                CloseConnection();
            }

            return f;
        }

        #endregion MyExecuteScalar
    }
}