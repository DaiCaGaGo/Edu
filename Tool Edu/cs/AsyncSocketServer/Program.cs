using System;
using System.Configuration;
using System.Net;

//This project implements an echo socket server.
//The socket server requires four command line parameters:
//Usage: AsyncSocketServer.exe <#connections> <Receive Size In Bytes> <address family: ipv4 | ipv6> <Local Port Number>

//# Connections: The maximum number of connections the server will accept simultaneously.
//#Receive Size in Bytes: The buffer size used by the server for each receive operation.
//#Address family: The address family of the socket the server will use to listen for incoming connections.  Supported values are ‘ipv4’ and ‘ipv6’.
//#Local Port Number: The port the server will bind to.

//Example: AsyncSocketServer.exe 500 1024 ipv4 8000

namespace AsyncSocketServer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args == null) throw new ArgumentNullException("args");
            int numConnections;
            int receiveSize;
            IPEndPoint localEndPoint;

            // parse command line parameters
            // format: #connections, receive size per connection, address family, port num
            //if (args.Length < 4)
            //{
            //    Console.WriteLine("Usage: AsyncSocketServer.exe <#connections> <receiveSizeInBytes> <address family: ipv4 | ipv6> <Local Port Number>");
            //    return;
            //}

            try
            {
                //numConnections = int.Parse(args[0]);
                //receiveSize = int.Parse(args[1]);
                //string addressFamily = args[2].ToLower();
                //port = int.Parse(args[3]);

                numConnections = 200; //chap nhan 200 ket noi dong thoi toi server.
                receiveSize = 1048576; //bo dem du lieu nhan cho moi recieve operation
                const string addressFamily = "ipv4";
                var port = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);
                if (numConnections <= 0)
                {
                    throw new ArgumentException("The number of connections specified must be greater than 0");
                }
                if (receiveSize <= 0)
                {
                    throw new ArgumentException("The receive size specified must be greater than 0");
                }
                if (port <= 0)
                {
                    throw new ArgumentException("The port specified must be greater than 0");
                }

                // This sample supports two address family types: ipv4 and ipv6
                if (addressFamily.Equals("ipv4"))
                {
                    localEndPoint = new IPEndPoint(IPAddress.Any, port);
                }
                else if (addressFamily.Equals("ipv6"))
                {
                    localEndPoint = new IPEndPoint(IPAddress.IPv6Any, port);
                }
                else
                {
                    throw new ArgumentException("Invalid address family specified");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Co loi say ra trong qua trinh khoi tao tham so server:");
                Console.WriteLine(e.Message);
                Console.WriteLine("Usage: AsyncSocketServer.exe <#connections> <receiveSizeInBytes> <address family: ipv4 | ipv6> <Local Port Number>");
                return;
            }

            //Start the server listening for incoming connection requests
            Server smppServer = new Server(numConnections, receiveSize);
            smppServer.Init();
            smppServer.Start(localEndPoint);
            Console.WriteLine("Server started. Begin listening icoming connections request...");

            string datareaded = "";
            while (datareaded != "exit")
            {
                datareaded = Console.ReadLine();
                switch (datareaded)
                {
                    case "1":
                        Console.Clear();
                        break;

                    case "2":
                        Console.WriteLine("Total bytes read: {0}\nTotal Clients connected to server: {1}\nInfo Client Binded to server:\n {2}",
                            smppServer.TotalRead, smppServer.ClientConnected, smppServer.GetInfoClientBinded);
                        break;

                    case "3":
                        Console.WriteLine("Input partnerid for clear sessions by partnerid:");
                        string parteridtoclear = Console.ReadLine();
                        if (parteridtoclear != "")
                            try {
                                if (parteridtoclear != null)
                                    smppServer.ClearSessionByPartnerId(int.Parse(parteridtoclear));
                            }
                            catch (Exception ex) { Console.WriteLine("Clear sesion by partnerid cause error:" + ex.Message); }
                        break;

                    case "4":
                        Console.WriteLine("Input tokenid for clear sessions:");
                        string tokentid = Console.ReadLine();
                        if (tokentid != "")
                            try {
                                if (tokentid != null) smppServer.ClearSessionByTokenId(int.Parse(tokentid));
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Clear session by tokenid cause error:" + ex);
                            }
                        break;

                    case "5":
                        if (smppServer.IsLog)
                        {
                            smppServer.IsLog = false;
                            Console.WriteLine("SMPP Server set log to false");
                        }
                        else
                        {
                            smppServer.IsLog = true;
                            Console.WriteLine("SMPP Server set log to true");
                        }
                        break;

                    case "6":
                        Console.WriteLine(@"
                                     SMPP_SOCKET_CONNECT_SENT = 1;
                                     SMPP_SOCKET_CONNECTED = 2;
                                     SMPP_BIND_SENT = 3;
                                     SMPP_BINDED = 4;
                                     SMPP_UNBIND_SENT = 5;
                                     SMPP_UNBINDED = 6;
                                     SMPP_SOCKET_DISCONNECTED = 7;
                                     ");
                        break;

                    case "7":
                        smppServer.ClearSessionOverTimeNotSendEnquirelink();
                        break;

                    case "exit":
                        Console.WriteLine("he thong se thoat ngay");
                        break;

                    default:
                        Console.WriteLine(@"Command invalid, please try again and select option:
                            1 - Clearsceen
                            2 - Statictic
                            3 - Clear sesssion of partnerid
                            4 - Clear session by tokenid
                            5 - setLog
                            6 - Show Connection State of Session.
                            7 - Clear session not send enquirelink orver 5 minute.
                            exit - Exit application.");
                        break;
                }
            }
        }
    }
}