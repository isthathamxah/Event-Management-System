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

namespace Event
{
    public partial class Attendee_Dashboard : Form
    {
        public Attendee_Dashboard()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
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
            //back to attendee login
            this.Close();
            Login login = new Login();
            login.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //go to event dashboard
            this.Close();
            EventDashboard eventDashboard = new EventDashboard();
            eventDashboard.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //go to book event
            this.Close();
            Booking booking = new Booking();
            booking.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //go to payment
            this.Close();
            Payment payment = new Payment();
            payment.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //go to feedback
            this.Close();
            Feedback feedback = new Feedback();
            feedback.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //go to manage profile
            this.Close();
            ManageProfile manageProfile = new ManageProfile();
            manageProfile.Show();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }
    }
}
