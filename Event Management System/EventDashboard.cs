/*using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EMS
{
    public partial class EventDashboard : Form
    {
        public EventDashboard()
        {
            InitializeComponent();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            //go to attendee dashboard
            this.Close();
            Attendee_Dashboard attendee = new Attendee_Dashboard();
            attendee.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
        }

        private void button4_Click_1(object sender, EventArgs e)
        {

            //go to booking page
            this.Close();
            Booking booking = new Booking();
            booking.Show();
        }
    }
}
*/



using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Event_Management_System;

namespace Event
{
    public partial class EventDashboard : Form
    {
        // Connection string for SQL Server (update with your actual connection details)
        string connectionString = "Data Source=DESKTOP-Q3RNHLM;Initial Catalog=Project;Integrated Security=True;TrustServerCertificate=True";

        public EventDashboard()
        {
            InitializeComponent();
        }

        // Button click event for "Back"
        private void button6_Click(object sender, EventArgs e)
        {
            // Handle back button click event (e.g., navigate to previous screen)
            this.Close();
            Attendee_Dashboard attendee_Dashboard = new Attendee_Dashboard();
            attendee_Dashboard.Show();
            // Or navigate to another form
        }

        // Button click event for "Exit"
        private void button7_Click(object sender, EventArgs e)
        {
            // Handle exit button click event
            Application.Exit();
        }

        // Button click event for "Apply"
        private void button1_Click(object sender, EventArgs e)
        {
            // Apply button logic (example: save data, apply filters)
            string keyword = textBox1.Text;
            DateTime startDate = dateTimePicker1.Value;
            DateTime endDate = dateTimePicker2.Value;
            string location = comboBox1.SelectedItem?.ToString();
            decimal minPrice = numericUpDown1.Value;
            decimal maxPrice = numericUpDown2.Value;
            bool isTicketAvailable = checkBox1.Checked;
            string category = textBox1.Text;
            string organizer = textBox2.Text;

            // Save event data to the database
            SaveEventData(keyword, location, startDate, endDate, minPrice, maxPrice, isTicketAvailable, category, organizer);
        }

        // Save event data to the database
        private void SaveEventData(string keyword, string location, DateTime startDate, DateTime endDate, decimal minPrice, decimal maxPrice, bool isTicketAvailable, string category, string organizer)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL query to insert the event data
                    string query = "INSERT INTO EventFilters (Keyword, Location, StartDate, EndDate, MinPrice, MaxPrice, IsTicketAvailable, Category, Organizer) " +
                                   "VALUES (@Keyword, @Location, @StartDate, @EndDate, @MinPrice, @MaxPrice, @IsTicketAvailable, @Category, @Organizer)";
                    SqlCommand cmd = new SqlCommand(query, connection);

                    // Add parameters to prevent SQL injection
                    cmd.Parameters.AddWithValue("@Keyword", keyword);
                    cmd.Parameters.AddWithValue("@Location", location);
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate);
                    cmd.Parameters.AddWithValue("@MinPrice", minPrice);
                    cmd.Parameters.AddWithValue("@MaxPrice", maxPrice);
                    cmd.Parameters.AddWithValue("@IsTicketAvailable", isTicketAvailable);
                    cmd.Parameters.AddWithValue("@Category", category);
                    cmd.Parameters.AddWithValue("@Organizer", organizer);

                    // Execute the query
                    int rowsAffected = cmd.ExecuteNonQuery();

                    // Notify the user
                    MessageBox.Show($"{rowsAffected} row(s) inserted successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        // Button click event for "Clear All"
        private void button3_Click(object sender, EventArgs e)
        {
            // Clear all form fields
            textBox1.Clear();
            checkedListBox1.ClearSelected();
            comboBox1.SelectedIndex = -1;
            numericUpDown1.Value = numericUpDown1.Minimum;
            numericUpDown2.Value = numericUpDown2.Minimum;
            checkBox1.Checked = false;
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            textBox1.Clear();
            textBox2.Clear();
        }

        // DataGridView DataBinding (example for displaying event data)
        private void BindEventData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL query to select event data
                    string query = "SELECT EventID, Keyword, Location, StartDate, EndDate, MinPrice, MaxPrice, IsTicketAvailable, Category, Organizer FROM EventFilters";
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);

                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    // Bind data to DataGridView
                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        // Form Load Event
        private void EventDashboard_Load(object sender, EventArgs e)
        {
            // Example: Initialize comboBox1 with data (could be from a database or predefined list)
            comboBox1.Items.AddRange(new string[] { "Location1", "Location2", "Location3" });

            // Example: Set default values for numericUpDown controls
            numericUpDown1.Value = 10; // Set default minimum price
            numericUpDown2.Value = 100; // Set default maximum price

            // You can also call other methods here like BindEventData to load initial data into DataGridView
            BindEventData();
        }

        // Optionally, you can override the form closing behavior
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            // Additional cleanup if needed before the form closes
        }

        // Additional event handlers, e.g., for button4_Click_1, can go here if needed
        private void button4_Click_1(object sender, EventArgs e)
        {

            //go to booking page
            this.Close();
            Booking booking = new Booking();
            booking.Show();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {
            // Custom drawing logic
        }


    }
}
