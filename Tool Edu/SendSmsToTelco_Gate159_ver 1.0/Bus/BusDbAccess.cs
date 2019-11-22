using Data;
using System.Data;

namespace Bus
{
    public class BusDbAccess
    {
        #region Variables

        private readonly DataAccess _dataAccess = new DataAccess();

        #endregion Variables

        #region Gtel

        #region Get data add to queue

        /// <summary>
        /// Gtel_s the get data_ to_ send SMS.
        /// </summary>
        /// <param name="top">The top.</param>
        /// <returns></returns>
        public DataTable Gtel_GetData_To_SendSms(int top)
        {
            return _dataAccess.Gtel_GetData_To_SendSms(top);
        }

        #endregion Get data add to queue

        #region Update data when sent success

        /// <summary>
        /// Gtel_s the update_ when_ success.
        /// </summary>
        /// <param name="tableSuccess">The table success.</param>
        /// <returns></returns>
        public bool Gtel_Update_When_Success(DataTable tableSuccess)
        {
            string error = "";
            return _dataAccess.Gtel_Update_When_Success(tableSuccess, ref error);
        }

        #endregion Update data when sent success

        #region Gtel_Rollback_Error

        /// <summary>
        /// Gtel_s the rollback_ when_ fail.
        /// </summary>
        /// <param name="tableFail">The table fail.</param>
        /// <returns></returns>
        public bool Gtel_Rollback_When_Fail(DataTable tableFail)
        {
            string error = "";
            return _dataAccess.Gtel_Rollback_When_Fail(tableFail, ref error);
        }

        #endregion Gtel_Rollback_Error

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
            return _dataAccess.Incom_GetData_To_SendSms(top);
        }

        #endregion Get data add to queue

        #region Update data when sent success

        /// <summary>
        /// Incom_s the update_ when_ success.
        /// </summary>
        /// <param name="tableSuccess">The table success.</param>
        /// <returns></returns>
        public bool Incom_Update_When_Success(DataTable tableSuccess)
        {
            string error = "";
            return _dataAccess.Incom_Update_When_Success(tableSuccess, ref error);
        }

        #endregion Update data when sent success

        #region Gtel_Rollback_Error

        /// <summary>
        /// Incom_s the rollback_ when_ fail.
        /// </summary>
        /// <param name="tableFail">The table fail.</param>
        /// <returns></returns>
        public bool Incom_Rollback_When_Fail(DataTable tableFail)
        {
            string error = "";
            return _dataAccess.Incom_Rollback_When_Fail(tableFail, ref error);
        }

        #endregion Gtel_Rollback_Error

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
            return _dataAccess.Iris_GetData_To_SendSms(top);
        }

        #endregion Get data add to queue

        #region Update data when sent success

        /// <summary>
        /// Iris_s the update_ when_ success.
        /// </summary>
        /// <param name="tableSuccess">The table success.</param>
        /// <returns></returns>
        public bool Iris_Update_When_Success(DataTable tableSuccess)
        {
            string error = "";
            return _dataAccess.Iris_Update_When_Success(tableSuccess, ref error);
        }

        #endregion Update data when sent success

        #region Gtel_Rollback_Error

        /// <summary>
        /// Iris_s the rollback_ when_ fail.
        /// </summary>
        /// <param name="tableFail">The table fail.</param>
        /// <returns></returns>
        public bool Iris_Rollback_When_Fail(DataTable tableFail)
        {
            string error = "";
            return _dataAccess.Iris_Rollback_When_Fail(tableFail, ref error);
        }

        #endregion Gtel_Rollback_Error

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
            return _dataAccess.Neo_GetData_To_SendSms(top);
        }

        #endregion Get data add to queue

        #region Update data when sent success

        /// <summary>
        /// Neo_s the update_ when_ success.
        /// </summary>
        /// <param name="tableSuccess">The table success.</param>
        /// <returns></returns>
        public bool Neo_Update_When_Success(DataTable tableSuccess)
        {
            string error = "";
            return _dataAccess.Neo_Update_When_Success(tableSuccess, ref error);
        }

        #endregion Update data when sent success

        #region Gtel_Rollback_Error

        /// <summary>
        /// Neo_s the rollback_ when_ fail.
        /// </summary>
        /// <param name="tableFail">The table fail.</param>
        /// <returns></returns>
        public bool Neo_Rollback_When_Fail(DataTable tableFail)
        {
            string error = "";
            return _dataAccess.Neo_Rollback_When_Fail(tableFail, ref error);
        }

        #endregion Gtel_Rollback_Error

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
            return _dataAccess.Vht_GetData_To_SendSms(top);
        }

        #endregion Get data add to queue

        #region Update data when sent success

        /// <summary>
        /// VHT_s the update_ when_ success.
        /// </summary>
        /// <param name="tableSuccess">The table success.</param>
        /// <returns></returns>
        public bool Vht_Update_When_Success(DataTable tableSuccess)
        {
            string error = "";
            return _dataAccess.Vht_Update_When_Success(tableSuccess, ref error);
        }

        #endregion Update data when sent success

        #region Gtel_Rollback_Error

        /// <summary>
        /// VHT_s the rollback_ when_ fail.
        /// </summary>
        /// <param name="tableFail">The table fail.</param>
        /// <returns></returns>
        public bool Vht_Rollback_When_Fail(DataTable tableFail)
        {
            string error = "";
            return _dataAccess.Vht_Rollback_When_Fail(tableFail, ref error);
        }

        #endregion Gtel_Rollback_Error

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
            return _dataAccess.Vivas_GetData_To_SendSms(top);
        }

        #endregion Get data add to queue

        #region Update data when sent success

        /// <summary>
        /// Vivas_s the update_ when_ success.
        /// </summary>
        /// <param name="tableSuccess">The table success.</param>
        /// <returns></returns>
        public bool Vivas_Update_When_Success(DataTable tableSuccess)
        {
            string error = "";
            return _dataAccess.Vivas_Update_When_Success(tableSuccess, ref error);
        }

        #endregion Update data when sent success

        #region Gtel_Rollback_Error

        /// <summary>
        /// Vivas_s the rollback_ when_ fail.
        /// </summary>
        /// <param name="tableFail">The table fail.</param>
        /// <returns></returns>
        public bool Vivas_Rollback_When_Fail(DataTable tableFail)
        {
            string error = "";
            return _dataAccess.Vivas_Rollback_When_Fail(tableFail, ref error);
        }

        #endregion Gtel_Rollback_Error

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
            return _dataAccess.Vmg_GetData_To_SendSms(top);
        }

        #endregion Get data add to queue

        #region Update data when sent success

        /// <summary>
        /// VMG_s the update_ when_ success.
        /// </summary>
        /// <param name="tableSuccess">The table success.</param>
        /// <returns></returns>
        public bool Vmg_Update_When_Success(DataTable tableSuccess)
        {
            string error = "";
            return _dataAccess.Vmg_Update_When_Success(tableSuccess, ref error);
        }

        #endregion Update data when sent success

        #region Gtel_Rollback_Error

        /// <summary>
        /// VMG_s the rollback_ when_ fail.
        /// </summary>
        /// <param name="tableFail">The table fail.</param>
        /// <returns></returns>
        public bool Vmg_Rollback_When_Fail(DataTable tableFail)
        {
            string error = "";
            return _dataAccess.Vmg_Rollback_When_Fail(tableFail, ref error);
        }

        #endregion Gtel_Rollback_Error

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
            return _dataAccess.GetTemplateTelCo(senderName);
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
            return _dataAccess.GetData_To_SendSms(top);
        }

        #endregion Get Sms Push To Queue

        #region Update sms status when send success

        /// <summary>
        /// Update_s the when_ success.
        /// </summary>
        /// <param name="tableSuccess">The table success.</param>
        /// <returns></returns>
        public bool Update_When_Success(DataTable tableSuccess)
        {
            string error = "";
            return _dataAccess.Update_When_Success(tableSuccess, ref error);
        }

        #endregion Update sms status when send success

        #region Rollback sms when send fail

        /// <summary>
        /// Rollback_s the when_ fail.
        /// </summary>
        /// <param name="tableFail">The table fail.</param>
        /// <returns></returns>
        public bool Rollback_When_Fail(DataTable tableFail)
        {
            string error = "";
            return _dataAccess.Rollback_When_Fail(tableFail, ref error);
        }

        #endregion Rollback sms when send fail

        #endregion All
    }
}