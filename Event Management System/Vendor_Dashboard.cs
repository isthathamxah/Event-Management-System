using Event;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Event_Management_System
{
    public partial class Vendor_Dashboard : Form
    {
        public Vendor_Dashboard()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //back to 
            this.Close();
            Login login = new Login();
            login.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //go to landing
            this.Close();
            LandingPage landing = new LandingPage();
            landing.Show();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //go to landing
            this.Close();

            ServiceBidding bidding = new ServiceBidding();
            bidding.Show();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //go to landing
            this.Close();


            ManageContract manageContract = new ManageContract();
            manageContract.Show();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //go to landing
            this.Close();
            ManageProfile manageProfile = new ManageProfile();
            manageProfile.Show();

        }
    }
}
