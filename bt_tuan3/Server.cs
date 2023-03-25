using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Server : Form
    {
        private UdpClient server;
        private Thread listeningThread;
        private List<string> messages = new List<string>();
        private IPEndPoint clientEndPoint;
        public Server()
        {
            InitializeComponent();
            server = new UdpClient(5000);
            listeningThread = new Thread(new ThreadStart(ListenForMessages));
            listeningThread.Start();

            listView1.View = View.Details;
            listView1.Columns.Add("Message", -2, HorizontalAlignment.Left);
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            server.Close();
            listeningThread.Abort();
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                e.Handled = true;
                SendMessage();
            }
        }

        private void SendMessage()
        {
            string message = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            if (Control.ModifierKeys == Keys.Shift && message.EndsWith("\r"))
            {
                message = message.Substring(0, message.Length - 1) + "\n";
            }

            byte[] data = Encoding.ASCII.GetBytes(message);
            message = "ADMIN: " + message;
            data = Encoding.ASCII.GetBytes(message);
            server.Send(data, data.Length, "localhost", 5000);
            if (clientEndPoint != null)
            {
                server.Send(data, data.Length, clientEndPoint);
            }
            Invoke(new Action(() =>
            {
                if (!messages.Contains(message))
                {
                    messages.Add(message);
                }
                DisplayMessages();
            }));
            textBox1.Text = "";
        }

        private void DisplayMessages()
        {
            listView1.Items.Clear();
            foreach (string message in messages)
            {
                listView1.Items.Add(new ListViewItem(new[] { message }));
            }
        }
        private void ListenForMessages()
        {
            while (true)
            {
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = server.Receive(ref remoteEndPoint);
                string message = Encoding.ASCII.GetString(data);

                if (clientEndPoint == null)
                {
                    clientEndPoint = remoteEndPoint;
                }

                Invoke(new Action(() =>
                {
                    if (!messages.Contains(message))
                    {
                        messages.Add(message);
                    }
                    DisplayMessages();
                }));
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = !string.IsNullOrEmpty(textBox1.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendMessage();
        }
        private void SERVER_Load(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
