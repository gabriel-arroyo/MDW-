using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MDW_wf
{
    public partial class ConnectingForm : Form
    {
        public string Antenna = "";
        public ConnectingForm(string antenna)
        {
            InitializeComponent();
            Antenna = antenna;
        }

        private void ConnectingForm_Load(object sender, EventArgs e)
        {
            //lbAntenna.Text = Antenna;
        }
       
    }
}
