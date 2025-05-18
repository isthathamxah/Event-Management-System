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
    public partial class MonitorFeedback : Form
    {
        // Connection string to your database
        private string connectionString = "Data Source=DESKTOP-Q3RNHLM;Initial Catalog=Project;Integrated Security=True;TrustServerCertificate=True";


        public MonitorFeedback()
        {
            InitializeComponent();
        }

        // Back button functionality
        private void button6_Click(object sender, EventArgs e)
        {
            // Navigate back to admin dashboard
            this.Close();
            Admin_Dashboard admin = new Admin_Dashboard();
            admin.Show();
        }

        // Exit button functionality
        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();  // Exit the application
        }

        // Load feedbacks based on selected filter (Name, Email, Id)
        private void LoadFeedbacks()
        {
            string query = "SELECT * FROM Feedbacks";

            // Adding filter based on comboBox and textBox input
            if (!string.IsNullOrEmpty(textBox4.Text) && comboBox1.SelectedItem != null)
            {
                string filterField = comboBox1.SelectedItem.ToString();
                query += $" WHERE {filterField} LIKE @filter";
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, conn);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@filter", "%" + textBox4.Text + "%");

                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable; // Binding data to DataGridView
            }
        }

        // Search button or textbox input event for feedback filtering
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            LoadFeedbacks();  // Reload feedbacks based on filter text change
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadFeedbacks();  // Reload feedbacks when field selection changes
        }

        // Delete feedback based on FeedbackID
        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Please enter a FeedbackID to delete.");
                return;
            }

            int feedbackID;
            if (int.TryParse(textBox2.Text, out feedbackID))
            {
                DeleteFeedback(feedbackID);
            }
            else
            {
                MessageBox.Show("Invalid FeedbackID. Please enter a valid numeric value.");
            }
        }

        // Function to delete a feedback from the database
        private void DeleteFeedback(int feedbackID)
        {
            string query = "DELETE FROM Feedbacks WHERE FeedbackID = @FeedbackID";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@FeedbackID", feedbackID);

                conn.Open();
                int rowsAffected = command.ExecuteNonQuery();
                conn.Close();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Feedback deleted successfully.");
                    LoadFeedbacks();  // Refresh the DataGrid after deletion
                }
                else
                {
                    MessageBox.Show("No feedback found with the given ID.");
                }
            }
        }


        /*private void button7_Click(object sender, EventArgs e)
        {
            // Close the application or form
            this.Close();
        }*/

        // Form load event to initialize and load data on form load
        private void MonitorFeedback_Load(object sender, EventArgs e)
        {
            LoadFeedbacks();  // Load all feedbacks initially

            // Set default items in comboBox
            comboBox1.Items.AddRange(new object[] { "Name", "Email", "Id" });
        }
    }
}
