using Microsoft.VisualBasic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace cue_and_guess
{
    public partial class Form1 : Form
    {
        public static string message;
        public static string message_text;
        public static List<string> message_list = new List<string>();
        public static int frequency;
        public static char[] separator = { ':' };
        public static bool textbox_has_text;


        public static string connect_ip = "127.0.0.1";
        public static int connect_port = 6666;
        public static Socket client = null;
        public static bool is_connecting;
        public static byte[] data = new byte[1024 * 1024 * 8];


        public Form1()
        {
            InitializeComponent();
            connect_server();
            new Thread(() =>
            {
                receive_message();
            }).Start();
        }

        static void connect_server()
        {
            try
            {
                IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(connect_ip), connect_port);
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(iPEndPoint);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            try
            {
                send_message("username", "user");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        static void send_message(string type, string message)
        {
            try
            {
                string msg = (type + ":" + message).ToString();//将数据类型和数据内容组合后发送
                client.Send(Encoding.Default.GetBytes(msg));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void receive_message()
        {
            try
            {
                while (true)
                {
                    int length = client.Receive(data);
                    string message_dictionary = Encoding.Default.GetString(data, 0, length);
                    string[] message = message_dictionary.Split(separator, 2);
                    if (message_dictionary == null || message_dictionary.Length == 0)//判断是否断开连接
                    {
                        client.Close();
                        break;
                    }
                    else
                    {
                        switch (message[0])//判断数据类型
                        {
                            case "message":
                                //返回主线程
                                this.Invoke(new Action(() =>
                                {
                                    show_message(message[1]);
                                }));
                                break;
                            case "userlist":
                                this.Invoke(new Action(() =>
                                {
                                    show_user_list(message[1]);
                                }));
                                break;
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void show_user_list(string users)
        {
            string[] user_list = users.Split(new char[] { ',' });
            listBox1.Items.Clear();
            foreach (string user in user_list)
            {
                if (user.Length > 0)
                {
                    listBox1.Items.Add(user);
                }
            }
        }

        void show_message(string message)
        {
            string msg_show = message + "\r\n";
            message_list.Add(msg_show);
            if (message_list.Count > 50)//限制聊天框显示字数
            {
                message_list.RemoveRange(0, message_list.Count - 50);
            }
            message_text = null;
            foreach (string message1 in message_list)
            {
                message_text += message1;
            }
            textBox1.Text = message_text;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            textBox3.Text = "username";
            textBox3.ForeColor = Color.Gray;
            textbox_has_text = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            //保持滚动条始终处于最底部
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();

        }

        private void button1_Click(object sender, EventArgs e)//测试用按钮
        {
            message = "This is the list" + frequency.ToString() + "\r\n";
            frequency++;
            message_list.Add(message);
            textBox1_TextChanged(sender, e);
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                send_button_Click(sender, e);
            }
        }

        private void send_button_Click(object sender, EventArgs e)
        {
            message = textBox2.Text;
            textBox2.Text = null;
            send_message("message", message);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                client.Send(Encoding.Default.GetBytes("message:close"));
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }


        private void textBox3_Enter(object sender, EventArgs e)
        {
            if (textbox_has_text == false)
            {
                textBox3.Text = "";
            }
            textBox3.ForeColor = Color.Black;
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                textBox3.Text = "username";
                textBox3.ForeColor = Color.Gray;
                textbox_has_text = false;
            }
            else
            {
                textbox_has_text = true;
            }
        }

        private void change_user_name_Click(object sender, EventArgs e)
        {
            message = textBox3.Text;
            send_message("username", message);
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                change_user_name_Click(sender, e);
            }
        }
    }
}