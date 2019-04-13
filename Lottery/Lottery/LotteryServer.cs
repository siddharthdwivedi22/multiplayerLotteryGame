using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Data.OleDb;
using System.Data.SqlClient;

/*
Close conection and check ports
    */

namespace Lottery
    {
    public partial class LotteryServer : Form
        {
        int m_lastDraw;
        //string ip_str = "192.168.1.156";
        Socket m_SendSocket;
        Socket m_ReceiveSocket;
        IPEndPoint m_ServerIPEndPoint;

        // Declare three timer objects
        Timer m_Timer_1;
        Timer m_Timer_2;
        Timer m_Timer_3;


        // Declare variables used with the timers
        int m_iTimer_count_1;
        int m_iTimer_count_2;
        int m_iTimer_count_3;

        ConectionMessage cMessage = new ConectionMessage();


        public LotteryServer ()
            {
            InitializeComponent();

            // Initialise controls
            Timer_tbox.Text = "60";
            string szLocalIPAddress = GetLocalIPAddress_AsString(); // Get local IP address as a default value
            IPlabel.Text = szLocalIPAddress;             // Place local IP address in IP address field
            lblOutPort.Text = "8000"; // Default output port number
            lblInPort.Text = "8009";// Default input port number
            UDP_Sockets();
            cMessage.Show();
            }
        private void Form1_Load (object sender, EventArgs e)
            {
            // TODO: This line of code loads data into the 'lottoDataSet.Lottotable' table. You can move, or remove it, as needed.
            BindServer();
            txtBoxDraw.Text = DBConnectivity.LoadlastDrawNo();
            m_lastDraw = int.Parse(txtBoxDraw.Text);
            Start_Timer_2();

            }

        private void UDP_Sockets ()
            {
            try
                {
                // Create the socket, for UDP use
                m_SendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                m_SendSocket.EnableBroadcast = true;
                m_ReceiveSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                m_ReceiveSocket.Blocking = false;
                m_SendSocket.EnableBroadcast = true;
                }
            catch (SocketException se)
                {
                // If an exception occurs, display an error message
                MessageBox.Show(se.Message);
                }
            }

        private string GetLocalIPAddress_AsString ()
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

        private void Broadcast_ServerIP ()
            {
            try
                {

                // Get the IP address from the appropriate text box
                System.Net.IPAddress DestinationIPAddress = System.Net.IPAddress.Broadcast;

                // Get the Port number from the appropriate text box
                String szPort = lblOutPort.Text;
                int iPort = System.Convert.ToInt16(szPort, 10);

                // Combine Address and Port to create an Endpoint
                System.Net.IPEndPoint remoteEndPoint6 = new System.Net.IPEndPoint(DestinationIPAddress, iPort);
                System.Net.IPEndPoint remoteEndPoint7 = new System.Net.IPEndPoint(DestinationIPAddress, iPort);

                //m_SendSocket.Connect(remoteEndPoint);
                String szData6 = txtBoxDraw.Text;
                if (szData6.Equals(""))
                    {
                    szData6 = "Default message";
                    }
                byte[] byData6 = System.Text.Encoding.ASCII.GetBytes(szData6);
                m_SendSocket.SendTo(byData6, remoteEndPoint6);

                String szData7 = GetLocalIPAddress_AsString();
                if (szData7.Equals(""))
                    {
                    szData7 = "Default message";
                    }
                byte[] byData7 = System.Text.Encoding.ASCII.GetBytes(szData7);
                m_SendSocket.SendTo(byData7, remoteEndPoint7);


                }
            catch (SocketException)
                {
                // If an exception occurs, display an error message
                //MessageBox.Show(se.Message);
                }
            finally
                {

                }

            }


        private void RandNumber ()
            {
            ArrayList arlNum = new ArrayList();
            Random RandomClass = new Random();

            while (arlNum.Count < 5)
                {
                int g = RandomClass.Next(1, 50);
                if (!arlNum.Contains(g))
                    {
                    arlNum.Add(g);
                    }
                }

            arlNum.Sort();

            textBox1.Text = arlNum[0].ToString();
            textBox2.Text = arlNum[1].ToString();
            textBox3.Text = arlNum[2].ToString();
            textBox4.Text = arlNum[3].ToString();
            textBox5.Text = arlNum[4].ToString();
            }

        //timers
        void Start_Timer_1 ()
            {
            // Initialise variables
            m_iTimer_count_1 = 600;
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
                timer1.Enabled = true;
                timer2.Enabled = true;
                Start_Timer_3();
                }
            else if (0 >= m_iTimer_count_1 % 2)
                {
                receive_Ticket();

                }
            else
                {   // If the value is an odd number
                DBConnectivity.LoadDraws(LottoTable, txtBoxDraw.Text);
                txtBoxPrice.Text = DBConnectivity.LoadPrice(int.Parse(txtBoxDraw.Text)).ToString();
                }

            Timer_tbox.Text = m_iTimer_count_1.ToString();


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
                m_Timer_2.Enabled = false;
                //btnStart.Enabled = true;
                lblDraw.Text = "Curent Draw";
                btnStart.Text = "Conected";
                btnStart.BackColor = Color.Green;
                btnStart.ForeColor = Color.Gold;
                m_lastDraw++;
                txtBoxDraw.Text = m_lastDraw.ToString();

                Start_Timer_1();


                }
            else if (m_iTimer_count_2 > 0)
                {
                Broadcast_ServerIP();
                btnStart.Enabled = false;

                }


            }
        void Start_Timer_3 ()
            {
            // Initialise variables
            m_iTimer_count_3 = 10;
            m_Timer_3 = new System.Windows.Forms.Timer();
            m_Timer_3.Interval = 1000; // 1000 milliseconds (1 second)
            m_Timer_3.Tick += OnTimedEvent_Timer_3;
            m_Timer_3.Enabled = true;
            }


        private void OnTimedEvent_Timer_3 (object sender, EventArgs e)
            {

            m_iTimer_count_3--;

            if (m_iTimer_count_3 == 0)
                {
                m_Timer_3.Enabled = false;
                btnSend.BackColor = Color.Green;
                btnSend.ForeColor = Color.Gold;
                btnReset.Enabled = true;
                }
            else if (m_iTimer_count_3 % 2 > 0)
                {
                lotto_Winner();
                Send_Draw_Reults();
                btnSend.BackColor = Color.Red;
                btnSend.ForeColor = Color.Gold;

                }
            else
                {

                btnSend.BackColor = Color.Gold;
                btnSend.ForeColor = Color.Red;
                }


            }

        private void timer1_Tick_1 (object sender, EventArgs e)
            {

            RandNumber();

            }

        private void timer2_Tick_1 (object sender, EventArgs e)
            {
            timer1.Enabled = false;

            }

        private void btnSend_Click (object sender, EventArgs e)
            {
            lotto_Winner();
            Send_Draw_Reults();

            }

        private void Send_Draw_Reults ()
            {
            try
                {
                // Get the IP address from the appropriate text box
                System.Net.IPAddress DestinationIPAddress = System.Net.IPAddress.Broadcast;//Parse(ip_str);

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

                //m_SendSocket.Connect(remoteEndPoint);
                String szData1 = textBox1.Text; //+ " - " +textBox2.Text+" - "+textBox3.Text + " - "+ textBox4.Text +" - "+textBox5.Text;
                if (szData1.Equals(""))
                    {
                    szData1 = "Default message";
                    }
                byte[] byData1 = System.Text.Encoding.ASCII.GetBytes(szData1);
                m_SendSocket.SendTo(byData1, remoteEndPoint1);

                String szData2 = textBox2.Text;
                if (szData2.Equals(""))
                    {
                    szData2 = "Default message";
                    }
                byte[] byData2 = System.Text.Encoding.ASCII.GetBytes(szData2);
                m_SendSocket.SendTo(byData2, remoteEndPoint2);

                String szData3 = textBox3.Text;
                if (szData3.Equals(""))
                    {
                    szData3 = "Default message";
                    }
                byte[] byData3 = System.Text.Encoding.ASCII.GetBytes(szData3);
                m_SendSocket.SendTo(byData3, remoteEndPoint3);

                String szData4 = textBox4.Text;
                if (szData4.Equals(""))
                    {
                    szData4 = "Default message";
                    }
                byte[] byData4 = System.Text.Encoding.ASCII.GetBytes(szData4);
                m_SendSocket.SendTo(byData4, remoteEndPoint4);

                String szData5 = textBox5.Text;
                if (szData5.Equals(""))
                    {
                    szData5 = "Default message";
                    }
                byte[] byData5 = System.Text.Encoding.ASCII.GetBytes(szData5);
                m_SendSocket.SendTo(byData5, remoteEndPoint5);

                String szData6 = label12.Text;
                if (szData6.Equals(""))
                    {
                    szData6 = "Default message";
                    }
                byte[] byData6 = System.Text.Encoding.ASCII.GetBytes(szData6);
                m_SendSocket.SendTo(byData6, remoteEndPoint6);

                }
            catch (SocketException se)
                {
                // If an exception occurs, display an error message
                MessageBox.Show(se.Message);
                }
            //m_SendSocket.Close();
            }


        private void lotto_Winner ()
            {
            string winnerName = DBConnectivity.LoadlWinner(int.Parse(txtBoxDraw.Text), textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text);
            if (winnerName != null)
                {
                label12.Text = winnerName + " £" + txtBoxPrice.Text;
                }
            else
                {
                label12.Text = "No winner this time :(";
                }


            }

        private void btnReset_Click (object sender, EventArgs e)
            {
            textBox1.Text = "0";
            textBox2.Text = "0";
            textBox3.Text = "0";
            textBox4.Text = "0";
            textBox5.Text = "0";
            }

        private void btnLoadTable_Click (object sender, EventArgs e)
            {

            DBConnectivity.LoadDraws(LottoTable, txtBoxDraw.Text);

            }

        private void btnStart_Click (object sender, EventArgs e)
            {

            }




        private void receive_Ticket ()
            {
            try
                {
                EndPoint localEndPoint1 = (EndPoint)m_ServerIPEndPoint;
                byte[] ReceiveBuffer1 = new byte[20];
                int iReceiveByteCount1;
                iReceiveByteCount1 = m_ReceiveSocket.ReceiveFrom(ReceiveBuffer1, ref localEndPoint1);

                if (0 < iReceiveByteCount1)
                    {   // Copy the number of bytes received, from the message buffer to the text control
                    label6.Text = Encoding.ASCII.GetString(ReceiveBuffer1, 0, iReceiveByteCount1);

                    }

                EndPoint localEndPoint2 = (EndPoint)m_ServerIPEndPoint;
                byte[] ReceiveBuffer2 = new byte[2];
                int iReceiveByteCount2;
                iReceiveByteCount2 = m_ReceiveSocket.ReceiveFrom(ReceiveBuffer2, ref localEndPoint2);

                if (0 < iReceiveByteCount2)
                    {   // Copy the number of bytes received, from the message buffer to the text control
                    label7.Text = Encoding.ASCII.GetString(ReceiveBuffer2, 0, iReceiveByteCount2);

                    }
                EndPoint localEndPoint3 = (EndPoint)m_ServerIPEndPoint;
                byte[] ReceiveBuffer3 = new byte[2];
                int iReceiveByteCount3;
                iReceiveByteCount3 = m_ReceiveSocket.ReceiveFrom(ReceiveBuffer3, ref localEndPoint3);

                if (0 < iReceiveByteCount3)
                    {   // Copy the number of bytes received, from the message buffer to the text control
                    label8.Text = Encoding.ASCII.GetString(ReceiveBuffer3, 0, iReceiveByteCount3);

                    }
                EndPoint localEndPoint4 = (EndPoint)m_ServerIPEndPoint;
                byte[] ReceiveBuffer4 = new byte[2];
                int iReceiveByteCount4;
                iReceiveByteCount4 = m_ReceiveSocket.ReceiveFrom(ReceiveBuffer4, ref localEndPoint4);

                if (0 < iReceiveByteCount4)
                    {   // Copy the number of bytes received, from the message buffer to the text control
                    label9.Text = Encoding.ASCII.GetString(ReceiveBuffer4, 0, iReceiveByteCount4);

                    }
                EndPoint localEndPoint5 = (EndPoint)m_ServerIPEndPoint;
                byte[] ReceiveBuffer5 = new byte[2];
                int iReceiveByteCount5;
                iReceiveByteCount5 = m_ReceiveSocket.ReceiveFrom(ReceiveBuffer5, ref localEndPoint5);

                if (0 < iReceiveByteCount5)
                    {   // Copy the number of bytes received, from the message buffer to the text control
                    label10.Text = Encoding.ASCII.GetString(ReceiveBuffer5, 0, iReceiveByteCount5);

                    }
                EndPoint localEndPoint6 = (EndPoint)m_ServerIPEndPoint;
                byte[] ReceiveBuffer6 = new byte[2];
                int iReceiveByteCount6;
                iReceiveByteCount6 = m_ReceiveSocket.ReceiveFrom(ReceiveBuffer6, ref localEndPoint6);

                if (0 < iReceiveByteCount6)
                    {   // Copy the number of bytes received, from the message buffer to the text control
                    label11.Text = Encoding.ASCII.GetString(ReceiveBuffer6, 0, iReceiveByteCount6);
                    }

                EndPoint localEndPoint7 = (EndPoint)m_ServerIPEndPoint;
                byte[] ReceiveBuffer7 = new byte[20];
                int iReceiveByteCount7;
                iReceiveByteCount7 = m_ReceiveSocket.ReceiveFrom(ReceiveBuffer7, ref localEndPoint7);

                if (0 < iReceiveByteCount7)
                    {   // Copy the number of bytes received, from the message buffer to the text control
                    label13.Text = Encoding.ASCII.GetString(ReceiveBuffer7, 0, iReceiveByteCount7);

                    }
                FillDBLotto();

                }
            catch // Catch any errors
                {
                // Display a diagnostic message
                //MessageBox.Show("*** No message received ***");
                }

            }

        private void FillDBLotto ()
            {
            string name = label6.Text;
            string no1 = label7.Text;
            string no2 = label8.Text;
            string no3 = label9.Text;
            string no4 = label10.Text;
            string no5 = label11.Text;
            string dno = txtBoxDraw.Text;
            string am = label13.Text;
            DBConnectivity.SaveNumber(name, no1, no2, no3, no4, no5, dno, am);

            }


        private void BindServer ()
            {
            try
                {

                // Get the Port number from the appropriate text box
                String szPort = lblInPort.Text;
                int iPort = System.Convert.ToInt16(szPort, 10);
                // Create an Endpoint that will cause the listening activity to apply to all the local node's interfaces
                m_ServerIPEndPoint = new System.Net.IPEndPoint(IPAddress.Any, iPort);
                // Bind to the local IP Address and selected port
                m_ReceiveSocket.Bind(m_ServerIPEndPoint);
                //Receive_button.Enabled = true;
                //Bind_button.Enabled = false;
                }
            catch (SocketException se)
                {
                // If an exception occurs, display an error message
                MessageBox.Show(se.Message);
                }
            }

        }
    }
