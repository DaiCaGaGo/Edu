using EduSendSms_MobiBank_API;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Services
{
    public class MfsService
    {
        public async Task<int> SendSMSAsync(string url, string user, string password, string phone, string message, string tranId, string brandName, string dataEncode, string sendTime)
        {
            string result = String.Empty;

            try
            {
                IDictionary<string, string> smsCSKH = new Dictionary<string, string>() {
                    { "phone", phone },
                    { "mess", message },
                    { "user", user},
                    { "pass", password },
                    { "tranId", tranId },
                    { "brandName", brandName },
                    { "dataEncode", dataEncode },
                    { "sendTime", sendTime }
                };

                HttpResponseMessage response = await CallAPIAsync(url, JsonConvert.SerializeObject(smsCSKH));

                if (response != null)
                {
                    result = await response.Content.ReadAsStringAsync();
                    EduSendSms_MobiBank_API.XuLy.ghilog("SysSMS", "SendSMS_CSKHAsync [" + tranId + "]" + "[" + result + "]");

                    Dictionary<string, dynamic> dict = ConvertJsonToObject(result);
                    if (dict != null && "1".Equals(dict["code"]))
                    {
                        return 0;
                    }
                    else if (dict != null && !"0".Equals(dict["code"]))
                    {
                        return Convert.ToInt32(dict["code"]);
                    }
                }
            }
            catch (Exception ex)
            {
                XuLy.ghilog("SysSMS", "SendSMS_CSKHAsync [" + tranId + "]" + "[" + ex + "]");
            }

            return -1;
        }

        private async Task<HttpResponseMessage> CallAPIAsync(string url, string content)
        {
            try
            {
                HttpClient http = new HttpClient();
                HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
                httpRequest.Content = new StringContent(content, Encoding.UTF8, "application/json");
                return await http.SendAsync(httpRequest);
            }
            catch (Exception ex)
            {
                XuLy.ghilog("SysSMS", "CallAPIAsync [" + content + "]" + "[" + ex.ToString() + "]");
                return null;
            }
        }

        public static string ReplaceJson(string jsonString)
        {
            if (jsonString.StartsWith("[") || jsonString.EndsWith("]"))
            {
                string stringResult = jsonString.Replace("[", "").Replace("]", "");
                return stringResult;
            }
            return jsonString;
        }

        public static Dictionary<string, dynamic> ConvertJsonToObject(string json)
        {
            try
            {
                if (!String.IsNullOrEmpty(ReplaceJson(json)))
                {
                    return JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(ReplaceJson(json));
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                XuLy.ghilog("SysSMS", "ConvertJsonToObject [" + json + "]" + "[" + e.ToString() + "]");
                return null;
            }
        }
    }
}
