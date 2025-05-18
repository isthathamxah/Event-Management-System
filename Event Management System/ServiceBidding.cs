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
    public partial class ServiceBidding : Form
    {
        public ServiceBidding()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Show the OpenFileDialog
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Get the selected file path
                string filePath = openFileDialog1.FileName;

                // Display the selected file path in a MessageBox (or save it as needed)
                MessageBox.Show("Selected File: " + filePath, "File Selection");
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            //go to vendor dashbiard
            this.Close();
            Vendor_Dashboard vd = new Vendor_Dashboard();
            vd.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //go to vendor dashbiard
            this.Close();

        }


    }
}

