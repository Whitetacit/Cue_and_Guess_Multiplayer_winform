using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Net.Http;

namespace cue_and_guess_server
{
    class server_Main
    {
        public static string ip = "127.0.0.1";//ip地址定义
        public static int port = 50000;//端口定义

        static void Main(string[] args)
        {

            byte[] data = new byte[1024];
            //服务器初始化部分
            Socket TcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//tcp通讯类型定义
            IPAddress IP = IPAddress.Parse(ip);
            EndPoint address = new IPEndPoint(IP, port);//封装ip和端口
            TcpSocket.Bind(address);//开始监听
            TcpSocket.Listen(20);//最大监听数

            while (true)
            {
                try
                {
                    while (true)
                    {
                        Socket ClientSocket = TcpSocket.Accept();//接受连接并分配端口
                        IPEndPoint CilentIP = (IPEndPoint)ClientSocket.RemoteEndPoint;
                        Console.WriteLine(CilentIP);//写出客户端地址

                        //主进程
                        Thread t1= new Thread(() =>
                        read_msg(ClientSocket, data));
                        t1.Start();
                    }
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }

        }
        private static void send_msg(Socket clientSocket, string message)
        {
            

                byte[] data = Encoding.UTF8.GetBytes(message);//对字符串做编码，得到一个字符串的字节数组
                clientSocket.Send(data);

        }

        private static void read_msg(Socket clientSocket, byte[] data)
        {
            try
            {
                while (true)
                {
                    int length = clientSocket.Receive(data);
                    string message2 = Encoding.UTF8.GetString(data, 0, length);//把字节数据转化成一个字符串
                    Console.WriteLine("收到了消息:" + message2);
                    send_msg(clientSocket, message2);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

    }
}