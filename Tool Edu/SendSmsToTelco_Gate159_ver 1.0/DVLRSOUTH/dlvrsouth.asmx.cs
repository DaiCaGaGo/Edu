using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Configuration;
using System.IO;
namespace DVLRSOUTH
{
    /// <summary>
    /// Summary description for dlvrsouth
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class dlvrsouth : System.Web.Services.WebService
    {
        private DataSet m_DataSet = new DataSet();
        private DataTable m_Table = new DataTable();

        private SqlCommand m_Cmdsql = new SqlCommand();
        private SqlConnection m_Connsql = null;
        protected string StrConnection_gate = ConfigurationManager.ConnectionStrings["StrGate"].ToString();
     
        public SqlConnection connectgate()
        {
            SqlConnection con = new SqlConnection(StrConnection_gate);
            try
            {
                con.Open();
                con.Close();
            }
            catch
            {
                con = null;
            }
            return con;
        }
        [WebMethod]
        public string dvlrsouth(string smsid,long receivedts, long deliveredts, int status, string user, string from, string to, string text,int errorcode)
        {
            try { 
            m_Connsql = connectgate();
            DataSet ds = new DataSet();
            if (m_Connsql != null)
            {
                m_Connsql.Open();
                m_Cmdsql.Parameters.Clear();
                m_Cmdsql.Connection = m_Connsql;
                m_Cmdsql.CommandText = "SP_SOUTH_DVLR";
                m_Cmdsql.CommandTimeout = m_Connsql.ConnectionTimeout;
                m_Cmdsql.CommandType = System.Data.CommandType.StoredProcedure;
                m_Cmdsql.Parameters.Add(new SqlParameter("@smsid", SqlDbType.NVarChar));
                m_Cmdsql.Parameters["@smsid"].Value = smsid;

                m_Cmdsql.Parameters.Add(new SqlParameter("@receivedts", SqlDbType.NVarChar));
                m_Cmdsql.Parameters["@receivedts"].Value = receivedts.ToString();

                m_Cmdsql.Parameters.Add(new SqlParameter("@deliveredts", SqlDbType.NVarChar));
                m_Cmdsql.Parameters["@deliveredts"].Value = deliveredts.ToString();

                m_Cmdsql.Parameters.Add(new SqlParameter("@status", SqlDbType.Int));
                m_Cmdsql.Parameters["@status"].Value = status;

                m_Cmdsql.Parameters.Add(new SqlParameter("@user", SqlDbType.NVarChar));
                m_Cmdsql.Parameters["@user"].Value = user;

                m_Cmdsql.Parameters.Add(new SqlParameter("@from", SqlDbType.NVarChar));
                m_Cmdsql.Parameters["@from"].Value = from;

                m_Cmdsql.Parameters.Add(new SqlParameter("@to", SqlDbType.NVarChar));
                m_Cmdsql.Parameters["@to"].Value = to;

                m_Cmdsql.Parameters.Add(new SqlParameter("@text", SqlDbType.NVarChar));
                m_Cmdsql.Parameters["@text"].Value = text;

                m_Cmdsql.Parameters.Add(new SqlParameter("@errorcode", SqlDbType.Int));
                m_Cmdsql.Parameters["@errorcode"].Value = errorcode;
                SqlDataAdapter da = new SqlDataAdapter(m_Cmdsql);
                da.Fill(ds);
                m_Connsql.Close();
            }
            else
            {
                ds = null;
            }
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["0"].ToString() == "0")
                    return "DONE";
                
            }
            return "FAIL";
            }catch(Exception e)
            {
                return "FAIL|"+e.ToString();

            }
        }
    }
}
