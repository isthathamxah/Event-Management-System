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
    public partial class Feedback : Form
    {
        // Connection string to connect to your database
        string connectionString = "Data Source=DESKTOP-Q3RNHLM;Initial Catalog=Project;Integrated Security=True;TrustServerCertificate=True";

        public Feedback()
        {
            InitializeComponent();
        }

        // Load events into the left-side grid when the form is loaded
        private void Feedback_Load(object sender, EventArgs e)
        {
            LoadEvents();
        }

        // Method to load events from the database into the DataGridView
        private void LoadEvents()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT EventID, EventTitle, Location, StartDate, EndDate FROM CreateEvent";
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, conn);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    // Bind the data to the DataGridView
                    dataGridViewEvents.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading events: " + ex.Message);
            }
        }

        // Back button click event (to go to Attendee Dashboard)
        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            Attendee_Dashboard attendee = new Attendee_Dashboard();
            attendee.Show();
        }

        // Submit feedback button click event
        private void button4_Click(object sender, EventArgs e)
        {
            // Get values from form fields
            int attendeeID = Convert.ToInt32(textBox7.Text);  // Attendee ID
            int eventID = Convert.ToInt32(textBox2.Text);     // Event ID
            int rating = Convert.ToInt32(numericUpDown1.Value);  // Rating
            string comments = richTextBox1.Text;  // Comments

            // Validate input
            if (attendeeID == 0 || eventID == 0 || rating == 0 || string.IsNullOrEmpty(comments))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // Insert feedback into the database
            InsertFeedback(attendeeID, eventID, rating, comments);
        }

        // Method to insert feedback into the database
        private void InsertFeedback(int attendeeID, int eventID, int rating, string comments)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "INSERT INTO Feedback2 (AttendeeID, EventID, Rating, Comments) " +
                                   "VALUES (@AttendeeID, @EventID, @Rating, @Comments)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Adding parameters to prevent SQL Injection
                        cmd.Parameters.AddWithValue("@AttendeeID", attendeeID);
                        cmd.Parameters.AddWithValue("@EventID", eventID);
                        cmd.Parameters.AddWithValue("@Rating", rating);
                        cmd.Parameters.AddWithValue("@Comments", comments);

                        // Execute the query
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Feedback submitted successfully!");
                        }
                        else
                        {
                            MessageBox.Show("Error submitting feedback.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
    }
}
