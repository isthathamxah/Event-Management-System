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
    public partial class Organizer_Dashboard : Form
    {
        public Organizer_Dashboard()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //go to manage tickets
            this.Close();
            ManageTickets manageTickets = new ManageTickets();
            manageTickets.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //open login form
            this.Close();
            Login login = new Login();
            login.Show();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            //back to landing page
            this.Close();
            LandingPage landing = new LandingPage();
            landing.Show();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //go to craete Event
            this.Close();
            CreateEvent createEvent = new CreateEvent();
            createEvent.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //go to manage attendee
            this.Close();
            ManageAttendee manageAttendee = new ManageAttendee();
            manageAttendee.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //go to manage Vendors
            this.Close();
            ManageVendors manageVendors = new ManageVendors();
            manageVendors.Show();

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
    }
}
