using System;
using System.IO;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace Common
{
    public class WebRequest
    {
        private readonly System.Net.WebRequest _request;
        private Stream _dataStream;
        private string _status;
        public string _header;

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public WebRequest(string url)
        {
            _request = System.Net.WebRequest.Create(url);
        }

        public WebRequest(string url, string method)
            : this(url)
        {
            if (method.Equals("GET") || method.Equals("POST"))
            {
                _request.Method = method;
            }
            else
                throw new Exception("invalid method type");
        }

        public WebRequest(string url, string method, string data)
            : this(url, method)
        {
            string postData = data;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            _request.ContentType = "application/xml";
            _request.ContentLength = byteArray.Length;
            _dataStream = _request.GetRequestStream();
            _dataStream.Write(byteArray, 0, byteArray.Length);
            _dataStream.Close();
        }

        public WebRequest(string url, string method, string data, string setcookies)
            : this(url, method)
        {
            string postData = data;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            _request.ContentType = "application/x-www-form-urlencoded";
            _request.Headers.Add("Cookie", setcookies);
            _request.ContentLength = byteArray.Length;
            _dataStream = _request.GetRequestStream();
            _dataStream.Write(byteArray, 0, byteArray.Length);
            _dataStream.Close();
        }

        public string GetResponse()
        {
            WebResponse response = _request.GetResponse();
            this.Status = ((HttpWebResponse)response).StatusDescription;
            _dataStream = response.GetResponseStream();
            if (_dataStream != null)
            {
                StreamReader reader = new StreamReader(_dataStream);
                string responseFromServer = reader.ReadToEnd();
                reader.Close();
                if (_dataStream != null) _dataStream.Close();
                response.Close();
                return responseFromServer;
            }

            return "";
        }

        public string GetResponseCookie()
        {
            WebResponse response = _request.GetResponse();
            this.Status = ((HttpWebResponse)response).StatusDescription;
            _dataStream = response.GetResponseStream();
            _header = response.Headers["Set-Cookie"];
            if (_dataStream != null)
            {
                StreamReader reader = new StreamReader(_dataStream);
                string responseFromServer = reader.ReadToEnd();
                reader.Close();
                _dataStream.Close();
                response.Close();
                return responseFromServer;
            }

            return "";
        }
    }
}