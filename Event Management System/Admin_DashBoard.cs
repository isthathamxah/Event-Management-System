using Event;
using Event_Management_System;
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
    public partial class Admin_Dashboard : Form
    {
        public Admin_Dashboard()
        {
            InitializeComponent();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
            LandingPage landing = new LandingPage();
            landing.Show();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            // go to admin login
            this.Close();
            Login login = new Login();
            login.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // go to manage organier
            this.Close();
            ManageOrganizer manage = new ManageOrganizer();
            manage.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Close();
            ManageAttendee manage = new ManageAttendee();
            manage.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //go to manageevents
            this.Close();
            ManageEvents manage = new ManageEvents();
            manage.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //go to monitor feedback
            this.Close();
            MonitorFeedback monitor = new MonitorFeedback();
            monitor.Show();
        }
    }
}
