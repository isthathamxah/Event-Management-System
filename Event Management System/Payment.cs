using Event;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Event_Management_System
{
    public partial class Payment : Form
    {
        string connectionString = "Data Source=DESKTOP-Q3RNHLM;Initial Catalog=Project;Integrated Security=True;TrustServerCertificate=True";

        public Payment()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Go to attendee dashboard
            this.Close();
            Attendee_Dashboard attendee = new Attendee_Dashboard();
            attendee.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(comboBox1.Text) ||
            string.IsNullOrWhiteSpace(textBox1.Text) ||
            string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            try
            {
                // Prepare the SQL query to insert data into the Payment table
                string query = "INSERT INTO Payment (DueAmount, AccountNumber, CVC, ExpiryDate) " +
                               "VALUES (@DueAmount, @AccountNumber, @CVC, @ExpiryDate)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection
                    connection.Open();

                    // Create the SQL command
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@DueAmount", textBox1.Text);   // Due Amount
                    command.Parameters.AddWithValue("@AccountNumber", textBox2.Text); // Account Number
                    command.Parameters.AddWithValue("@CVC", textBox3.Text);         // CVC
                    command.Parameters.AddWithValue("@ExpiryDate", textBox4.Text);  // Expiry Date

                    // Execute the query
                    command.ExecuteNonQuery();
                }

                // Show success message
                MessageBox.Show("Payment information saved successfully.");

                // Optionally, close the payment form and go to another form
                this.Close();
                /*Ticket_Download ticketDownload = new Ticket_Download(); // Assuming you have a Ticket_Download form
                ticketDownload.Show();*/
            }
            catch (Exception ex)
            {
                // Show an error message if something goes wrong
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
