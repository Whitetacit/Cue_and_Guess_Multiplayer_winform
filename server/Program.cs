using System;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace cue_and_guess_server
{
    class ClientController
    {
        public static string server_ip = "127.0.0.1";
        public static int server_port = 6666;
        public static Socket server;
        public static Socket client;
        public static byte[] data = new byte[1024];
        public static Dictionary<string, Socket> clients_ip = new Dictionary<string, Socket>();
        static void Main(string[] args)
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            EndPoint endPoint = new IPEndPoint(IPAddress.Parse(server_ip), server_port);
            server.Bind(endPoint);
            server.Listen(10);
            new Thread(() =>
            {
                try
                {
                    accept();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }).Start();
        
        }

        static void accept() 
        {
            while (true)
            {
                //监听连接
                client = server.Accept();
                string client_ip = client.RemoteEndPoint.ToString();
                if (!clients_ip.ContainsKey(client_ip))
                {
                    clients_ip.Add(client_ip, client);
                    Console.WriteLine(client_ip);
                }
                new Thread(() =>
                {
                    try
                    {
                        Recieve();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                })
                { IsBackground = true }.Start();

            }
        }
        static void Recieve()//接收消息
        {
            while (true)
            {
                try
                {
                    int length = client.Receive(data);
                    string message = Encoding.Default.GetString(data, 0, length);
                    Console.WriteLine(client.RemoteEndPoint.ToString() + message);
                    if (message == null || message.Length == 0 || message == "close")//判断客户端是否断开
                    {
                        string client_ip = client.RemoteEndPoint.ToString();
                        Console.WriteLine(client_ip);
                        clients_ip.Remove(client_ip);
                        break;
                    }
                    Send(message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        static void Send(string msg)//发送消息
        {
            try
            {
                string message = msg;
                foreach(Socket client in clients_ip.Values)
                {
                    client.Send(Encoding.Default.GetBytes(message));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}