using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneEduDataAccess
{
    public class DBOracleUtils

    {
        private string TNS = "Data Source=(DESCRIPTION =" +
         "(ADDRESS = (PROTOCOL = TCP)(HOST = 103.1.210.1)(PORT = 1521))" +
         "(CONNECT_DATA =" +
         "(SERVER = DEDICATED)" +
         "(SERVICE_NAME = ORCL)));" +
         "User Id= onedu;Password=onedu123";
        private OracleConnection Con;
        public DataTable GetDataTableByQuery(string strQuery)
        {
            DataTable tab = new DataTable();
            try
            {
                Con = new OracleConnection(TNS);
                Con.Open();
                OracleDataAdapter da = new OracleDataAdapter(strQuery, Con);
                da.Fill(tab);
            }
            catch { }
            finally
            {
                Con.Close();
                Con.Dispose();
            }
            return tab;
        }
        public DataTable GetDataTableByQueryAndParam(string strQuery, List<OracleParameter> lstPa)
        {
            DataTable tab = new DataTable();
            try
            {
                Con = new OracleConnection(TNS);
                Con.Open();
                OracleCommand cmd = new OracleCommand(strQuery, Con);
                cmd.Parameters.Clear();
                cmd.Parameters.AddRange(lstPa.ToArray());
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                da.Fill(tab);
            }
            catch(Exception ex){ }
            finally
            {
                Con.Close();
                Con.Dispose();
            }
            return tab;
        }
    }
}