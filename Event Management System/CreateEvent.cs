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
    public partial class CreateEvent : Form
    {
        // Connection string for the database
        string connectionString = "Data Source=DESKTOP-Q3RNHLM;Initial Catalog=Project;Integrated Security=True;TrustServerCertificate=True";

        public CreateEvent()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Event handler for the Add button
            string eventTitle = textBox1.Text;
            string description = textBox2.Text;
            string location = textBox3.Text;
            DateTime startDate = dateTimePicker1.Value;
            DateTime endDate = dateTimePicker2.Value;

            if (string.IsNullOrWhiteSpace(eventTitle) || string.IsNullOrWhiteSpace(description) || string.IsNullOrWhiteSpace(location))
            {
                MessageBox.Show("Please fill out all fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (startDate > endDate)
            {
                MessageBox.Show("Start date cannot be later than the end date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Save event details to the database
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO CreateEvent (EventTitle, Description, Location, StartDate, EndDate) " +
                                   "VALUES (@EventTitle, @Description, @Location, @StartDate, @EndDate)";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@EventTitle", eventTitle);
                    command.Parameters.AddWithValue("@Description", description);
                    command.Parameters.AddWithValue("@Location", location);
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show($"Event '{eventTitle}' created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearFields(); // Clear fields after successful submission
                    }
                    else
                    {
                        MessageBox.Show("Failed to create the event. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Event handler for the Clear All Fields button
            ClearFields();
        }

        private void ClearFields()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // Add your logic for the picture box click event
            MessageBox.Show("PictureBox clicked.");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
            Organizer_Dashboard od = new Organizer_Dashboard();
            od.Show();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // Add your logic for the label click event (if necessary)
            MessageBox.Show("Label clicked.");
        }

    }
}
