using System.Data;
using System.Data.SqlClient;

namespace Data
{
    public class DataAccess : Connection
    {
        #region Gtel

        #region Get data add to queue

        /// <summary>
        /// Gtel_s the get data_ to_ send SMS.
        /// </summary>
        /// <param name="top">The top.</param>
        /// <returns></returns>
        public DataTable Gtel_GetData_To_SendSms(int top)
        {
            DataTable tableResult = new DataTable();

            var parTop = new SqlParameter("@top", top);

            DataSet ds = ExecuteQueryDataSet("sp_gtel_get_sms_to_send", CommandType.StoredProcedure, parTop);
            if (ds != null && ds.Tables.Count > 0)
                tableResult = ds.Tables[0];

            return tableResult;
        }

        #endregion Get data add to queue

        #region Update data when sent success

        /// <summary>
        /// Gtel_s the update_ when_ success.
        /// </summary>
        /// <param name="tableSuccess">The table success.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        public bool Gtel_Update_When_Success(DataTable tableSuccess, ref string error)
        {
            var parTableStatus = new SqlParameter("@tableStatus", tableSuccess);

            var result = MyExecuteNonQuery("sp_gtel_update_status_when_success", CommandType.StoredProcedure, ref error, parTableStatus);

            return result;
        }

        #endregion Update data when sent success

        #region Rollback when sent error

        /// <summary>
        /// Gtel_s the rollback_ when_ fail.
        /// </summary>
        /// <param name="tableFail">The table fail.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        public bool Gtel_Rollback_When_Fail(DataTable tableFail, ref string error)
        {
            var parTableStatus = new SqlParameter("@tableData", tableFail);

            var result = MyExecuteNonQuery("sp_gtel_update_fail", CommandType.StoredProcedure, ref error, parTableStatus);

            return result;
        }

        #endregion Rollback when sent error

        #endregion Gtel

        #region Incom

        #region Get data add to queue

        /// <summary>
        /// Incom_s the get data_ to_ send SMS.
        /// </summary>
        /// <param name="top">The top.</param>
        /// <returns></returns>
        public DataTable Incom_GetData_To_SendSms(int top)
        {
            DataTable tableResult = new DataTable();

            var parTop = new SqlParameter("@top", top);

            DataSet ds = ExecuteQueryDataSet("sp_incom_get_sms_to_send", CommandType.StoredProcedure, parTop);
            if (ds != null && ds.Tables.Count > 0)
                tableResult = ds.Tables[0];

            return tableResult;
        }

        #endregion Get data add to queue

        #region Update data when sent success

        /// <summary>
        /// Incom_s the update_ when_ success.
        /// </summary>
        /// <param name="tableSuccess">The table success.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        public bool Incom_Update_When_Success(DataTable tableSuccess, ref string error)
        {
            var parTableStatus = new SqlParameter("@tableStatus", tableSuccess);

            var result = MyExecuteNonQuery("sp_incom_update_status_when_success", CommandType.StoredProcedure, ref error, parTableStatus);

            return result;
        }

        #endregion Update data when sent success

        #region Rollback when sent error

        /// <summary>
        /// Incom_s the rollback_ when_ fail.
        /// </summary>
        /// <param name="tableFail">The table fail.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        public bool Incom_Rollback_When_Fail(DataTable tableFail, ref string error)
        {
            var parTableStatus = new SqlParameter("@tableData", tableFail);

            var result = MyExecuteNonQuery("sp_incom_update_fail", CommandType.StoredProcedure, ref error, parTableStatus);

            return result;
        }

        #endregion Rollback when sent error

        #endregion Incom

        #region IRIS

        #region Get data add to queue

        /// <summary>
        /// Iris_s the get data_ to_ send SMS.
        /// </summary>
        /// <param name="top">The top.</param>
        /// <returns></returns>
        public DataTable Iris_GetData_To_SendSms(int top)
        {
            DataTable tableResult = new DataTable();

            var parTop = new SqlParameter("@top", top);

            DataSet ds = ExecuteQueryDataSet("sp_iris_get_sms_to_send", CommandType.StoredProcedure, parTop);
            if (ds != null && ds.Tables.Count > 0)
                tableResult = ds.Tables[0];

            return tableResult;
        }

        #endregion Get data add to queue

        #region Update data when sent success

        /// <summary>
        /// Iris_s the update_ when_ success.
        /// </summary>
        /// <param name="tableSuccess">The table success.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        public bool Iris_Update_When_Success(DataTable tableSuccess, ref string error)
        {
            var parTableStatus = new SqlParameter("@tableStatus", tableSuccess);

            var result = MyExecuteNonQuery("sp_iris_update_status_when_success", CommandType.StoredProcedure, ref error, parTableStatus);

            return result;
        }

        #endregion Update data when sent success

        #region Rollback when sent error

        /// <summary>
        /// Iris_s the rollback_ when_ fail.
        /// </summary>
        /// <param name="tableFail">The table fail.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        public bool Iris_Rollback_When_Fail(DataTable tableFail, ref string error)
        {
            var parTableStatus = new SqlParameter("@tableData", tableFail);

            var result = MyExecuteNonQuery("sp_iris_update_fail", CommandType.StoredProcedure, ref error, parTableStatus);

            return result;
        }

        #endregion Rollback when sent error

        #endregion IRIS

        #region NEO

        #region Get data add to queue

        /// <summary>
        /// Neo_s the get data_ to_ send SMS.
        /// </summary>
        /// <param name="top">The top.</param>
        /// <returns></returns>
        public DataTable Neo_GetData_To_SendSms(int top)
        {
            DataTable tableResult = new DataTable();

            var parTop = new SqlParameter("@top", top);

            DataSet ds = ExecuteQueryDataSet("sp_neo_get_sms_to_send", CommandType.StoredProcedure, parTop);
            if (ds != null && ds.Tables.Count > 0)
                tableResult = ds.Tables[0];

            return tableResult;
        }

        #endregion Get data add to queue

        #region Update data when sent success

        /// <summary>
        /// Neo_s the update_ when_ success.
        /// </summary>
        /// <param name="tableSuccess">The table success.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        public bool Neo_Update_When_Success(DataTable tableSuccess, ref string error)
        {
            var parTableStatus = new SqlParameter("@tableStatus", tableSuccess);

            var result = MyExecuteNonQuery("sp_neo_update_status_when_success", CommandType.StoredProcedure, ref error, parTableStatus);

            return result;
        }

        #endregion Update data when sent success

        #region Rollback when sent error

        /// <summary>
        /// Neo_s the rollback_ when_ fail.
        /// </summary>
        /// <param name="tableFail">The table fail.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        public bool Neo_Rollback_When_Fail(DataTable tableFail, ref string error)
        {
            var parTableStatus = new SqlParameter("@tableData", tableFail);

            var result = MyExecuteNonQuery("sp_neo_update_fail", CommandType.StoredProcedure, ref error, parTableStatus);

            return result;
        }

        #endregion Rollback when sent error

        #endregion NEO

        #region VHT

        #region Get data add to queue

        /// <summary>
        /// VHT_s the get data_ to_ send SMS.
        /// </summary>
        /// <param name="top">The top.</param>
        /// <returns></returns>
        public DataTable Vht_GetData_To_SendSms(int top)
        {
            DataTable tableResult = new DataTable();

            var parTop = new SqlParameter("@top", top);

            DataSet ds = ExecuteQueryDataSet("sp_vht_get_sms_to_send", CommandType.StoredProcedure, parTop);
            if (ds != null && ds.Tables.Count > 0)
                tableResult = ds.Tables[0];

            return tableResult;
        }

        #endregion Get data add to queue

        #region Update data when sent success

        /// <summary>
        /// VHT_s the update_ when_ success.
        /// </summary>
        /// <param name="tableSuccess">The table success.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        public bool Vht_Update_When_Success(DataTable tableSuccess, ref string error)
        {
            var parTableStatus = new SqlParameter("@tableStatus", tableSuccess);

            var result = MyExecuteNonQuery("sp_vht_update_status_when_success", CommandType.StoredProcedure, ref error, parTableStatus);

            return result;
        }

        #endregion Update data when sent success

        #region Rollback when sent error

        /// <summary>
        /// VHT_s the rollback_ when_ fail.
        /// </summary>
        /// <param name="tableFail">The table fail.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        public bool Vht_Rollback_When_Fail(DataTable tableFail, ref string error)
        {
            var parTableStatus = new SqlParameter("@tableData", tableFail);

            var result = MyExecuteNonQuery("sp_vht_update_fail", CommandType.StoredProcedure, ref error, parTableStatus);

            return result;
        }

        #endregion Rollback when sent error

        #endregion VHT

        #region VIVAS

        #region Get data add to queue

        /// <summary>
        /// Vivas_s the get data_ to_ send SMS.
        /// </summary>
        /// <param name="top">The top.</param>
        /// <returns></returns>
        public DataTable Vivas_GetData_To_SendSms(int top)
        {
            DataTable tableResult = new DataTable();

            var parTop = new SqlParameter("@top", top);

            DataSet ds = ExecuteQueryDataSet("sp_vivas_get_sms_to_send", CommandType.StoredProcedure, parTop);
            if (ds != null && ds.Tables.Count > 0)
                tableResult = ds.Tables[0];

            return tableResult;
        }

        #endregion Get data add to queue

        #region Update data when sent success

        /// <summary>
        /// Vivas_s the update_ when_ success.
        /// </summary>
        /// <param name="tableSuccess">The table success.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        public bool Vivas_Update_When_Success(DataTable tableSuccess, ref string error)
        {
            var parTableStatus = new SqlParameter("@tableStatus", tableSuccess);

            var result = MyExecuteNonQuery("sp_vivas_update_status_when_success", CommandType.StoredProcedure, ref error, parTableStatus);

            return result;
        }

        #endregion Update data when sent success

        #region Rollback when sent error

        /// <summary>
        /// Vivas_s the rollback_ when_ fail.
        /// </summary>
        /// <param name="tableFail">The table fail.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        public bool Vivas_Rollback_When_Fail(DataTable tableFail, ref string error)
        {
            var parTableStatus = new SqlParameter("@tableData", tableFail);

            var result = MyExecuteNonQuery("sp_vivas_update_fail", CommandType.StoredProcedure, ref error, parTableStatus);

            return result;
        }

        #endregion Rollback when sent error

        #endregion VIVAS

        #region VMG

        #region Get data add to queue

        /// <summary>
        /// VMG_s the get data_ to_ send SMS.
        /// </summary>
        /// <param name="top">The top.</param>
        /// <returns></returns>
        public DataTable Vmg_GetData_To_SendSms(int top)
        {
            DataTable tableResult = new DataTable();

            var parTop = new SqlParameter("@top", top);

            DataSet ds = ExecuteQueryDataSet("sp_vmg_get_sms_to_send", CommandType.StoredProcedure, parTop);
            if (ds != null && ds.Tables.Count > 0)
                tableResult = ds.Tables[0];

            return tableResult;
        }

        #endregion Get data add to queue

        #region Update data when sent success

        /// <summary>
        /// VMG_s the update_ when_ success.
        /// </summary>
        /// <param name="tableSuccess">The table success.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        public bool Vmg_Update_When_Success(DataTable tableSuccess, ref string error)
        {
            var parTableStatus = new SqlParameter("@tableStatus", tableSuccess);

            var result = MyExecuteNonQuery("sp_vmg_update_status_when_success", CommandType.StoredProcedure, ref error, parTableStatus);

            return result;
        }

        #endregion Update data when sent success

        #region Rollback when sent error

        /// <summary>
        /// VMG_s the rollback_ when_ fail.
        /// </summary>
        /// <param name="tableFail">The table fail.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        public bool Vmg_Rollback_When_Fail(DataTable tableFail, ref string error)
        {
            var parTableStatus = new SqlParameter("@tableData", tableFail);

            var result = MyExecuteNonQuery("sp_vmg_update_fail", CommandType.StoredProcedure, ref error, parTableStatus);

            return result;
        }

        #endregion Rollback when sent error

        #endregion VMG

        #region All

        #region GetTemplateTelCo

        /// <summary>
        /// Gets the template tel co.
        /// </summary>
        /// <param name="senderName">Name of the sender.</param>
        /// <returns></returns>
        public DataTable GetTemplateTelCo(string senderName)
        {
            DataTable tableResult = new DataTable();

            var parSender = new SqlParameter("@sender", senderName);

            DataSet ds = ExecuteQueryDataSet("sp_get_template_by_sender", CommandType.StoredProcedure, parSender);
            if (ds != null && ds.Tables.Count > 0)
                tableResult = ds.Tables[0];

            return tableResult;
        }

        #endregion GetTemplateTelCo

        #region Get Sms Push To Queue

        /// <summary>
        /// Gets the data_ to_ send SMS.
        /// </summary>
        /// <param name="top">The top.</param>
        /// <returns></returns>
        public DataTable GetData_To_SendSms(int top)
        {
            DataTable tableResult = new DataTable();

            var parTop = new SqlParameter("@top", top);

            DataSet ds = ExecuteQueryDataSet("sp_all_get_sms_push_to_queue", CommandType.StoredProcedure, parTop);
            if (ds != null && ds.Tables.Count > 0)
                tableResult = ds.Tables[0];

            return tableResult;
        }

        #endregion Get Sms Push To Queue

        #region Update sms status when send success

        /// <summary>
        /// Update_s the when_ success.
        /// </summary>
        /// <param name="tableSuccess">The table success.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        public bool Update_When_Success(DataTable tableSuccess, ref string error)
        {
            var parTableStatus = new SqlParameter("@tableStatus", tableSuccess);

            var result = MyExecuteNonQuery("sp_all_update_success", CommandType.StoredProcedure, ref error, parTableStatus);

            return result;
        }

        #endregion Update sms status when send success

        #region Rollback sms when send fail

        /// <summary>
        /// Rollback_s the when_ fail.
        /// </summary>
        /// <param name="tableFail">The table fail.</param>
        /// <param name="error">The error.</param>
        /// <returns></returns>
        public bool Rollback_When_Fail(DataTable tableFail, ref string error)
        {
            var parTableStatus = new SqlParameter("@tableData", tableFail);

            var result = MyExecuteNonQuery("sp_all_rollback_fail", CommandType.StoredProcedure, ref error, parTableStatus);

            return result;
        }

        #endregion Rollback sms when send fail

        #endregion All
    }
}