using MDW_wf.Connectivity;
using MDW_wf.Controller;
using HTKLibrary.Comunications;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HTKLibrary.Classes.MDW;

namespace MDW_wf
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void lbExit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Application.Exit();
        }

        private void btLogin_Click(object sender, EventArgs e)
        {
            Program.configManager.User = tbEmail.Text;
            Program.configManager.Password = tbPass.Text;

            this.Close();
        }

     
    }
}
