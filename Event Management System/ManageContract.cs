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
    public partial class ManageContract : Form
    {
        public ManageContract()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            //go to landing
            this.Close();
            Vendor_Dashboard vendor_Dashboard = new Vendor_Dashboard();
            vendor_Dashboard.Show();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            //go to landing
            this.Close();




        }
    }
}
