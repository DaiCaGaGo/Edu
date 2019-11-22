using System;
using System.Net.Sockets;

namespace AsyncSocketServer
{
    /// <summary>
    /// This class is designed for use as the object to be assigned to the SocketAsyncEventArgs.UserToken property.
    /// </summary>
    internal class AsyncUserToken
    {
        private Socket _mSocket;
        public int Tokenid;
        public String Systemid;
        public int Connectionstate;
        public int Partnerid;
        public string PartnerIp;
        public string Bindtype;
        public DateTime LastEnquireLink;
        private const int Buffersize = 1048576;
        private int _mLen;
        private byte[] _receivebuffer;
        private int _currentIndex;

        public AsyncUserToken()
            : this(null)
        {
        }

        public AsyncUserToken(Socket socket)
        {
            _mSocket = socket;
            _receivebuffer = new byte[Buffersize];
            LastEnquireLink = DateTime.Now;
        }

        public AsyncUserToken(Socket socket, int buffersize)
        {
            _mSocket = socket;
            _receivebuffer = new byte[buffersize];
            LastEnquireLink = DateTime.Now;
        }

        public Socket Socket
        {
            get { return _mSocket; }
            set { _mSocket = value; }
        }

        public void ProccessData(SocketAsyncEventArgs args)
        {
            args.SetBuffer(_receivebuffer, 0, Buffersize);
        }

        public void SetData(SocketAsyncEventArgs args)
        {
            Array.Copy(args.Buffer, args.Offset, _receivebuffer, _currentIndex, args.BytesTransferred);
        }

        public int CurrentIndex
        {
            get { return _currentIndex; }
            set { _currentIndex = value; }
        }

        public int MLen
        {
            get { return _mLen; }
            set { _mLen = value; }
        }

        public byte[] Buffer
        {
            get { return _receivebuffer; }
        }

        public void ClearBuffer()
        {
            _mLen = 0; _currentIndex = 0; _receivebuffer = new byte[Buffersize];
        }

        public void ClearBufferNull()
        {
            _mLen = 0; _currentIndex = 0; _receivebuffer = new byte[Buffersize]; Tokenid = 0; Partnerid = 0; Systemid = null; PartnerIp = null;
        }

        public int BufferSize
        {
            get { return Buffersize; }
        }

        #region IDisposable Members

        /// <summary>
        /// Release instance.
        /// </summary>
        public void Dispose()
        {
            try
            {
                _mSocket.Shutdown(SocketShutdown.Send);
                ClearBufferNull();
            }
            catch (Exception)
            {
                // Throw if client has closed, so it is not necessary to catch.
            }
            finally
            {
                _mSocket.Close();
            }
        }

        #endregion IDisposable Members
    }
}