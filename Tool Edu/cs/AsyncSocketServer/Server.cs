using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
// ReSharper disable All

namespace AsyncSocketServer
{
    /// <summary>
    /// Implements the connection logic for the socket server.  After accepting a connection, all data read
    /// from the client is sent back to the client.  The read and echo back to the client pattern is continued
    /// until the client disconnects.
    /// </summary>
    internal class Server
    {
        private readonly int _mNumConnections;   // the maximum number of connections the sample is designed to handle simultaneously
        public int MReceiveBufferSize { get; private set; }
        private readonly BufferManager _mBufferManager;  // represents a large reusable set of buffers for all socket operations
        private const int OpsToPreAlloc = 2;    // read, write (don't alloc buffer space for accepts)
        private Socket _listenSocket;            // the socket used to listen for incoming connection requests
        private readonly SocketAsyncEventArgsPool _mReadWritePool; // pool of reusable SocketAsyncEventArgs objects for write, read and accept socket operations
        private int _mTotalBytesRead;           // count total # bytes received by the server
        private int _mNumConnectedSockets;      // count total number of clients connected to the server
        private readonly Semaphore _mMaxNumberAcceptedClients; // gioi han so luong client duoc chap nhan ket noi dong thoi.
        private readonly AsyncUserToken[] _listBinded; // danh sach session bind to smpp server
        private int _tokenIdIncrease = 1; // ID nhan dang moi session;
        private readonly Utilities _uti = new Utilities();
        private bool _isLog = true;
        private const int NumberSessionEachAccount = 10; //So luong session duoc phep ket noi vao smpp tren moi tai khoan smpp.
        public Timer TmChecklongtimesendmsg { get; private set; }

        /// <summary>
        /// Create an uninitialized server instance.  To start the server listening for connection requests
        /// call the Init method followed by Start method
        /// </summary>
        /// <param name="numConnections">the maximum number of connections the sample is designed to handle simultaneously</param>
        /// <param name="receiveBufferSize">buffer size to use for each socket I/O operation</param>
        public Server(int numConnections, int receiveBufferSize)
        {
            _mTotalBytesRead = 0; //tong so byte Server da nhan
            _mNumConnectedSockets = 0; //dem so luong socket da ket noi toi server
            _mNumConnections = numConnections;//so ket noi toi da toi server
            MReceiveBufferSize = receiveBufferSize; //dung luong bytes cache cho moi i/o operations.
            _listBinded = new AsyncUserToken[numConnections]; // mang danh sach phien dang hoat dong.
            /*allocate buffers such that the maximum number of sockets can have one outstanding read and
            write posted to the socket simultaneously*/
            _mBufferManager = new BufferManager(receiveBufferSize * _mNumConnections * OpsToPreAlloc, receiveBufferSize);
            _mReadWritePool = new SocketAsyncEventArgsPool(_mNumConnections);
            _mMaxNumberAcceptedClients = new Semaphore(_mNumConnections, _mNumConnections);
            TmChecklongtimesendmsg = new Timer(tm_checklongtimesendmsg_callback, null, 0, 60000 * 60 * 30);
        }

        private void tm_checklongtimesendmsg_callback(object stateinfo)
        {
            int h = DateTime.Now.Hour;
            if (h > 6 && h < 21)
                try
                {
                    string log = _uti.CheckLongtimeSendMsg();
                    //Console.WriteLine(DateTime.Now.ToString() + ", Goi timercallback xu ly canh bao 10 phut Thanh cong." + log);
                    LogApp("Goi timercallback xu ly canh bao 10 phut Thanh cong." + log);
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(DateTime.Now.ToString() + ", Goi timercallback xu ly canh bao 10 phut That bai.");
                    LogApp("Goi timercallback xu ly canh bao 10 phut That bai." + ex.Message);
                }
            try
            {
                LogApp("Xoa cac phien lam viec ko gui enquire link trong 5 phut at: " + DateTime.Now);
                ClearSessionOverTimeNotSendEnquirelink();
            }
            catch (Exception ex)
            {
                //Console.WriteLine(DateTime.Now.ToString() + ", Goi timercallback xu ly canh bao 10 phut That bai.");
                LogApp("Error100:ClearSessionOverTimeNotSendEnquirelink: " + ex.Message);
            }
        }

        /// <summary>
        /// Initializes the server by preallocating reusable buffers and context objects. These objects do not
        /// need to be preallocated or reused, by is done this way to illustrate how the API can easily be used
        /// to create reusable objects to increase server performance.
        /// </summary>
        ///

        public void Init()
        {
            // Allocates one large byte buffer which all I/O operations use a piece of.  This gaurds
            // against memory fragmentation
            _mBufferManager.InitBuffer();
            // preallocate pool of SocketAsyncEventArgs objects
            for (int i = 0; i < _mNumConnections; i++)
            {
                //Pre-allocate a set of reusable SocketAsyncEventArgs object
                var readWriteEventArg = new SocketAsyncEventArgs();
                readWriteEventArg.Completed += IO_Completed;
                readWriteEventArg.UserToken = new AsyncUserToken();
                // assign a byte buffer from the buffer pool to the SocketAsyncEventArg object
                _mBufferManager.SetBuffer(readWriteEventArg);
                // add SocketAsyncEventArg to the pool
                _mReadWritePool.Push(readWriteEventArg);
            }
        }

        /// <summary>
        /// Starts the server such that it is listening for incoming connection requests.
        /// </summary>
        /// <param name="localEndPoint">The endpoint which the server will listening for conenction requests on</param>
        public void Start(IPEndPoint localEndPoint)
        {
            // create the socket which listens for incoming connections
            _listenSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _listenSocket.Bind(localEndPoint);
            // start the server with a listen backlog of 100 connections
            _listenSocket.Listen(_mNumConnections);
            // post accepts on the listening socket
            StartAccept(null);
        }

        public void Stop()
        {
        }

        /// <summary>
        /// Begins an operation to accept a connection request from the client
        /// </summary>
        /// <param name="acceptEventArg">The context object to use when issuing the accept operation on the
        /// server's listening socket</param>
        public void StartAccept(SocketAsyncEventArgs acceptEventArg)
        {
            if (acceptEventArg == null)
            {
                acceptEventArg = new SocketAsyncEventArgs();
                acceptEventArg.Completed += AcceptEventArg_Completed;
            }
            else
            {
                // socket must be cleared since the context object is being reused
                acceptEventArg.AcceptSocket = null;
            }

            _mMaxNumberAcceptedClients.WaitOne();
            bool willRaiseEvent = _listenSocket.AcceptAsync(acceptEventArg);
            if (!willRaiseEvent)
            {
                ProcessAccept(acceptEventArg);
            }
        }

        /// <summary>
        /// This method is the callback method associated with Socket.AcceptAsync operations and is invoked
        /// when an accept operation is complete
        /// </summary>
        private void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            string ip;
            //Get the socket for the accepted client connection and put it into the
            SocketAsyncEventArgs readEventArgs = _mReadWritePool.Pop();
            //ReadEventArg object user token
            AsyncUserToken readEventArgsAut = (AsyncUserToken)readEventArgs.UserToken;
            readEventArgsAut.Socket = e.AcceptSocket;
            try
            {
                ip =
                    IPAddress.Parse(((IPEndPoint) readEventArgsAut.Socket.RemoteEndPoint).Address.ToString()).ToString();
                Interlocked.Increment(ref _mNumConnectedSockets);
                LogApp(
                    string.Format(
                        "a Client connection accepted from ip: {1}. There are {0} clients connected to the server.",
                        _mNumConnectedSockets, ip));
            }
            catch
            {
                //
            }
            //As soon as the client is connected, post a receive to the connection
            bool willRaiseEvent = readEventArgsAut.Socket.ReceiveAsync(readEventArgs);
            if (!willRaiseEvent)
            {
                ProcessReceive(readEventArgs);
            }
            // Accept the next connection request
            StartAccept(e);
        }

        /// <summary>
        /// This method is called whenever a receive or send opreation is completed on a socket
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">SocketAsyncEventArg associated with the completed receive operation</param>
        private void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            // determine which type of operation just completed and call the associated handler
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceive(e);
                    break;

                case SocketAsyncOperation.Send:
                    ProcessSend(e);
                    break;

                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }
        }

        /// <summary>
        /// This method is invoked when an asycnhronous receive operation completes. If the
        /// remote host closed the connection, then the socket is closed.  If data was received then
        /// the data is echoed back to the client.
        /// </summary>
        // ReSharper disable once FunctionComplexityOverflow
        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            AsyncUserToken aut = (AsyncUserToken)e.UserToken;
            //check if the remote host closed the connection
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                Socket s = aut.Socket;
                int byteread = e.BytesTransferred;
                //increment the count of the total bytes receive by the server
                Interlocked.Add(ref _mTotalBytesRead, byteread);
                aut.MLen = aut.CurrentIndex + byteread;

                if (aut.MLen > aut.BufferSize)
                {
                    aut.ClearBuffer();
                }
                else
                {
                    aut.SetData(e);
                    byte[] pduBody = new byte[0];
                    int i, x;
                    x = 0;
                    var exitFlag = false;

                    while (((aut.MLen - x) >= 16) && (exitFlag == false))
                    {
                        int commandLength = aut.Buffer[x + 0];

                        for (i = x + 1; i < x + 4; i++) // dich trai 8 bit sau do or voi phan tu sau trong mang de lay gia tri cua _command_length
                        {
                            commandLength <<= 8;
                            commandLength = commandLength | aut.Buffer[i];
                        }

                        uint commandId = aut.Buffer[x + 4];
                        for (i = x + 5; i < x + 8; i++)
                        {
                            commandId <<= 8;
                            commandId = commandId | aut.Buffer[i];
                        }

                        int commandStatus = aut.Buffer[x + 8];
                        for (i = x + 9; i < x + 12; i++)
                        {
                            commandStatus <<= 8;
                            commandStatus = commandStatus | aut.Buffer[i];
                        }

                        int sequenceNumber = aut.Buffer[x + 12];
                        for (i = x + 13; i < x + 16; i++)
                        {
                            sequenceNumber <<= 8;
                            sequenceNumber = sequenceNumber | aut.Buffer[i];
                        }

                        if ((commandLength <= (aut.MLen - x)) && (commandLength >= 16))
                        {
                            int bodyLength;
                            if (commandLength == 16)
                                bodyLength = 0;
                            else
                            {
                                bodyLength = commandLength - 16;
                                pduBody = new byte[bodyLength];
                                Array.Copy(aut.Buffer, x + 16, pduBody, 0, bodyLength);
                            }

                            //////////////////////////////////////////////////////////////////////////////////////////
                            switch (commandId)
                            {
                                case 0x00000009:
                                    decodeBind_resp(sequenceNumber, pduBody, bodyLength, s, aut, "TR");
                                    break;

                                case 0x00000002:
                                    decodeBind_resp(sequenceNumber, pduBody, bodyLength, s, aut, "T");
                                    break;

                                case 0x00000001:
                                    decodeBind_resp(sequenceNumber, pduBody, bodyLength, s, aut, "R");
                                    break;

                                case 0x00000004:
                                    if (aut.Connectionstate != smpp.ConnectionStates.SmppBinded)
                                    {
                                        Console.WriteLine("{0} Client not binded, close client connection.", aut.Systemid);
                                        CloseClientSocket(e);
                                        return;
                                    }
                                    else
                                    {
                                        DecodeSubmitSm(sequenceNumber, pduBody, bodyLength, aut.Partnerid, aut.Systemid, aut.Tokenid.ToString(), s);
                                    }
                                    break;

                                case 0x80000103:
                                    Console.WriteLine("Data sm res");
                                    break;

                                case 0x80000015:
                                    Console.WriteLine("enquired link respone");
                                    break;

                                case 0x00000015:
                                    LogApp(string.Format("[enquired link resp]: TokenId: {1}, sequence:{0}, PartnerName: {2}, IP: {3}", sequenceNumber, aut.Tokenid, aut.Systemid, aut.PartnerIp));
                                    aut.LastEnquireLink = DateTime.Now;
                                    Send(EnquireLink_respPDU(sequenceNumber, smpp.StatusCodes.EsmeRok), s);
                                    break;

                                case 0x00000006:
                                    LogApp(string.Format("[Unbind Command]: TokenId: {1}, SystemId: {2}, Partner IP: {3}, sequence:{0}", sequenceNumber, aut.Tokenid, aut.Systemid, aut.PartnerIp));
                                    aut.Connectionstate = smpp.ConnectionStates.SmppUnbinded;
                                    unbind_resp_proceess(sequenceNumber, s);
                                    break;

                                case 0x80000005:
                                    Console.WriteLine("Dliver_sm_response");
                                    break;

                                case 0x00000103:
                                    Console.WriteLine("data sm");
                                    break;

                                case 0x80000000:
                                    Console.WriteLine("generic_nack, cmdstatus: {0}, cmdid:{1}, sequence:{2}", commandStatus, commandId, sequenceNumber);
                                    break;

                                default:
                                    Console.WriteLine("unknow pdu type");
                                    Send(GenericNackPdu(sequenceNumber, smpp.StatusCodes.EsmeRinvcmdid), s);
                                    break;
                            }
                            ///////////////////////////////////////////////////////////////////////////////////////////
                            ////END SMPP Command parsing
                            ///////////////////////////////////////////////////////////////////////////////////////////

                            if (commandLength == (aut.MLen - x))
                            {
                                aut.MLen = 0;
                                x = 0;
                                aut.CurrentIndex = 0;
                                exitFlag = true;
                            }
                            else
                            {
                                x += commandLength;
                            }
                        }
                        else
                        {
                            if (sequenceNumber != 0)
                                Send(GenericNackPdu(sequenceNumber, smpp.StatusCodes.EsmeRinvmsglen), s);
                            aut.MLen = aut.MLen - x;
                            aut.CurrentIndex = aut.MLen;
                            Array.Copy(aut.Buffer, x, aut.Buffer, 0, aut.MLen);
                            exitFlag = true;
                            LogApp(string.Format("Error1:cmdLen:{0} cmdid:{1} X:{2} MLen:{3} CuIndex:{4} PartnerId:{5} TokenId:{6} Partnername:{7}", commandLength, commandId, x, aut.MLen, aut.CurrentIndex, aut.Partnerid, aut.Tokenid, aut.Systemid));
                            if (x == 0 && commandLength == 0 && commandId == 0) { aut.CurrentIndex = 0; break; }
                        }
                        //if (x < token.MLen)
                        //    Console.WriteLine("NEXT PDU STEP IN POS {0} FROM {1}", x, token.MLen);
                    }//end while
                    //Console.WriteLine("--------------------------End while-----------------------------");
                }
                aut.ProccessData(e);
                if (!s.ReceiveAsync(e))
                {
                    ProcessReceive(e);
                }
            }//socket error
            else
            {
                LogApp(string.Format("Read 0 bytes or read error from client socket. Client Socket Closed: tokenid:{0}, systemid:{1}, partnerid:{2}.", aut.Tokenid, aut.Systemid, aut.Partnerid));
                CloseClientSocket(e);
            }
        }

        private void CloseClientSocket(SocketAsyncEventArgs e)
        {
            //Decrement the counter keeping track of the total number of clients connected to the server
            Interlocked.Decrement(ref _mNumConnectedSockets);
            AsyncUserToken token = (AsyncUserToken)e.UserToken;
            string logtxt = string.Format("CloseClientSocket: There are {0} clients connected to the server.", _mNumConnectedSockets);
            logtxt += string.Format(" TokenInfo: SystemId:{0}, tokenid:{1}, partnerid:{2}", token.Systemid, token.Tokenid, token.PartnerIp);
            LogApp(logtxt);
            if (!string.IsNullOrEmpty(token.Systemid))
                _uti.SendMail2SystemId(-101, token.Systemid, token.Systemid + "" + token.PartnerIp);
            //Close the socket associated with the client
            try
            {
                token.Socket.Shutdown(SocketShutdown.Send);
                token.Socket.Close();
                token.ClearBufferNull(); //clear buffer truoc khi su dung lai.
            }
            catch (Exception ex) { LogApp("Loi khi shutdown socket: " + ex.Message); }

            ClearSessionByTokenId(token.Tokenid);
            _mMaxNumberAcceptedClients.Release();
            //Free the SocketAsyncEventArg so they can be reused by another client
            _mReadWritePool.Push(e);
        }

        private byte[] GenericNackPdu(int sequenceNumber, int commandStatus)
        {
            byte[] pdu = new byte[16];

            smpp.Tools.CopyIntToArray(16, pdu, 0);

            smpp.Tools.CopyIntToArray(0x80000000, pdu, 4);

            smpp.Tools.CopyIntToArray(commandStatus, pdu, 8);

            smpp.Tools.CopyIntToArray(sequenceNumber, pdu, 12);

            return pdu;
        }//sendGenericNack

        private byte[] EnquireLink_respPDU(int sequenceNumber, int commandStatus)
        {
            byte[] pdu = new byte[16];

            smpp.Tools.CopyIntToArray(16, pdu, 0);

            smpp.Tools.CopyIntToArray(0x80000015, pdu, 4);

            smpp.Tools.CopyIntToArray(commandStatus, pdu, 8);

            smpp.Tools.CopyIntToArray(sequenceNumber, pdu, 12);

            return pdu;
        }//sendGenericNack

        // ReSharper disable once UnusedParameter.Local
        private void decodeBind_resp(int sequenceNumber, byte[] pduBody, int bodyLength, Socket s, AsyncUserToken token, string bindType)
        {
            var _systemid = new byte[17];
            byte[] _password = new byte[10];
            byte[] _systemtype = new byte[14];
            byte interfaceVersion;
            byte addrTon, addrNpi;
            byte[] addresRange = new byte[42];

            int pos = 0;

            int sysidlen = 0;

            while (sysidlen < 16 && pduBody[pos] != 0x00)
            {
                _systemid[sysidlen] = pduBody[pos];
                pos++;
                sysidlen++;
            }

            int pwdlen = 0;
            pos++;
            while (pwdlen < 9 && pduBody[pos] != 0x00)
            {
                _password[pwdlen] = pduBody[pos];
                pos++;
                pwdlen++;
            }
            pos++;
            int systypelen = 0;
            while (systypelen < 9 && pduBody[pos] != 0x00)
            {
                _systemtype[systypelen] = pduBody[pos];
                pos++;
                systypelen++;
            }
            pos++;
            interfaceVersion = pduBody[pos];
            pos++;
            addrTon = pduBody[pos];
            pos++;
            addrNpi = pduBody[pos];

            int arlen = 0;
            while (arlen < 41 && pduBody[pos] != 0x00)
            {
                addresRange[arlen] = pduBody[pos];
                pos++;
                arlen++;
            }

            string systemid, password, systemtype, parterip = "";
            systemid = Encoding.ASCII.GetString(_systemid, 0, sysidlen);
            password = Encoding.ASCII.GetString(_password, 0, pwdlen);
            systemtype = Encoding.ASCII.GetString(_systemtype, 0, systypelen);

            if (s != null)
                parterip = IPAddress.Parse(((IPEndPoint)s.RemoteEndPoint).Address.ToString()).ToString();

            byte[] pdu = new byte[16];
            smpp.Tools.CopyIntToArray(16, pdu, 0);
            if (bindType == "TR")
                smpp.Tools.CopyIntToArray(0x80000009, pdu, 4);
            else if (bindType == "T")
                smpp.Tools.CopyIntToArray(0x80000002, pdu, 4);
            else if (bindType == "R")
                smpp.Tools.CopyIntToArray(0x80000001, pdu, 4);
            int checkresult = _uti.CheckPartnerBind(systemid, password, parterip);
            if (checkresult > 0 && GetSessionOfPartnerId(checkresult) < NumberSessionEachAccount)
            {
                smpp.Tools.CopyIntToArray(0, pdu, 8);
                token.Connectionstate = smpp.ConnectionStates.SmppBinded;
                token.Systemid = systemid;
                token.Partnerid = checkresult;
                token.PartnerIp = parterip;
                token.Bindtype = bindType;
                Interlocked.Add(ref _tokenIdIncrease, 1);
                token.Tokenid = _tokenIdIncrease;

                lock (_listBinded)
                {
                    for (int i = 0; i < _listBinded.Length; i++)
                    {
                        if (_listBinded[i] == null)
                        {
                            _listBinded[i] = token; break;
                        }
                    }
                }
            }
            else
            {
                smpp.Tools.CopyIntToArray(0x0000000D, pdu, 8);
                token.Connectionstate = smpp.ConnectionStates.SmppUnbinded;
            }
            smpp.Tools.CopyIntToArray(sequenceNumber, pdu, 12);
            LogApp(string.Format("[bind_resp] IP connected {6}, systemid {0}, password {1}, systemtype {2}, " +
               "interface_version {3}, _addr_ton {4}, _addr_npi {5}, bin status: {7}, bindType:{8}, bind state: {9} ",
               systemid, password, systemtype, interfaceVersion, addrTon, addrNpi, parterip, checkresult, bindType, token.Connectionstate));

            Send(pdu, s);
        }

        private void DecodeSubmitSm(int sequenceNumber, byte[] body, int bodyLength, int partnerid, string systemid, string tokenid, Socket s)
        {
            try
            {
                #region DecodeSubmit_SM

                bool isDeliveryReceipt = false;
                bool isUdhiSet = false;
                int sourceAddrTon, sourceAddrNpi, destAddrTon, destAddrNpi;
                int priorityFlag, dataCoding, esmClass, protocolId;
                byte[] sourceAddr = new byte[21];
                byte[] destAddr = new byte[21];
                byte[] validityPeriod = new byte[18];
                byte[] scheduleDeliveryTime = new byte[18];
                byte saLength, daLength;
                byte[] shortMessage = new byte[0];
                int smLength;
                int pos;
                int commandstatus = smpp.StatusCodes.EsmeRok;

                pos = 0;

                while ((pos < 5) && (body[pos] != 0x00))
                    pos++;
                if (body[pos] != 0x00)
                {
                    commandstatus = smpp.StatusCodes.EsmeRsubmitfail;
                }

                sourceAddrTon = body[++pos];
                sourceAddrNpi = body[++pos];

                pos += 1;
                saLength = 0;
                while ((saLength < 20) && (body[pos] != 0x00))
                {
                    sourceAddr[saLength] = body[pos];
                    pos++;
                    saLength++;
                }
                if (body[pos] != 0x00)
                {
                    commandstatus = smpp.StatusCodes.EsmeRsubmitfail;
                }

                destAddrTon = body[++pos];
                destAddrNpi = body[++pos];

                pos++;
                daLength = 0;
                while ((daLength < 20) && (body[pos] != 0x00))
                {
                    destAddr[daLength] = body[pos];
                    pos++;
                    daLength++;
                }
                if (body[pos] != 0x00)
                {
                    commandstatus = smpp.StatusCodes.EsmeRsubmitfail;
                }
                esmClass = body[++pos];

                switch (esmClass)
                {
                    case 0x00:
                        break;

                    case 0x04:
                        isDeliveryReceipt = true;
                        break;

                    default:
                        break;
                }

                protocolId = body[++pos];
                priorityFlag = body[++pos];

                pos += 1;
                int sdtlen = 0;

                while ((sdtlen < 17) && (body[pos] != 0x00))
                {
                    scheduleDeliveryTime[sdtlen] = body[pos];
                    pos++;
                    sdtlen++;
                }
                if (body[pos] != 0x00)
                {
                    commandstatus = smpp.StatusCodes.EsmeRsubmitfail;
                }

                pos += 1;
                int vldlen = 0;
                while ((vldlen < 17) && (body[pos] != 0x00))
                {
                    validityPeriod[vldlen] = body[pos];
                    pos++;
                    vldlen++;
                }
                if (body[pos] != 0x00)
                {
                    commandstatus = smpp.StatusCodes.EsmeRsubmitfail;
                }

                pos += 2;
                dataCoding = body[++pos];
                pos += 1;
                smLength = body[++pos];
                pos += 1;
                if (smLength > 0)
                {
                    shortMessage = new byte[smLength];
                    Array.Copy(body, pos, shortMessage, 0, smLength);
                }
                else
                {
                    int tag = body[pos];
                    tag <<= 8;
                    tag = tag | body[++pos];

                    smLength = body[++pos];
                    smLength <<= 8;
                    smLength = smLength | body[++pos];

                    pos++;
                    shortMessage = new byte[smLength];
                    Array.Copy(body, pos, shortMessage, 0, smLength);
                }

                string to;
                string from;
                string textString = "";
                string validity_period;
                validity_period = Encoding.ASCII.GetString(validityPeriod, 0, vldlen);
                bool isUnicode = false, isFlash = false;
                to = Encoding.ASCII.GetString(destAddr, 0, daLength);
                from = Encoding.ASCII.GetString(sourceAddr, 0, saLength);
                string original = "";
                if (dataCoding == 8) //USC2
                {
                    original = Encoding.Unicode.GetString(shortMessage, 0, shortMessage.Length);
                    if (systemid == "sapcom") original = Encoding.BigEndianUnicode.GetString(shortMessage, 0, shortMessage.Length);
                    textString = original;
                }
                else //dcs==3 or 0 Latin 1 or Ascii
                {
                    original = Encoding.UTF8.GetString(shortMessage, 0, shortMessage.Length);
                    textString = _uti.RemoveSpecialCharacters(original);
                }

                isUnicode = smpp.Tools.GetDataCoding(textString) == 8 ? true : false;

                string messid = "0";
                if (commandstatus == smpp.StatusCodes.EsmeRok)
                {
                    messid = _uti.PartnerSmsInsert(partnerid, systemid, from, to, textString, _uti.GetTelco(to), isUnicode, isFlash);
                }
                if (_isLog)
                    LogApp(String.Format("[Submit_sm: Partner:{4}, tokenid:{10}, sequen: {5},messid: {6}], SenderName {0}, Phone {1}, isUnicode: {2}, Message: {3}, dcs:{7}, validity_period:{8}, CommandStatus:{9}",
                        from, to, isUnicode, original, systemid, sequenceNumber, messid, dataCoding, validity_period, commandstatus, tokenid));
                switch (messid)
                {
                    case "-4":
                        commandstatus = smpp.StatusCodes.EsmeRcontentover;
                        break;

                    case "-5":
                        commandstatus = smpp.StatusCodes.EsmeRsmsloop;
                        break;

                    case "-6":
                        commandstatus = smpp.StatusCodes.EsmeRinvsrcadr;
                        break;

                    case "-7":
                        commandstatus = smpp.StatusCodes.EsmeRinvdstadr;
                        break;

                    case "-98":
                        commandstatus = smpp.StatusCodes.EsmeRsyserr;
                        break;

                    case "-99":
                        commandstatus = smpp.StatusCodes.EsmeRsyserr;
                        break;

                    case "-100":
                        commandstatus = smpp.StatusCodes.EsmeRsyserr;
                        break;

                    default:
                        break;
                }

                byte[] mesidbytes = Encoding.ASCII.GetBytes(messid);

                #endregion DecodeSubmit_SM

                #region SendSubmit_response

                int pdulen = 16 + mesidbytes.Length + 1;

                byte[] pdu = new byte[pdulen];

                smpp.Tools.CopyIntToArray(pdulen, pdu, 0);

                smpp.Tools.CopyIntToArray(0x80000004, pdu, 4);

                smpp.Tools.CopyIntToArray(commandstatus, pdu, 8);

                smpp.Tools.CopyIntToArray(sequenceNumber, pdu, 12);

                Array.Copy(mesidbytes, 0, pdu, 16, mesidbytes.Length);

                if (_isLog)
                    LogApp(string.Format("[submit_sm_resp] partner:{2},tokenid:{4}, smsid:{0},cmdstatus: {1},sequence: {3}", messid, commandstatus, systemid, sequenceNumber, tokenid));

                Send(pdu, s);

                #endregion SendSubmit_response
            }
            catch (Exception ex) { LogApp("[decodeSubmitSM] Error:" + ex.Message); }
        }

        private void unbind_resp_proceess(int seq, Socket s)
        {
            byte[] pdu = new byte[16];

            smpp.Tools.CopyIntToArray(16, pdu, 0);

            smpp.Tools.CopyIntToArray(0x80000006, pdu, 4);

            smpp.Tools.CopyIntToArray(0, pdu, 8);

            smpp.Tools.CopyIntToArray(seq, pdu, 12);

            Send(pdu, s);
        }

        private void Send(byte[] pdu, Socket s)
        {
            Send(pdu, pdu.Length, s);
        }

        public void Send(byte[] data, int n, Socket s)
        {
            try
            {
                if (_isLog)
                    LogApp("Sending PDU : " + smpp.Tools.ConvertArrayToHexString(data, n));
                s.BeginSend(data, 0, n, 0, SendCallback, s);
            }
            catch (Exception ex)
            {
                if (_isLog)
                    LogApp("Send Error | " + ex.ToString());
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;
                // Complete sending the data to the remote device.
                int bytesSent = client.EndSend(ar);
                if (_isLog)
                    LogApp("Sent: " + bytesSent.ToString() + " bytes");
            }
            catch (Exception ex)
            {
                if (_isLog)
                    LogApp("Send call back error: | " + ex.ToString());
            }
        }//Send

        /// <summary>
        /// This method is invoked when an asynchronous send operation completes.  The method issues another receive
        /// on the socket to read any additional data sent from the client
        /// </summary>
        /// <param name="e"></param>
        private void ProcessSend(SocketAsyncEventArgs e)
        {
            AsyncUserToken token = (AsyncUserToken)e.UserToken;
            if (e.SocketError == SocketError.Success) // neu gui thanh cong tiep tuc doc data.
            {
                //read the next block of data send from the client
                bool willRaiseEvent = token.Socket.ReceiveAsync(e);
                if (!willRaiseEvent)
                {
                    ProcessReceive(e);
                }
            }  //neu gui loi close va bao loi.
            else
            {
                LogApp(string.Format("ProcessSend Error: tokenid:{0} partnerid:{1} partnername:{2}", token.Tokenid, token.Partnerid, token.Systemid));
                CloseClientSocket(e);
            }
        }

        public int TotalRead
        {
            get { return _mTotalBytesRead; }
        }

        public int ClientConnected
        {
            get { return _mNumConnectedSockets; }
        }

        public string GetInfoClientBinded
        {
            get
            {
                string info = ""; int count = 0;
                if (_listBinded != null)
                {
                    for (int i = 0; i < _listBinded.Length; i++)
                    {
                        if (_listBinded[i] != null && _listBinded[i].Connectionstate == smpp.ConnectionStates.SmppBinded)
                        {
                            count++;
                            info += String.Format("\nPartnerId: {0}, BindType: {1}, IP: {2}, SystemId: {3}, ConnectionState: {4}, TokenId: {5}, LastEnquireLink: {6}",
                                _listBinded[i].Partnerid, _listBinded[i].Bindtype, _listBinded[i].PartnerIp, _listBinded[i].Systemid,
                                _listBinded[i].Connectionstate, _listBinded[i].Tokenid, _listBinded[i].LastEnquireLink);
                        }
                        else continue;
                    }
                }
                return String.Format("Number sessions binded: {0}\nInfo detail: {1}", count, info);
            }
        }

        public void SendDeliverSm()
        {
        }

        //dem so luong session bind to partnerid
        public int GetSessionOfPartnerId(int partnerid)
        {
            int c = 0;
            lock (_listBinded)
            {
                for (int i = 0; i < _listBinded.Length; i++)
                {
                    if (_listBinded[i] != null && _listBinded[i].Partnerid == partnerid && _listBinded[i].Connectionstate == smpp.ConnectionStates.SmppBinded)
                    {
                        c++;
                    }
                }
            }
            return c;
        }

        //Xoa cac phien lam viec lienket voi ParnterID?
        public void ClearSessionByPartnerId(int partnerid)
        {
            int clients = 0; int totalbinded = 0;
            lock (_listBinded)
            {
                for (int i = 0; i < _listBinded.Length; i++)
                {
                    if (_listBinded[i] != null)
                    {
                        if (_listBinded[i].Partnerid == partnerid)
                        {
                            _listBinded[i].Connectionstate = smpp.ConnectionStates.SmppSocketDisconnected;
                            _listBinded[i].ClearBufferNull();
                            try
                            {
                                _listBinded[i].Socket.Close();
                            }
                            catch { }
                            _listBinded[i] = null;
                            clients++;
                        }
                        totalbinded++;
                    }
                    else continue;
                }
            }
            LogApp(string.Format("Number of session be cleared by partnerid({2}): {0}/{1}", clients, totalbinded, partnerid));
        }

        //Xoa cac phien lam viec lien ket voi TokenId
        public void ClearSessionByTokenId(int tokenid)
        {
            int clients = 0; int totalbinded = 0;
            lock (_listBinded)
            {
                for (int i = 0; i < _listBinded.Length; i++)
                {
                    if (_listBinded[i] != null)
                    {
                        if (_listBinded[i].Tokenid == tokenid)
                        {
                            _listBinded[i].Connectionstate = smpp.ConnectionStates.SmppSocketDisconnected;
                            _listBinded[i].ClearBufferNull();
                            try
                            {
                                _listBinded[i].Socket.Close();
                            }
                            catch { }
                            _listBinded[i] = null;
                            clients++;
                        }
                        totalbinded++;
                    }
                    else continue;
                }
            }
            LogApp(string.Format("Number of session be cleared by TokenId:({2}): {0}/{1}", clients, totalbinded, tokenid));
        }

        //Xoa cac phien lam viec ma khong gui enquireLink trong vong 10phut.
        public void ClearSessionOverTimeNotSendEnquirelink()
        {
            int clients = 0; int totalbinded = 0; string partnerIDandTokenId = "";
            lock (_listBinded)
            {
                for (int i = 0; i < _listBinded.Length; i++)
                {
                    if (_listBinded[i] != null)
                    {
                        TimeSpan t = DateTime.Now - _listBinded[i].LastEnquireLink;

                        if (t != null && t.Minutes > 5)
                        {
                            _listBinded[i].Connectionstate = smpp.ConnectionStates.SmppSocketDisconnected;
                            _listBinded[i].ClearBufferNull();
                            partnerIDandTokenId += String.Format("\nPartnerId:{0},PartnerIP:{1},TokenId:{2},SystemId:{3},LastEnquireLink:{4}",
                                _listBinded[i].Partnerid,
                                _listBinded[i].PartnerIp,
                                _listBinded[i].Tokenid,
                                _listBinded[i].Systemid,
                                _listBinded[i].LastEnquireLink);
                            try
                            {
                                _listBinded[i].Socket.Close();
                            }
                            catch { }
                            _listBinded[i] = null;
                            clients++;
                        }
                        totalbinded++;
                    }
                    else continue;
                }
            }
            LogApp(string.Format("Number of session be cleared when not send enquire link over 5 minute: ({0}/{1})\nDetais:\n{2}", clients, totalbinded, partnerIDandTokenId));
        }

        public bool IsLog
        {
            set { _isLog = value; }
            get { return _isLog; }
        }

        private void LogApp(string mes)
        {
            try
            {
                using (System.IO.StreamWriter fstr = new System.IO.StreamWriter(String.Format("SmscLog_{0}.txt", DateTime.Now.ToShortDateString().Replace("/", "-")), true))
                {
                    fstr.WriteLine(String.Format("{0}: {1}", DateTime.Now, mes));
                }
            }
            catch { return; }
        }
    }
}