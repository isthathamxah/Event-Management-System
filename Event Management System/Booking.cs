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

namespace Event_Management_System
{
    public partial class Booking : Form
    {
        // Connection string to your database
        string connectionString = "Data Source=DESKTOP-Q3RNHLM;Initial Catalog=Project;Integrated Security=True;TrustServerCertificate=True";

        public Booking()
        {
            InitializeComponent();
            LoadBookingData();
        }

        // Method to load data into DataGridView
        private void LoadBookingData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Load Event Summary and Available Tickets
                    string query1 = "SELECT EventSummary, AvailableTickets FROM Booking";
                    SqlDataAdapter adapter1 = new SqlDataAdapter(query1, connection);
                    DataTable dt1 = new DataTable();
                    adapter1.Fill(dt1);
                    dataGridView1.DataSource = dt1;

                    // Load Ticket Types and Total Tickets
                    string query2 = "SELECT ChooseType, TotalTickets FROM Booking";
                    SqlDataAdapter adapter2 = new SqlDataAdapter(query2, connection);
                    DataTable dt2 = new DataTable();
                    adapter2.Fill(dt2);
                    dataGridView2.DataSource = dt2;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // Add functionality for pictureBox click if required
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {
            // Add functionality for splitContainer paint if required
        }

        // Navigate to the Attendee Dashboard
        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
            Attendee_Dashboard attendee = new Attendee_Dashboard();
            attendee.Show();
        }

        // Handle booking and navigate to Payment
        private void button4_Click(object sender, EventArgs e)
        {
            // Validate user input
            if (checkedListBox1.CheckedItems.Count == 0 || numericUpDown1.Value <= 0)
            {
                MessageBox.Show("Please select a ticket type and specify the total tickets.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string ticketType = checkedListBox1.CheckedItems[0].ToString();
            int totalTickets = (int)numericUpDown1.Value;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Insert booking into the database
                    string insertQuery = "INSERT INTO Booking (EventSummary, AvailableTickets, ChooseType, TotalTickets) " +
                                         "VALUES (@EventSummary, @AvailableTickets, @ChooseType, @TotalTickets)";

                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                    insertCommand.Parameters.AddWithValue("@EventSummary", "Sample Event"); // Replace with selected event summary
                    insertCommand.Parameters.AddWithValue("@AvailableTickets", 100); // Replace with remaining tickets logic
                    insertCommand.Parameters.AddWithValue("@ChooseType", ticketType);
                    insertCommand.Parameters.AddWithValue("@TotalTickets", totalTickets);
                    insertCommand.ExecuteNonQuery();

                    // Update available tickets
                    string updateQuery = "UPDATE Booking SET AvailableTickets = AvailableTickets - @BookedTickets WHERE EventSummary = @EventSummary";
                    SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                    updateCommand.Parameters.AddWithValue("@BookedTickets", totalTickets);
                    updateCommand.Parameters.AddWithValue("@EventSummary", "Sample Event"); // Replace with dynamic value
                    updateCommand.ExecuteNonQuery();

                    MessageBox.Show("Booking successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refresh the data
                    LoadBookingData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error processing booking: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Navigate to Payment form
            this.Close();
            Payment payment = new Payment();
            payment.Show();
        }
    }
}
