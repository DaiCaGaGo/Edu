using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using AsyncSocketClient.smpp;

//Implements a sample socket client to connect to the server implmented in the AsyncSocketServer project
//Usage: AsyncSocketClient.exe <destination IP address> <destination port number>

//Destination IP address: The IP Address of the server to connect to
//Destination Port Number: The port number to connect to

namespace AsyncSocketClient
{
    internal class Program
    {
        private static readonly ManualResetEvent ClientDone = new ManualResetEvent(false);
        private static Socket _sock;
        private static int _mPos;
        private static int _mLen;
        private static byte[] _mbResponse = new byte[KernelParameters.MaxBufferSize];
        private static int _sequennumber = 1;

        private static void Main(string[] args)
        {
            string systemid = "";
            string password = "";
            string phone = "";
            Console.Write("Nhap systemid:");
            systemid = Console.ReadLine();
            Console.Write("Nhap password:");
            password = Console.ReadLine();

            //args = new string[] { "103.1.210.1", "8888" };
            args = new string[] { "123.30.59.159", "8088" };
            IPAddress destinationAddr = null;          // IP Address of server to connect to
            int destinationPort = 0;                   // Port number of server

            if (args.Length != 2)
            {
                Console.WriteLine("Usage: AsyncSocketClient.exe <destination IP address> <destination port number>");
            }
            try
            {
                destinationAddr = IPAddress.Parse(args[0]);
                destinationPort = int.Parse(args[1]);
                if (destinationPort <= 0)
                {
                    throw new ArgumentException("Destination port number provided cannot be less than or equal to 0");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Usage: AsyncSocketClient.exe <destination IP address> <destination port number>");
            }
            _sock = new Socket(destinationAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            // Create a socket and connect to the server

            SocketAsyncEventArgs connectEventArg = new SocketAsyncEventArgs();
            connectEventArg.Completed += SocketEventArg_Completed;
            connectEventArg.RemoteEndPoint = new IPEndPoint(destinationAddr, destinationPort);
            connectEventArg.UserToken = _sock;
            _sock.ConnectAsync(connectEventArg);
            ClientDone.WaitOne();

            string red = "";
            while (red != "q")
            {
                Console.WriteLine("Enter send message [{0}], bt to bind tranceiver, ub to unbind, q to exit", _sequennumber);
                red = Console.ReadLine();
                byte[] pdu = new byte[0];

                if (red == "bt") //bind tranceiver.
                {
                    pdu = GetPduBindTranceiver(systemid, password);
                }
                else if (red == "ub")
                {
                    pdu = GetPdUunBindTranceiver(_sequennumber);
                }
                else if (red == "")
                //submit_sm
                {
                    phone = "84972828429";
                    red = "Test msg, Time: " + DateTime.Now + " .....";
                    pdu = PduSubmitSm(_sequennumber, 1, 1, "1SMS.VN", 1, 1, phone, 1, 1, 1, DateTime.Now, DateTime.Now, 1, 1, 3, 0, Encoding.ASCII.GetBytes(red));
                    _sequennumber++;
                }
                else { break; }
                // Begin sending the data to the remote device.
                SocketAsyncEventArgs writeEventArg = new SocketAsyncEventArgs();
                writeEventArg.Completed += SocketEventArg_Completed;
                writeEventArg.RemoteEndPoint = new IPEndPoint(destinationAddr, destinationPort);
                writeEventArg.UserToken = _sock;
                writeEventArg.SetBuffer(pdu, 0, pdu.Length);
                bool willRaiseEvent = _sock.SendAsync(writeEventArg);
                ClientDone.WaitOne();
                if (!willRaiseEvent)
                {
                    ProcessSend(writeEventArg);
                }
            }
            _sock.Shutdown(SocketShutdown.Both);
            _sock.Close();
            Console.WriteLine("Client closed");
            Console.ReadKey();
        }

        private static byte[] GetPduBindTranceiver(string systemid, string password)
        {
            byte[] bindPdu = new byte[1024];
            int pos = 0; int i = 0; int n = 0;

            pos = 7;
            bindPdu[pos] = 0x09;

            pos = 12;
            Tools.CopyIntToArray(128, bindPdu, pos);// sequence_number
            pos = 15;

            pos++;
            n = systemid.Length;
            for (i = 0; i < n; i++, pos++)
                bindPdu[pos] = (byte)systemid[i];//system_id
            bindPdu[pos] = 0x00;

            pos++;
            n = password.Length;
            for (i = 0; i < n; i++, pos++)
                bindPdu[pos] = (byte)password[i];//password
            bindPdu[pos] = 0x00;

            pos++;
            //n = 1;
            //for (i = 0; i < n; i++, pos++)
            //    Bind_PDU[pos] = (byte)" "[i];
            bindPdu[pos] = 0x00;

            bindPdu[++pos] = 0x34; //interface version
            bindPdu[++pos] = (byte)1; //addr_ton
            bindPdu[++pos] = (byte)1; //addr_npi

            //address_range
            pos++;
            //n = 1;
            //for (i = 0; i < n; i++, pos++)
            //    Bind_PDU[pos] = (byte)" "[i];
            bindPdu[pos] = 0x00;

            pos++;
            bindPdu[3] = Convert.ToByte(pos & 0x00FF);
            bindPdu[2] = Convert.ToByte((pos >> 8) & 0x00FF);
            return bindPdu;
        }

        /// <summary>
        /// A single callback is used for all socket operations. This method forwards execution on to the correct handler
        /// based on the type of completed operation
        /// </summary>
        private static void SocketEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Connect:
                    ProcessConnect(e);
                    break;

                case SocketAsyncOperation.Receive:
                    ProcessReceive(e);
                    break;

                case SocketAsyncOperation.Send:
                    ProcessSend(e);
                    break;

                default:
                    throw new Exception("Invalid operation completed");
            }
        }

        /// <summary>
        /// Called when a ConnectAsync operation completes
        /// </summary>
        private static void ProcessConnect(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                Console.WriteLine("Successfully connected to the server");
                Thread.Sleep(1000);
                ClientDone.Set();
            }
            else
            {
                throw new SocketException((int)e.SocketError);
            }
        }

        /// <summary>
        /// Called when a SendAsync operation completes
        /// </summary>
        private static void ProcessSend(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                //Read data sent from the server
                Socket sock = e.UserToken as Socket;
                bool willRaiseEvent = sock.ReceiveAsync(e);
                if (!willRaiseEvent)
                {
                    ProcessReceive(e);
                }
            }
            else
            {
                throw new SocketException((int)e.SocketError);
            }
        }

        /// <summary>
        /// Called when a ReceiveAsync operation completes
        /// </summary>
        private static void ProcessReceive(SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                //Console.WriteLine("\nReceived from server: {0} bytes", e.BytesTransferred);
                int commandLength;
                uint commandId;
                int commandStatus;
                int sequenceNumber;
                int bodyLength;
                byte[] pduBody = new byte[0];
                bool exitFlag;
                int i, x;

                if (e.BytesTransferred > 0)
                {
                    _mLen = _mPos + e.BytesTransferred;
                    if (_mLen > KernelParameters.MaxBufferSize)
                    {
                        _mLen = 0;
                        _mPos = 0;
                        _mbResponse = new byte[KernelParameters.MaxBufferSize];
                    }
                    else
                    {
                        Array.Copy(e.Buffer, e.Offset, _mbResponse, _mPos, e.BytesTransferred);
                        x = 0;
                        exitFlag = false;
                        while ((_mLen - x) >= 16 && (exitFlag == false))
                        {
                            commandLength = _mbResponse[x + 0];
                            for (i = x + 1; i < x + 4; i++)
                            {
                                commandLength <<= 8;
                                commandLength = commandLength | _mbResponse[i];
                            }

                            commandId = _mbResponse[x + 4];
                            for (i = x + 5; i < x + 8; i++)
                            {
                                commandId <<= 8;
                                commandId = commandId | _mbResponse[i];
                            }

                            commandStatus = _mbResponse[x + 8];
                            for (i = x + 9; i < x + 12; i++)
                            {
                                commandStatus <<= 8;
                                commandStatus = commandStatus | _mbResponse[i];
                            }

                            sequenceNumber = _mbResponse[x + 12];
                            for (i = x + 13; i < x + 16; i++)
                            {
                                sequenceNumber <<= 8;
                                sequenceNumber = sequenceNumber | _mbResponse[i];
                            }
                            if ((commandLength <= (_mLen - x)) && (commandLength >= 16))
                            {
                                if (commandLength == 16)
                                    bodyLength = 0;
                                else
                                {
                                    bodyLength = commandLength - 16;
                                    pduBody = new byte[bodyLength];
                                    Array.Copy(_mbResponse, x + 16, pduBody, 0, bodyLength);
                                }
                                //////////////////////////////////////////////////////////////////////////////////////////
                                ///SMPP Command parsing

                                switch (commandId)
                                {
                                    case 0x00000009:
                                        Console.WriteLine("Bind_Transiver");
                                        break;

                                    case 0x80000009:
                                        Console.WriteLine("Bind_Transiver_Resp, Command status: {0}", commandStatus);
                                        break;

                                    case 0x80000002:
                                        Console.WriteLine("Bind_Transmitter_Resp");
                                        break;

                                    case 0x80000004:
                                        Console.WriteLine("submit_sm_resp Command status: {0}, messid: {1}, sequennumber {2}.", commandStatus, Encoding.ASCII.GetString(pduBody), sequenceNumber);
                                        break;

                                    case 0x80000103:
                                        Console.WriteLine("Data sm res");
                                        break;

                                    case 0x80000015:
                                        Console.WriteLine("enquired link respone");
                                        break;

                                    case 0x00000015:
                                        Console.WriteLine("enquired link");
                                        break;

                                    case 0x80000006:
                                        Console.WriteLine("unbinded");
                                        break;

                                    case 0x00000005:
                                        Console.WriteLine("Dliver sm");
                                        break;

                                    case 0x00000103:
                                        Console.WriteLine("data sm");
                                        break;

                                    case 0x80000000:
                                        Console.WriteLine("generic_nack command status:{0} sequen {1}.", commandStatus, sequenceNumber);
                                        break;

                                    default:
                                        Console.WriteLine("unknow pdu type:" + Tools.ConvertUIntToHexString(commandId));
                                        break;
                                }
                                ///////////////////////////////////////////////////////////////////////////////////////////
                                ///END SMPP Command parsing
                                ///////////////////////////////////////////////////////////////////////////////////////////
                                if (commandLength == (_mLen - x))
                                {
                                    _mLen = 0;
                                    _mPos = 0;
                                    x = 0;
                                    exitFlag = true;
                                }
                                else
                                {
                                    x += commandLength;
                                }
                            }
                            else
                            {
                                _mLen -= x;
                                _mPos = _mLen;
                                Array.Copy(_mbResponse, x, _mbResponse, 0, _mLen);
                                exitFlag = true;
                                Console.WriteLine("Invalid PDU Length");
                            }
                            //if (x < mLen)
                            //    Console.WriteLine("NEXT PDU STEP IN POS " + Convert.ToString(x) + " FROM " + Convert.ToString(mLen));
                        }//end while
                    }
                }
                //Data has now been sent and received from the server. Disconnect from the server
                //Socket sock = e.UserToken as Socket;
                //sock.Shutdown(SocketShutdown.Both);
                //sock.Close();
                ProcessSend(e);
                ClientDone.Set();
            }
            else
            {
                throw new SocketException((int)e.SocketError);
            }
        }

        public static byte[] PduSubmitSm(int seq, byte sourceAddressTon, byte sourceAddressNpi, string sourceAddress,
                            byte destinationAddressTon, byte destinationAddressNpi, string destinationAddress,
                            byte esmClass, byte protocolId, byte priorityFlag,
                            DateTime sheduleDeliveryTime, DateTime validityPeriod, byte registeredDelivery,
                            byte replaceIfPresentFlag, byte dataCoding, byte smDefaultMsgId,
                            byte[] message)
        {
            byte[] destinationAddr;
            byte[] sourceAddr;
            byte[] submitSmPdu;
            byte[] _shedule_delivery_time;
            byte[] _validity_period;
            int sequenceNumber;
            int pos;
            byte smLength;

            submitSmPdu = new byte[KernelParameters.MaxPduSize];

            ////////////////////////////////////////////////////////////////////////////////////////////////
            /// Start filling PDU

            Tools.CopyIntToArray(0x00000004, submitSmPdu, 4);
            sequenceNumber = seq;
            Tools.CopyIntToArray(sequenceNumber, submitSmPdu, 12);
            pos = 16;
            submitSmPdu[pos] = 0x00; //service_type
            pos += 1;
            submitSmPdu[pos] = sourceAddressTon;
            pos += 1;
            submitSmPdu[pos] = sourceAddressNpi;
            pos += 1;
            sourceAddr = Tools.ConvertStringToByteArray(Tools.GetString(sourceAddress, 20, ""));
            Array.Copy(sourceAddr, 0, submitSmPdu, pos, sourceAddr.Length);
            pos += sourceAddr.Length;
            submitSmPdu[pos] = 0x00;
            pos += 1;
            submitSmPdu[pos] = destinationAddressTon;
            pos += 1;
            submitSmPdu[pos] = destinationAddressNpi;
            pos += 1;
            destinationAddr = Tools.ConvertStringToByteArray(Tools.GetString(destinationAddress, 20, ""));
            Array.Copy(destinationAddr, 0, submitSmPdu, pos, destinationAddr.Length);
            pos += destinationAddr.Length;
            submitSmPdu[pos] = 0x00;
            pos += 1;
            submitSmPdu[pos] = esmClass;
            pos += 1;
            submitSmPdu[pos] = protocolId;
            pos += 1;
            submitSmPdu[pos] = priorityFlag;
            pos += 1;
            _shedule_delivery_time = Tools.ConvertStringToByteArray(Tools.GetDateString(sheduleDeliveryTime));
            Array.Copy(_shedule_delivery_time, 0, submitSmPdu, pos, _shedule_delivery_time.Length);
            pos += _shedule_delivery_time.Length;
            submitSmPdu[pos] = 0x00;
            pos += 1;
            _validity_period = Tools.ConvertStringToByteArray(Tools.GetDateString(validityPeriod));
            Array.Copy(_validity_period, 0, submitSmPdu, pos, _validity_period.Length);
            pos += _validity_period.Length;
            submitSmPdu[pos] = 0x00;
            pos += 1;
            submitSmPdu[pos] = registeredDelivery;
            pos += 1;
            submitSmPdu[pos] = replaceIfPresentFlag;
            pos += 1;
            submitSmPdu[pos] = dataCoding;
            pos += 1;
            submitSmPdu[pos] = smDefaultMsgId;
            pos += 1;

            if ((message.Length <= 254 && dataCoding != 8) || (message.Length <= 127 && dataCoding == 8))
            {
                if (dataCoding != 8)
                {
                    smLength = message.Length > 254 ? (byte)254 : (byte)message.Length;
                }
                else
                {
                    smLength = message.Length > 127 ? (byte)127 : (byte)message.Length;
                }

                submitSmPdu[pos] = smLength;
                pos += 1;
                Array.Copy(message, 0, submitSmPdu, pos, smLength);
                pos += smLength;
            }
            else
            {
                submitSmPdu[pos] = 0x00;
                pos += 1;
                // _SUBMIT_SM_PDU[pos] = 0; // message[] data
                // pos += 1;

                // message_playload configs
                submitSmPdu[pos] = 0x4;
                pos += 1;
                submitSmPdu[pos] = 0x24;
                pos += 1;

                submitSmPdu[pos] = BitConverter.GetBytes(message.Length)[1];
                pos += 1;
                submitSmPdu[pos] = BitConverter.GetBytes(message.Length)[0];
                pos += 1;

                Array.Copy(message, 0, submitSmPdu, pos, message.Length);
                pos += message.Length;
                // end message_playload configs
            }

            Tools.CopyIntToArray(pos, submitSmPdu, 0);
            return submitSmPdu;
        }//SubmitSM

        public static byte[] GetPdUunBindTranceiver(int seq)
        {
            byte[] pdu = new byte[17];

            Tools.CopyIntToArray(17, pdu, 0);

            Tools.CopyIntToArray(0x00000006, pdu, 4);

            Tools.CopyIntToArray(0, pdu, 8);

            Tools.CopyIntToArray(seq, pdu, 12);

            pdu[16] = 0;

            return pdu;
        }
    }
}