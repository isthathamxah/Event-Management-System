using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
// Ensure these using directives are correctly referenced in your project.
// You might need to add references to System.Windows.Forms.VisualStyles for these to resolve,
// but they are often not strictly necessary for basic functionality and can sometimes be removed.
// using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
// using static System.Windows.Forms.VisualStyles.VisualStyleElement;
// Ensure Event_Management_System is a valid namespace in your solution if it's used elsewhere.
// If it's just for the Attendee_Dashboard class, then 'using Event_Management_System;' might be enough if Event_Management_System
// is the namespace where Attendee_Dashboard is defined.
using Event_Management_System;

namespace Event
{
    public partial class EventDashboard : Form
    {
        // Connection string for SQL Server (update with your actual connection details)
        // IMPORTANT: Replace "DESKTOP-Q3RNHLM" with your actual SQL Server instance name or IP.
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

            // FIX: Ensure location is not null. If no item is selected, use an empty string.
            string location = comboBox1.SelectedItem?.ToString() ?? string.Empty;

            decimal minPrice = numericUpDown1.Value;
            decimal maxPrice = numericUpDown2.Value;
            bool isTicketAvailable = checkBox1.Checked;

            // IMPORTANT: Review where 'category' should come from.
            // If textBox1 is for 'keyword', and 'category' is a separate field,
            // you should have a dedicated UI control for 'category' (e.g., another TextBox or ComboBox).
            // For now, it's set to an empty string to avoid reusing textBox1.Text and potential confusion.
            // Replace with the correct UI control's value if applicable (e.g., 'textBoxCategory.Text').
            string category = string.Empty;

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
                    // After saving, refresh the DataGridView to show the new data
                    BindEventData();
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
            // checkedListBox1.ClearSelected(); // This might not be the correct way to clear a CheckedListBox.
            // Consider clearing items or unchecking all items if that's the intent.
            if (checkedListBox1.Items.Count > 0) // Example of how to uncheck all items
            {
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i, false);
                }
            }

            comboBox1.SelectedIndex = -1; // Deselects any item in the ComboBox
            numericUpDown1.Value = numericUpDown1.Minimum;
            numericUpDown2.Value = numericUpDown2.Minimum;
            checkBox1.Checked = false;
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            // textBox1.Clear(); // This is redundant as it's already cleared above
            textBox2.Clear();
            // After clearing, you might want to refresh the DataGridView if it's tied to filters
            // but this method is for clearing input fields, not necessarily resetting displayed data.
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
                    MessageBox.Show($"Error loading event data: {ex.Message}");
                }
            }
        }

        // Form Load Event
        private void EventDashboard_Load(object sender, EventArgs e)
        {
            // Example: Initialize comboBox1 with data (could be from a database or predefined list)
            comboBox1.Items.AddRange(new string[] { "New York", "London", "Paris", "Online" });

            // Example: Set default values for numericUpDown controls
            numericUpDown1.Value = 0; // Set default minimum price to 0
            numericUpDown2.Value = 500; // Set a reasonable default maximum price

            // Load initial data into DataGridView when the form loads
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
            Booking booking = new Booking(); // Ensure 'Booking' class is defined and accessible
            booking.Show();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {
            // Custom drawing logic (if any) for splitContainer1_Panel1
        }
    }
}