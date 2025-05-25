using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Event_Management_System
{
    public partial class ServiceBidding : Form
    {
        // Connection string - Update this with your database connection
        private string connectionString = "Data Source=DESKTOP-Q3RNHLM;Initial Catalog=Project;Integrated Security=True;TrustServerCertificate=True";

        // Alternative simpler connection string for testing
        // private string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\EventManagement.mdf;Integrated Security=True";
        private string selectedFilePath = "";

        public ServiceBidding()
        {
            InitializeComponent();
            LoadBidsData();
            SetupForm();
        }

        private void SetupForm()
        {
            // Set initial values
            textBox2.Text = "";
            textBox2.ForeColor = Color.Gray;
            textBox2.Enter += TextBox2_Enter;
            textBox2.Leave += TextBox2_Leave;

            // Set placeholder text
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                textBox2.Text = "Search bids...";
                textBox2.ForeColor = Color.Gray;
            }

            // Set minimum date for DateTimePicker
            dateTimePicker1.MinDate = DateTime.Now;

            // Configure DataGridView
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void TextBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "Search bids..." && textBox2.ForeColor == Color.Gray)
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
            }
        }

        private void TextBox2_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                textBox2.Text = "Search bids...";
                textBox2.ForeColor = Color.Gray;
            }
        }

        private void LoadBidsData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT BidID, EventID, ServiceDescription, DeliveryTime, 
                                   AttachedFile, SubmissionDate FROM SponsorBids 
                                   ORDER BY SubmissionDate DESC";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridView1.DataSource = dt;

                    // Format columns
                    if (dataGridView1.Columns["BidID"] != null)
                        dataGridView1.Columns["BidID"].HeaderText = "Bid ID";
                    if (dataGridView1.Columns["EventID"] != null)
                        dataGridView1.Columns["EventID"].HeaderText = "Event ID";
                    if (dataGridView1.Columns["ServiceDescription"] != null)
                        dataGridView1.Columns["ServiceDescription"].HeaderText = "Service Description";
                    if (dataGridView1.Columns["DeliveryTime"] != null)
                        dataGridView1.Columns["DeliveryTime"].HeaderText = "Delivery Time";
                    if (dataGridView1.Columns["AttachedFile"] != null)
                        dataGridView1.Columns["AttachedFile"].HeaderText = "Attached File";
                    if (dataGridView1.Columns["SubmissionDate"] != null)
                        dataGridView1.Columns["SubmissionDate"].HeaderText = "Submission Date";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Database Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Submit Bid button
            try
            {
                MessageBox.Show("Submit button clicked!", "Debug"); // Debug message

                if (ValidateForm())
                {
                    SubmitBid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in button1_Click: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateForm()
        {
            // Validate Event ID
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please enter an Event ID.", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return false;
            }

            if (!int.TryParse(textBox1.Text, out int eventId) || eventId <= 0)
            {
                MessageBox.Show("Please enter a valid Event ID (positive number).", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return false;
            }

            // Validate Service Description
            if (string.IsNullOrWhiteSpace(richTextBox1.Text))
            {
                MessageBox.Show("Please enter a service description.", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                richTextBox1.Focus();
                return false;
            }

            // Validate Delivery Time
            if (dateTimePicker1.Value <= DateTime.Now)
            {
                MessageBox.Show("Delivery time must be in the future.", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dateTimePicker1.Focus();
                return false;
            }

            return true;
        }

        private void SubmitBid()
        {
            try
            {
                // Simple test without database first
                MessageBox.Show($"Attempting to submit bid:\n" +
                              $"Event ID: {textBox1.Text}\n" +
                              $"Description: {richTextBox1.Text}\n" +
                              $"Delivery: {dateTimePicker1.Value}\n" +
                              $"File: {selectedFilePath}", "Debug Info");

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Skip event validation for now - comment out this section for testing
                    /*
                    // First check if EventID exists (you might need to adjust this based on your Events table)
                    string checkEventQuery = "SELECT COUNT(*) FROM Events WHERE EventID = @EventID";
                    using (SqlCommand checkCmd = new SqlCommand(checkEventQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@EventID", int.Parse(textBox1.Text));
                        int eventCount = (int)checkCmd.ExecuteScalar();
                        
                        if (eventCount == 0)
                        {
                            MessageBox.Show("Event ID does not exist. Please enter a valid Event ID.", 
                                          "Invalid Event ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    */

                    // Insert the bid
                    string insertQuery = @"INSERT INTO SponsorBids (EventID, ServiceDescription, DeliveryTime, AttachedFile) 
                                         VALUES (@EventID, @ServiceDescription, @DeliveryTime, @AttachedFile)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@EventID", int.Parse(textBox1.Text));
                        cmd.Parameters.AddWithValue("@ServiceDescription", richTextBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@DeliveryTime", dateTimePicker1.Value);
                        cmd.Parameters.AddWithValue("@AttachedFile",
                            string.IsNullOrEmpty(selectedFilePath) ? (object)DBNull.Value : Path.GetFileName(selectedFilePath));

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Bid submitted successfully!", "Success",
                                          MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearForm();
                            LoadBidsData(); // Refresh the grid
                        }
                        else
                        {
                            MessageBox.Show("Failed to submit bid. Please try again.", "Error",
                                          MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database Error: {sqlEx.Message}\n\nPlease check your connection string and database setup.",
                              "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting bid: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearForm()
        {
            textBox1.Clear();
            richTextBox1.Clear();
            dateTimePicker1.Value = DateTime.Now.AddDays(1);
            selectedFilePath = "";
            button3.Text = "Browse File";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Browse File button
            openFileDialog1.Filter = "All Files (*.*)|*.*|PDF Files (*.pdf)|*.pdf|Word Documents (*.docx)|*.docx|Text Files (*.txt)|*.txt";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Title = "Select File to Attach";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                selectedFilePath = openFileDialog1.FileName;
                string fileName = Path.GetFileName(selectedFilePath);

                // Update button text to show selected file
                if (fileName.Length > 15)
                {
                    button3.Text = fileName.Substring(0, 12) + "...";
                }
                else
                {
                    button3.Text = fileName;
                }

                // Show confirmation
                MessageBox.Show($"Selected File: {fileName}", "File Selection",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            // Additional validation can be added here if needed
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Back button - go to vendor dashboard
            this.Hide();
            Vendor_Dashboard vd = new Vendor_Dashboard();
            vd.ShowDialog();
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Exit button
            DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Confirm Exit",
                                                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Search button
            SearchBids();
        }

        private void SearchBids()
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text) || textBox2.Text == "Search bids...")
            {
                LoadBidsData();
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string searchValue = textBox2.Text.Trim();
                    string query = "";

                    switch (comboBox1.Text)
                    {
                        case "Id":
                            query = @"SELECT BidID, EventID, ServiceDescription, DeliveryTime, 
                                    AttachedFile, SubmissionDate FROM SponsorBids 
                                    WHERE BidID = @SearchValue OR EventID = @SearchValue
                                    ORDER BY SubmissionDate DESC";
                            break;
                        case "Email":
                            // If you have vendor email in another table, you'd need to join
                            MessageBox.Show("Email search requires vendor information. Please use ID or Name search.",
                                          "Search Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        case "Name":
                            // Similar to email, would need vendor name from another table
                            MessageBox.Show("Name search requires vendor information. Please use ID search.",
                                          "Search Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        default:
                            query = @"SELECT BidID, EventID, ServiceDescription, DeliveryTime, 
                                    AttachedFile, SubmissionDate FROM SponsorBids 
                                    WHERE ServiceDescription LIKE @SearchValue 
                                    OR CAST(EventID AS VARCHAR) LIKE @SearchValue
                                    OR CAST(BidID AS VARCHAR) LIKE @SearchValue
                                    ORDER BY SubmissionDate DESC";
                            searchValue = $"%{searchValue}%";
                            break;
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    adapter.SelectCommand.Parameters.AddWithValue("@SearchValue", searchValue);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridView1.DataSource = dt;

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No bids found matching your search criteria.", "Search Results",
                                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching bids: {ex.Message}", "Search Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // Logo click - could navigate to main page or show info
            MessageBox.Show("Event Management System\nService Bidding Module", "About",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Add TextChanged event for real-time search (optional)
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // Uncomment the line below if you want real-time search as user types
            // SearchBids();
        }

        // Handle Enter key press in search textbox
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SearchBids();
                e.Handled = true;
            }
        }

        // Add double-click event for DataGridView to view details
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                string bidDetails = $"Bid ID: {row.Cells["BidID"].Value}\n" +
                                  $"Event ID: {row.Cells["EventID"].Value}\n" +
                                  $"Service: {row.Cells["ServiceDescription"].Value}\n" +
                                  $"Delivery Time: {row.Cells["DeliveryTime"].Value}\n" +
                                  $"Submission Date: {row.Cells["SubmissionDate"].Value}";

                MessageBox.Show(bidDetails, "Bid Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Clean up any resources if needed
            base.OnFormClosing(e);
        }
    }
}