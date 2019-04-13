using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

/*
Close conection and check ports
    */
namespace TicketOffice
    {
    public partial class TicketMachine : Form
        {

        //string ip_str = GetLocalIPAddress_AsString();
        Socket m_ReceiveSocket;
        Socket m_SendSocket;
        IPEndPoint m_localIPEndPoint;

        // Declare three timer objects
        Timer m_Timer_1;
        Timer m_Timer_2;

        // Declare variables used with the timers
        int m_iTimer_count_1;
        int m_iTimer_count_2;


        public TicketMachine ()
            {
            InitializeComponent();
            string szLocalIPAddress = GetLocalIPAddress_AsString(); // Get local IP address as a default value
            lblIP.Text = szLocalIPAddress;             // Place local IP address in IP address field
            lblOutPort.Text = "8009";  // Default port number
            lblInPort.Text = "8000";
            Start_Timer_2();
            //Receive_button.Enabled = false;     // Receive button is not enabled until the Bind has completed
            try
                {   // Create the Receive socket, for UDP use
                m_ReceiveSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                m_ReceiveSocket.Blocking = false;
                m_ReceiveSocket.EnableBroadcast = true;
                // Create the socket, for UDP use
                m_SendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                m_SendSocket.EnableBroadcast = true;
                }
            catch (SocketException se)
                {   // If an exception occurs, display an error message
                MessageBox.Show(se.Message);
                }

            }

        public string GetLocalIPAddress_AsString ()
            {
            string szHost = Dns.GetHostName();
            string szLocalIPaddress = "127.0.0.1";  // Default is local loopback address
            IPHostEntry IPHost = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress IP in IPHost.AddressList)
                {
                if (IP.AddressFamily == AddressFamily.InterNetwork) // Match only the IPv4 address
                    {
                    szLocalIPaddress = IP.ToString();

                    break;
                    }
                }
            return szLocalIPaddress;
            }

        //timers
        void Start_Timer_1 ()
            {
            // Initialise variables
            m_iTimer_count_1 = 5;
            m_Timer_1 = new System.Windows.Forms.Timer();
            m_Timer_1.Interval = 1000; // 1000 milliseconds (1 second)
            m_Timer_1.Tick += OnTimedEvent_Timer_1;
            m_Timer_1.Enabled = true;
            }
        private void OnTimedEvent_Timer_1 (object sender, EventArgs e)
            {

            m_iTimer_count_1--;
            if (m_iTimer_count_1 == 0)
                {
                m_Timer_1.Enabled = false;
                
                }
            else if (m_iTimer_count_1 % 2 > 0)
                {
                label11.Text = "The Winner Is!";
                Receive_Num();
                
                }

            }

        void Start_Timer_2 ()
            {
            // Initialise variables
            m_iTimer_count_2 = 15;
            m_Timer_2 = new System.Windows.Forms.Timer();
            m_Timer_2.Interval = 1000; // 1000 milliseconds (1 second)
            m_Timer_2.Tick += OnTimedEvent_Timer_2;
            m_Timer_2.Enabled = true;
            }
        private void OnTimedEvent_Timer_2 (object sender, EventArgs e)
            {

            m_iTimer_count_2--;
            if (m_iTimer_count_2 == 0)
                {

                btnStart.Text = "Connected";
                btnStart.BackColor = Color.Green;
                btnStart.ForeColor = Color.Gold;
                label4.Text = "Draw No.";
                int last_draw = int.Parse(txtBoxDraw.Text);
                int new_draw = last_draw + 1;
                txtBoxDraw.Text = new_draw.ToString();
                m_Timer_2.Enabled = false;
                

                }
            else if (m_iTimer_count_2 > 0)
                {
                Get_Server_IP();
                btnStart.Enabled = false;
               
                }
            label13.Text = m_iTimer_count_2.ToString();
            }


        private void btnPlay_Click (object sender, EventArgs e)
            {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text) || string.IsNullOrEmpty(textBox5.Text) || string.IsNullOrEmpty(textBox11.Text))
                {
                MessageBox.Show("Fields Can not be blank");
                }
            else
                {

                Arrayorganizer(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text);
                send_tickets();
                Tckt();
                Tckt_clear();
                }
            }

        private List<Label> Arrayorganizer (string no1_str, string no2_str, string no3_str, string no4_str, string no5_str)
            {

            var num_Str = new List<Label>() { label6, label7, label8, label9, label10 };

            int[] num = { int.Parse(no1_str), int.Parse(no2_str), int.Parse(no3_str), int.Parse(no4_str), int.Parse(no5_str) };

            bool didSwap;

            do
                {
                didSwap = false;

                for (int i = 0; i < num.Length - 1; i++)
                    {
                    if (num[i] > num[i + 1])
                        {
                        int temp = num[i + 1];
                        num[i + 1] = num[i];
                        num[i] = temp;
                        didSwap = true;
                        }
                    }

                } while (didSwap);

            for (int i = 0; i != num.Length; i++)
                {
                num_Str[i].Text = (num[i].ToString());
                }
            return num_Str;
            }

        private void send_tickets ()
            {
            try
                {
                // Get the IP address from the appropriate text box

                System.Net.IPAddress DestinationIPAddress = System.Net.IPAddress.Parse(serverIPlbl.Text);

                // Get the Port number from the appropriate text box
                String szPort = lblOutPort.Text;
                int iPort = System.Convert.ToInt16(szPort, 10);

                // Combine Address and Port to create an Endpoint
                System.Net.IPEndPoint remoteEndPoint1 = new System.Net.IPEndPoint(DestinationIPAddress, iPort);
                System.Net.IPEndPoint remoteEndPoint2 = new System.Net.IPEndPoint(DestinationIPAddress, iPort);
                System.Net.IPEndPoint remoteEndPoint3 = new System.Net.IPEndPoint(DestinationIPAddress, iPort);
                System.Net.IPEndPoint remoteEndPoint4 = new System.Net.IPEndPoint(DestinationIPAddress, iPort);
                System.Net.IPEndPoint remoteEndPoint5 = new System.Net.IPEndPoint(DestinationIPAddress, iPort);
                System.Net.IPEndPoint remoteEndPoint6 = new System.Net.IPEndPoint(DestinationIPAddress, iPort);
                System.Net.IPEndPoint remoteEndPoint7 = new System.Net.IPEndPoint(DestinationIPAddress, iPort);

                String szData1 = textBox11.Text; //
                if (szData1.Equals(""))
                    {
                    szData1 = "Default message";
                    }
                byte[] byData1 = System.Text.Encoding.ASCII.GetBytes(szData1);
                m_SendSocket.SendTo(byData1, remoteEndPoint1);

                String szData2 = label6.Text; //+ " - " +textBox2.Text+" - "+textBox3.Text + " - "+ textBox4.Text +" - "+textBox5.Text;
                if (szData2.Equals(""))
                    {
                    szData2 = "Default message";
                    }
                byte[] byData2 = System.Text.Encoding.ASCII.GetBytes(szData2);
                m_SendSocket.SendTo(byData2, remoteEndPoint2);

                String szData3 = label7.Text;
                if (szData3.Equals(""))
                    {
                    szData3 = "Default message";
                    }
                byte[] byData3 = System.Text.Encoding.ASCII.GetBytes(szData3);
                m_SendSocket.SendTo(byData3, remoteEndPoint3);

                String szData4 = label8.Text;
                if (szData4.Equals(""))
                    {
                    szData4 = "Default message";
                    }
                byte[] byData4 = System.Text.Encoding.ASCII.GetBytes(szData4);
                m_SendSocket.SendTo(byData4, remoteEndPoint4);

                String szData5 = label9.Text;
                if (szData5.Equals(""))
                    {
                    szData5 = "Default message";
                    }
                byte[] byData5 = System.Text.Encoding.ASCII.GetBytes(szData5);
                m_SendSocket.SendTo(byData5, remoteEndPoint5);

                String szData6 = label10.Text;
                if (szData6.Equals(""))
                    {
                    szData6 = "Default message";
                    }
                byte[] byData6 = System.Text.Encoding.ASCII.GetBytes(szData6);
                m_SendSocket.SendTo(byData6, remoteEndPoint6);

                String szData7 = price_Tbox.Text;
                if (szData7.Equals(""))
                    {
                    szData7 = "Default message";
                    }
                byte[] byData7 = System.Text.Encoding.ASCII.GetBytes(szData7);
                m_SendSocket.SendTo(byData7, remoteEndPoint7);
                }
            catch (SocketException se)
                {
                // If an exception occurs, display an error message
                MessageBox.Show(se.Message);
                }


            }


        private void Tckt ()
            {
            Ticket tckt = new Ticket();
            tckt.SetName = textBox11.Text;
            tckt.SetNo1 = textBox1.Text;
            tckt.SetNo2 = textBox2.Text;
            tckt.SetNo3 = textBox3.Text;
            tckt.SetNo4 = textBox4.Text;
            tckt.SetNo5 = textBox5.Text;
            tckt.Show();
            }
        private void Tckt_clear ()
            {
            textBox11.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            }

        private void btnDisplay_Click (object sender, EventArgs e)
            {

            Start_Timer_1();

            }


        private void Receive_Num ()
            {
            string val1 = textBox6.Text;

            try
                {
                EndPoint localEndPoint1 = (EndPoint)m_localIPEndPoint;
                byte[] ReceiveBuffer1 = new byte[20];
                int iReceiveByteCount1;
                iReceiveByteCount1 = m_ReceiveSocket.ReceiveFrom(ReceiveBuffer1, ref localEndPoint1);

                if (0 < iReceiveByteCount1)
                    {   // Copy the number of bytes received, from the message buffer to the text control
                    textBox6.Text = Encoding.ASCII.GetString(ReceiveBuffer1, 0, iReceiveByteCount1);

                    }
                EndPoint localEndPoint2 = (EndPoint)m_localIPEndPoint;
                byte[] ReceiveBuffer2 = new byte[4];
                int iReceiveByteCount2;
                iReceiveByteCount2 = m_ReceiveSocket.ReceiveFrom(ReceiveBuffer2, ref localEndPoint2);

                if (0 < iReceiveByteCount2)
                    {   // Copy the number of bytes received, from the message buffer to the text control
                    textBox7.Text = Encoding.ASCII.GetString(ReceiveBuffer2, 0, iReceiveByteCount2);

                    }
                EndPoint localEndPoint3 = (EndPoint)m_localIPEndPoint;
                byte[] ReceiveBuffer3 = new byte[4];
                int iReceiveByteCount3;
                iReceiveByteCount3 = m_ReceiveSocket.ReceiveFrom(ReceiveBuffer3, ref localEndPoint3);

                if (0 < iReceiveByteCount3)
                    {   // Copy the number of bytes received, from the message buffer to the text control
                    textBox8.Text = Encoding.ASCII.GetString(ReceiveBuffer3, 0, iReceiveByteCount3);

                    }
                EndPoint localEndPoint4 = (EndPoint)m_localIPEndPoint;
                byte[] ReceiveBuffer4 = new byte[4];
                int iReceiveByteCount4;
                iReceiveByteCount4 = m_ReceiveSocket.ReceiveFrom(ReceiveBuffer4, ref localEndPoint4);

                if (0 < iReceiveByteCount4)
                    {   // Copy the number of bytes received, from the message buffer to the text control
                    textBox9.Text = Encoding.ASCII.GetString(ReceiveBuffer4, 0, iReceiveByteCount4);

                    }
                EndPoint localEndPoint5 = (EndPoint)m_localIPEndPoint;
                byte[] ReceiveBuffer5 = new byte[4];
                int iReceiveByteCount5;
                iReceiveByteCount5 = m_ReceiveSocket.ReceiveFrom(ReceiveBuffer5, ref localEndPoint5);

                if (0 < iReceiveByteCount5)
                    {   // Copy the number of bytes received, from the message buffer to the text control
                    textBox10.Text = Encoding.ASCII.GetString(ReceiveBuffer5, 0, iReceiveByteCount5);
                    }

                EndPoint localEndPoint6 = (EndPoint)m_localIPEndPoint;
                byte[] ReceiveBuffer6 = new byte[20];
                int iReceiveByteCount6;
                iReceiveByteCount6 = m_ReceiveSocket.ReceiveFrom(ReceiveBuffer6, ref localEndPoint6);

                if (0 < iReceiveByteCount6)
                    {   // Copy the number of bytes received, from the message buffer to the text control
                    label11.Text = Encoding.ASCII.GetString(ReceiveBuffer6, 0, iReceiveByteCount6);
                    }
                }
            catch (Exception)// Catch any errors
                {   // Display a diagnostic message
                    //MessageBox.Show("*** No message received ***" + ex);
                }

            }


        private void BindClient ()
            {
            try
                {

                // Get the Port number from the appropriate text box
                String szPort = lblInPort.Text;
                int iPort = System.Convert.ToInt16(szPort, 10);
                // Create an Endpoint that will cause the listening activity to apply to all the local node's interfaces
                m_localIPEndPoint = new System.Net.IPEndPoint(IPAddress.Any, iPort);
                // Bind to the local IP Address and selected port
                m_ReceiveSocket.Bind(m_localIPEndPoint);

                }
            catch (SocketException se)
                {
                // If an exception occurs, display an error message
                MessageBox.Show(se.Message);
                }

            }

        private void TicketMachine_Load (object sender, EventArgs e)
            {
            BindClient();
            }

        private void btnStart_Click (object sender, EventArgs e)
            {


            }

        private void Get_Server_IP ()
            {

            try
                {
                EndPoint localEndPoint6 = (EndPoint)m_localIPEndPoint;
                EndPoint localEndPoint7 = (EndPoint)m_localIPEndPoint;
                byte[] ReceiveBuffer6 = new byte[20];
                byte[] ReceiveBuffer7 = new byte[20];
                int iReceiveByteCount6;
                int iReceiveByteCount7;
                iReceiveByteCount6 = m_ReceiveSocket.ReceiveFrom(ReceiveBuffer6, ref localEndPoint6);
                iReceiveByteCount7 = m_ReceiveSocket.ReceiveFrom(ReceiveBuffer7, ref localEndPoint7);

                if (0 < iReceiveByteCount7)
                    {
                    // Copy the number of bytes received, from the message buffer to the text control
                    serverIPlbl.Text = Encoding.ASCII.GetString(ReceiveBuffer7, 0, iReceiveByteCount7);
                    }

                if (0 < iReceiveByteCount6)
                    {
                    // Copy the number of bytes received, from the message buffer to the text control
                    txtBoxDraw.Text = Encoding.ASCII.GetString(ReceiveBuffer6, 0, iReceiveByteCount6);
                    }

                }
            catch (Exception) //Catch any errors
                {
                // Display a diagnostic message
                //MessageBox.Show("*** No message received *** " + ex);
                }
            //m_ReceiveSocket.Close();

            }


        private void textBox1_KeyPress (object sender, KeyPressEventArgs e)
            {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                e.Handled = true;
                }
            }

        private void textBox2_Validating (object sender, CancelEventArgs e)
            {
            int min = 1, max = 50;

            try
                {
                int n2 = int.Parse(textBox2.Text);
                if (n2 > max || n2 < min)
                    {
                    MessageBox.Show("Number can be only in range " + min.ToString() + " and " + max.ToString());
                    textBox2.Text = "";
                    }
                else if (textBox2.Text == textBox1.Text || textBox2.Text == textBox3.Text || textBox2.Text == textBox4.Text || textBox2.Text == textBox5.Text)
                    {
                    MessageBox.Show("Two Lotto numbers cannot be same. Choose 5 different Numbers");
                    textBox2.Text = "";
                    }
                }
            catch (Exception)
                {
                }
            }

        private void textBox2_KeyPress (object sender, KeyPressEventArgs e)
            {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                e.Handled = true;
                }
            }

        private void textBox3_KeyPress (object sender, KeyPressEventArgs e)
            {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))

                {
                e.Handled = true;
                }
            }

        private void textBox4_KeyPress (object sender, KeyPressEventArgs e)
            {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                e.Handled = true;
                }
            }

        private void textBox5_KeyPress (object sender, KeyPressEventArgs e)
            {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                e.Handled = true;
                }
            }

        private void textBox1_Validating (object sender, CancelEventArgs e)
            {
            int min = 1, max = 50;

            try
                {
                int n1 = int.Parse(textBox1.Text);
                if (n1 > max || n1 < min)
                    {
                    MessageBox.Show("Number can be only in range " + min.ToString() + " and " + max.ToString());
                    textBox1.Text = "";
                    }
                else if (textBox1.Text == textBox2.Text || textBox1.Text == textBox3.Text || textBox1.Text == textBox4.Text || textBox1.Text == textBox5.Text)
                    {
                    MessageBox.Show("Two Lotto numbers cannot be same. Choose 5 different Numbers");
                    textBox1.Text = "";
                    }
                }
            catch (Exception)
                {
                //
                }
            }

        private void textBox3_Validating (object sender, CancelEventArgs e)
            {
            int min = 1, max = 50;

            try
                {
                int n3 = int.Parse(textBox3.Text);
                if (n3 > max || n3 < min)
                    {
                    MessageBox.Show("Number can be only in range " + min.ToString() + " and " + max.ToString());
                    textBox3.Text = "";
                    }
                else if (textBox3.Text == textBox1.Text || textBox3.Text == textBox2.Text || textBox3.Text == textBox4.Text || textBox3.Text == textBox5.Text)
                    {
                    MessageBox.Show("Two Lotto numbers cannot be same. Choose 5 different Numbers");
                    textBox3.Text = "";
                    }
                }
            catch (Exception)
                {
                //
                }
            }

        private void textBox4_Validating (object sender, CancelEventArgs e)
            {
            int min = 1, max = 50;

            try
                {
                int n4 = int.Parse(textBox4.Text);
                if (n4 > max || n4 < min)
                    {
                    MessageBox.Show("Number can be only in range " + min.ToString() + " and " + max.ToString());
                    textBox4.Text = "";
                    }
                else if (textBox4.Text == textBox1.Text || textBox4.Text == textBox2.Text || textBox4.Text == textBox3.Text || textBox4.Text == textBox5.Text)
                    {
                    MessageBox.Show("Two Lotto numbers cannot be same. Choose 5 different Numbers");
                    textBox4.Text = "";
                    }
                }
            catch (Exception)
                {
                //
                }
            }

        private void textBox5_Validating (object sender, CancelEventArgs e)
            {
            int min = 1, max = 50;

            try
                {
                int n5 = int.Parse(textBox5.Text);
                if (n5 > max || n5 < min)
                    {
                    MessageBox.Show("Number can be only in range " + min.ToString() + " and " + max.ToString());
                    textBox5.Text = "";
                    }
                else if (textBox5.Text == textBox1.Text || textBox5.Text == textBox2.Text || textBox5.Text == textBox3.Text || textBox5.Text == textBox4.Text)
                    {
                    MessageBox.Show("Two Lotto numbers cannot be same. Choose 5 different Numbers");
                    textBox5.Text = "";
                    }
                }
            catch (Exception)
                {

                }
            }
        }
    }
