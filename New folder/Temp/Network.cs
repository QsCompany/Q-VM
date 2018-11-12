using System.IO;
using System.Text;
using System.Threading;

#pragma warning disable 618

using System;
using System.Management;
using System.Net;
using System.Net.Sockets;

namespace Compiler.Temp
{
    namespace Poll
    {
        internal class ConnectionThread
        {
            public TcpListener threadListener;
            private static int connections;

            public void HandleConnection(object state)
            {
                int recv;
                var data = new byte[1024];
                var client = threadListener.AcceptTcpClient();
                var ns = client.GetStream();
                connections++;
                Console.WriteLine("New client accepted: {0} active connections",
                    connections);
                var welcome = "Welcome to my test server";
                data = Encoding.ASCII.GetBytes(welcome);
                ns.Write(data, 0, data.Length);
                while (true)
                {
                    data = new byte[1024];
                    recv = ns.Read(data, 0, data.Length);
                    if (recv == 0)
                        break;
                    ns.Write(data, 0, recv);
                }
                ns.Close();
                client.Close();
                connections--;
                Console.WriteLine("Client disconnected: {0} active connections",
                    connections);
            }
        }

        internal class ThreadPoolTcpSrvr
        {
            private TcpListener client;

            public ThreadPoolTcpSrvr()
            {
                client = new TcpListener(9050);
                client.Start();
                Console.WriteLine("Waiting for clients...");
                while (true)
                {
                    while (!client.Pending())
                    {
                        Thread.Sleep(1000);
                    }
                    var newconnection = new ConnectionThread();
                    newconnection.threadListener = client;
                    ThreadPool.QueueUserWorkItem(new
                        WaitCallback(newconnection.HandleConnection));
                }
            }

            public static void main()
            {
                var tpts = new ThreadPoolTcpSrvr();
            }
        }
    }

    namespace StreamTcp
    {
        public class varTcpClient
        {
            public readonly Socket Server;

            public varTcpClient(IPEndPoint endPoint)
            {
                var server = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    server.Connect(endPoint);
                }
                catch (SocketException e)
                {
                    throw new Exception("Unable to connect to server.", e);
                }
                var ns = new NetworkStream(server);
                var sr = new StreamReader(ns);
                var sw = new StreamWriter(ns);
                var data = sr.ReadLine();
                Console.WriteLine(data);
                while (true)
                {
                    var input = Console.ReadLine();
                    if (input == "exit")
                        break;
                    sw.WriteLine(input);
                    sw.Flush();
                    data = sr.ReadLine();
                    Console.WriteLine(data);
                }
                Console.WriteLine("Disconnecting from server...");
                sr.Close();
                sw.Close();
                ns.Close();
                server.Shutdown(SocketShutdown.Both);
                server.Close();
            }
        }

        public class VarTcpServer
        {
            public readonly Socket Client;
            public readonly int maxConnection;
            public readonly NetworkStream Stream;

            public VarTcpServer(EndPoint endPoint, int mxConnection = 10)
            {
                var newsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                newsock.Bind(endPoint);
                newsock.Listen(maxConnection = mxConnection);
                Client = newsock.Accept();
                var newclient = (IPEndPoint)Client.RemoteEndPoint;
                Stream = new NetworkStream(Client);
                var sr = new StreamReader(Stream);
                var sw = new StreamWriter(Stream);
                sw.Flush();
                while (true)
                {
                    string data;
                    try
                    {
                        data = sr.ReadLine();
                    }
                    catch (IOException)
                    {
                        break;
                    }

                    Console.WriteLine(data);
                    sw.WriteLine(data);
                    sw.Flush();
                }
                Console.WriteLine("Disconnected from {0}", newclient.Address);
                sw.Close();
                sr.Close();
                Stream.Close();
            }
        }
    }

    namespace Tcp
    {
        public class varTcpClient
        {
            public readonly Socket Server;

            public varTcpClient(IPEndPoint endPoint)
            {
                Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    Server.Connect(endPoint);
                }
                catch (SocketException e)
                {
                    throw new Exception("Unable to connect to server.", e);
                }
                var data = Help.ReceiveVarData(Server);
                var stringData = Encoding.ASCII.GetString(data);
                Console.WriteLine(stringData);
                var message1 = "This is the first test";
                var message2 = "A short test";
                var message3 =
                    "This string is an even longer test. The quick brown Â_     fox jumps over the lazy dog.";
                var message4 = "a";
                var message5 = "The last test";
                Help.SendVarData(Server, Encoding.ASCII.GetBytes(message1));
                Help.SendVarData(Server, Encoding.ASCII.GetBytes(message2));
                Help.SendVarData(Server, Encoding.ASCII.GetBytes(message3));
                Help.SendVarData(Server, Encoding.ASCII.GetBytes(message4));
                Help.SendVarData(Server, Encoding.ASCII.GetBytes(message5));
                Console.WriteLine("Disconnecting from server...");
                Server.Shutdown(SocketShutdown.Both);
                Server.Close();
            }
        }

        public class VarTcpServer
        {
            public readonly Socket Client;

            public readonly int maxConnection = 10;

            public VarTcpServer(EndPoint endPoint, int mxConnection = 10)
            {
                var newsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                newsock.Bind(endPoint);
                newsock.Listen(maxConnection = mxConnection);
                Client = newsock.Accept();
                var newclient = (IPEndPoint)Client.RemoteEndPoint;
                for (var i = 0; i < 5; i++)
                {
                    var data = Help.ReceiveVarData(Client);
                    Console.WriteLine(Encoding.ASCII.GetString(data));
                }
                Client.Close();
                newsock.Close();
            }
        }
    }

    public static class Network
    {
        public static ManagementObjectCollection AdaptaterNetwork()
        {
            var query = new
                ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = 'TRUE'");
            return query.Get();
        }

        public static void Connect(IPEndPoint endPoint, SocketAsyncEventArgs iAsyncResult)
        {
            var v = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Tcp);
            var ez = WebRequest.Create(new Uri("ftp://192.168.43.1:3721/", UriKind.RelativeOrAbsolute));
        }

        public static void GetDNSAddressInfo(IPAddress address)
        {
            var ip = Dns.GetHostByAddress(address);
        }

        public static IPHostEntry GetDnsInfo(IPAddress address)
        {
            var d = new IPHostEntry();
            var results = Dns.GetHostByAddress(address);
            return results;
        }

        public static IPHostEntry GetDnsInfo(string site)
        {
            var d = new IPHostEntry();
            var results = Dns.GetHostByName(site);
            return results;
        }

        public static IPHostEntry ResolveDnsInfo(string site)
        {
            var iphe = Dns.Resolve(site);
            return iphe;
        }

        public static IPHostEntry ResolveDnsInfo(IPAddress address)
        {
            var iphe = Dns.Resolve(address.ToString());
            return iphe;
        }

        private static void callback(IAsyncResult ar)
        {
        }

        private static void ConnectAsServer(IPEndPoint hostIP, int maxConnections, IAsyncResult ar)
        {
            var newsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            newsock.Bind(hostIP);
            newsock.Listen(maxConnections);
            var client = newsock.Accept();
            newsock.BeginAccept(client, client.ReceiveBufferSize, callback, new object[] { newsock, client });
            var data = Encoding.ASCII.GetBytes("welcome");
            client.Send(data, data.Length, SocketFlags.None);
            var newclient = (IPEndPoint)client.RemoteEndPoint;

            Console.WriteLine("Connected with {0} at port {1}", newclient.Address, newclient.Port);
            for (var i = 0; i < 5; i++)
            {
                var recv = client.Receive(data);
                Console.WriteLine(Encoding.ASCII.GetString(data, 0, recv));
            }
            Console.WriteLine("Disconnecting from {0}", newclient.Address);
            client.Close();
            newsock.Close();
        }
    }

    internal static class Help
    {
        public static byte[] ReceiveData(Socket s, int size)
        {
            var total = 0;
            var dataleft = size;
            var data = new byte[size];
            int recv;
            while (total < size)
            {
                recv = s.Receive(data, total, dataleft, 0);
                if (recv == 0)
                {
                    data = Encoding.ASCII.GetBytes("exit");
                    break;
                }
                total += recv;
                dataleft -= recv;
            }
            return data;
        }

        public static byte[] ReceiveVarData(Socket s)
        {
            var total = 0;
            int recv;
            var datasize = new byte[4];
            recv = s.Receive(datasize, 0, 4, 0);
            var size = BitConverter.ToInt32(datasize, 0);
            var dataleft = size;
            var data = new byte[size];
            while (total < size)
            {
                recv = s.Receive(data, total, dataleft, 0);
                if (recv == 0)
                {
                    data = Encoding.ASCII.GetBytes("exit ");
                    break;
                }
                total += recv;
                dataleft -= recv;
            }
            return data;
        }

        public static int SendData(Socket s, byte[] data)
        {
            var total = 0;
            var size = data.Length;
            var dataleft = size;
            int sent;
            while (total < size)
            {
                sent = s.Send(data, total, dataleft, SocketFlags.None);
                total += sent;
                dataleft -= sent;
            }
            return total;
        }

        public static int SendVarData(Socket s, byte[] data)
        {
            var total = 0;
            var size = data.Length;
            var dataleft = size;
            int sent;
            var datasize = new byte[4];
            datasize = BitConverter.GetBytes(size);
            sent = s.Send(datasize);
            while (total < size)
            {
                sent = s.Send(data, total, dataleft, SocketFlags.None);
                total += sent;
                dataleft -= sent;
            }
            return total;
        }
    }
}