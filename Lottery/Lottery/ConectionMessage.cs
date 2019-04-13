﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lottery
    {
    public partial class ConectionMessage : Form
        {

        public ConectionMessage ()
            {
            InitializeComponent();
            TopMost = true;
            }

        private void ConectionMessage_Load (object sender, EventArgs e)
            {
            
            timer1.Enabled = true;
            timer1.Start();
            timer1.Interval = 1000;
            progressBar1.Maximum = 15;
            timer1.Tick += new EventHandler(timer1_Tick);

            }

        void timer1_Tick(object sender, EventArgs e)
            {
            if (progressBar1.Value !=15)
                {
                progressBar1.Value++;
                
                }
            else
                {
                timer1.Stop();
                this.Close();
                }
            }
        }
    }
