using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicketOffice
{
        class Conect_Receive_UDP
        {
            // Member variables of the class
            public Socket m_ReceiveSocket;
            public IPEndPoint m_localIPEndPoint;

            public Conect_Receive_UDP()


            {
           
                string szLocalIPAddress = GetLocalIPAddress_AsString(); // Get local IP address as a default value
 
                try
                {   // Create the Receive socket, for UDP use
                    m_ReceiveSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    m_ReceiveSocket.Blocking = false;
                }
                catch (SocketException se)
                {   // If an exception occurs, display an error message
                    MessageBox.Show(se.Message);
                }
            }
            public string GetLocalIPAddress_AsString()
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

        }
    }


