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
        public static Socket server = null;
        public static Socket client = null;
        public static byte[] data = new byte[1024 * 1024 * 8];
        public static string[] message = new string[2];
        public static char[] separator = { ':' };
        public static Dictionary<string, Socket> clients_ip = new Dictionary<string, Socket> { };
        public static Dictionary<string, string> user_name = new Dictionary<string, string> { };

        public static bool is_game_start = false;
        public static bool is_users_ready;
        public static Dictionary<string, bool> user_ready = new Dictionary<string, bool>();
        public static Dictionary<string, int> users_joined = new Dictionary<string, int> { };
        public static List<string> users_joined_sort = new List<string>();
        public static string host_user = null;
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
                    accept(server);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }).Start();

            
        }

        static void accept(object obj) 
        {
            Socket server = (Socket)obj;
            while (true)
            {
                //监听连接
                client = server.Accept();
                string client_ip = client.RemoteEndPoint.ToString();
                if (!clients_ip.ContainsKey(client_ip))
                {
                    clients_ip.Add(client_ip, client);
                    Console.WriteLine(client_ip + clients_ip[client_ip].RemoteEndPoint.ToString());
                    user_ready.Add(client_ip, false);

                }
                new Thread(() =>
                {
                    try
                    {
                        Recieve(client);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                })
                { IsBackground = true }.Start();

            }
        }

        static void close_client()
        {
            string client_ip = client.RemoteEndPoint.ToString();
            Console.WriteLine(client_ip + "lost connect");
            user_name.Remove(client_ip);
            clients_ip.Remove(client_ip);
        }
        static void Recieve(object obj)//接收消息
        {
            while (true)
            {
                Socket client = (Socket)obj;
                try
                {
                    string client_ip = client.RemoteEndPoint.ToString();
                    int length = client.Receive(data);
                    string message_dictionary = Encoding.Default.GetString(data, 0, length);
                    Console.WriteLine(client_ip + ":" + message_dictionary.ToString());
                    string[] message = message_dictionary.Split(separator,2);
                    if (message == null || message.Length == 0)//判断客户端是否断开
                    {
                        close_client();
                        break;
                    }
                    
                    switch (message[0])//判断数据类型
                    {
                        case "message"://消息
                            if (message[1] == "close")
                            {
                                Console.WriteLine(client_ip + "断开连接");
                                
                            }
                            else
                            {
                                Send("message", user_name[client_ip] + ": " + message[1]);
                                Console.WriteLine("发送消息 ip:" + client_ip + " name:" + user_name[client_ip] + ": " + message[1]);
                            }
                            break;

                        case "username"://用户名
                            if (user_name.ContainsKey(client_ip))
                            {
                                user_name[client_ip] = message[1];
                            }
                            else
                            {
                                user_name.Add(client_ip, message[1]);
                            }
                            string users_name = null;
                            foreach (KeyValuePair<string, string> pair in user_name)
                            {
                                users_name += pair.Value + ",";
                            }
                            Console.WriteLine(users_name);
                            Send("userlist", users_name);
                            break;
                        case "ready":
                            if (is_game_start == false)
                            {
                                if (message[1] == "1")
                                {
                                    user_ready[client_ip] = true;
                                }
                                else
                                {
                                    user_ready[client_ip] = false;
                                }
                            }
                            break;
                        case "game":
                            if (host_user.Contains(client_ip))
                            {
                                Send("game", message[1]);
                            }
                            break;
                    }
                    if (message_dictionary == "message:close")//客户端主动关闭连接
                    {
                        close_client();
                        break;
                    }
                    

                    
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    break;
                }
            }
        }
        static void Send(string type, string msg)//发送消息
        {
            try
            {
                string message = type + ":" + msg;
                if (type == "game")
                {
                    foreach (Socket client in clients_ip.Values)//向每个用户发送信息
                    {
                        if (clients_ip.FirstOrDefault(x=> x.Value == client).Key != host_user)//不给房主发送提示信息
                        {
                            client.Send(Encoding.Default.GetBytes(message));
                        }
                    }
                }
                else
                {
                    foreach (Socket client in clients_ip.Values)//向每个用户发送信息
                    {
                        client.Send(Encoding.Default.GetBytes(message));
                    }
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        static void game_control(object obj)
        {
            while(true)
            {
                if (check_users_ready())
                {
                    Send("message", "游戏将在5秒后开始");
                    //Thread.Sleep(5000);
                    if (check_users_ready())//重复检测防止准备启动时有人加入
                    {
                        is_game_start = true;
                    }
                }
                if (is_game_start == true)
                {
                    users_joined = null;
                    foreach (var user in user_name)//将已加入的玩家加入到游戏列表中
                    {
                        users_joined.Add(user.Key, 0);
                        users_joined_sort = random_list(users_joined);

                    }
                    users_joined_sort = random_list(users_joined);
                    for (int i = 0; i < 2; i++)
                    {
                        foreach (string user in users_joined_sort)
                        {
                            host_user = user;

                        }
                    }

                    
                }
            }
        }

        static bool check_users_ready()
        {
            foreach (var value in user_ready.Values)
            {
                if (value == false)
                {
                    return false;
                }
            }
            return true;
        }

        static List<string> random_list(Dictionary<string, int> dictionary)
        {
            List<string> list = dictionary.Keys.ToList();
            Random random = new Random();
            list = list.OrderBy(k => random.Next()).ToList();
            return list;
        }
    }
}