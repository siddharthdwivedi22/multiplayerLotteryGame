using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace TicketOffice
{
    public partial class Ticket : Form
    {
        private string name, no1, no2, no3, no4,no5;

        public string SetName
        {
            get { return name; }
            set { name = value; }
        }
        public string SetNo1
        {
            get { return no1; }
            set { no1 = value; }
        }
        public string SetNo2
        {
            get { return no2; }
            set { no2 = value; }
        }

        public string SetNo3
        {
            get { return no3; }
            set { no3 = value; }
        }

        public string SetNo4
        {
            get { return no4; }
            set { no4 = value; }
        }

        public string SetNo5
        {
            get { return no5; }
            set { no5 = value; }
        }

        public Ticket()
        {
            InitializeComponent(); 
        }

        private void Ticket_Load(object sender, EventArgs e)
        {
            textBox11.Text = name;
            textBox1.Text = no1;
            textBox2.Text = no2;
            textBox3.Text = no3;
            textBox4.Text = no4;
            textBox5.Text = no5;

        }
    }
}
