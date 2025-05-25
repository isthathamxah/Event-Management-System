using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Event_Management_System
{
    public partial class ManageContract : Form
    {
        // Database connection string - Update this with your actual connection string
        private string connectionString = "Data Source=DESKTOP-Q3RNHLM;Initial Catalog=Project;Integrated Security=True;TrustServerCertificate=True";

        public ManageContract()
        {
            InitializeComponent();
            LoadContracts();
            SetupDataGridView();
            WireUpEventHandlers(); // Added this line
        }

        // ADD THIS METHOD TO WIRE UP EVENT HANDLERS
        private void WireUpEventHandlers()
        {
            button1.Click += button1_Click;        // Add button
            button2.Click += button2_Click;        // Update button  
            button3.Click += button3_Click;        // Track Payment button
            pictureBox1.Click += pictureBox1_Click; // Search button
        }

        private void SetupDataGridView()
        {
            // Configure DataGridView for better display
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;

            // Add event handler for row selection
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
        }

        private void LoadContracts()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT EventID, ContractID, VendorID, ContractTerms, StartDate, EndDate FROM ManageContracts ORDER BY ContractID";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading contracts: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Populate form fields with selected contract data
                textBox1.Text = selectedRow.Cells["EventID"].Value?.ToString() ?? "";
                textBox3.Text = selectedRow.Cells["ContractID"].Value?.ToString() ?? "";
                textBox4.Text = selectedRow.Cells["VendorID"].Value?.ToString() ?? "";
                textBox5.Text = selectedRow.Cells["ContractTerms"].Value?.ToString() ?? "";

                if (DateTime.TryParse(selectedRow.Cells["StartDate"].Value?.ToString(), out DateTime startDate))
                    dateTimePicker1.Value = startDate;

                if (DateTime.TryParse(selectedRow.Cells["EndDate"].Value?.ToString(), out DateTime endDate))
                    dateTimePicker2.Value = endDate;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Add new contract
            if (!ValidateInputs())
            {
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if ContractID already exists
                    string checkQuery = "SELECT COUNT(*) FROM ManageContracts WHERE ContractID = @ContractID";
                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@ContractID", int.Parse(textBox3.Text));
                        int count = (int)checkCommand.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show("Contract ID already exists. Please use a different Contract ID.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    string query = @"INSERT INTO ManageContracts (EventID, ContractID, VendorID, ContractTerms, StartDate, EndDate) 
                                   VALUES (@EventID, @ContractID, @VendorID, @ContractTerms, @StartDate, @EndDate)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EventID", int.Parse(textBox1.Text));
                        command.Parameters.AddWithValue("@ContractID", int.Parse(textBox3.Text));
                        command.Parameters.AddWithValue("@VendorID", int.Parse(textBox4.Text));
                        command.Parameters.AddWithValue("@ContractTerms", textBox5.Text);
                        command.Parameters.AddWithValue("@StartDate", dateTimePicker1.Value.Date);
                        command.Parameters.AddWithValue("@EndDate", dateTimePicker2.Value.Date);

                        int result = command.ExecuteNonQuery();

                        if (result > 0)
                        {
                            MessageBox.Show("Contract added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadContracts();
                            ClearFields();
                        }
                        else
                        {
                            MessageBox.Show("Failed to add contract. No rows affected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database Error: {sqlEx.Message}\nError Number: {sqlEx.Number}", "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding contract: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Update existing contract
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a contract to update.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInputs())
            {
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"UPDATE ManageContracts 
                                   SET EventID = @EventID, VendorID = @VendorID, ContractTerms = @ContractTerms, 
                                       StartDate = @StartDate, EndDate = @EndDate 
                                   WHERE ContractID = @ContractID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EventID", int.Parse(textBox1.Text));
                        command.Parameters.AddWithValue("@ContractID", int.Parse(textBox3.Text));
                        command.Parameters.AddWithValue("@VendorID", int.Parse(textBox4.Text));
                        command.Parameters.AddWithValue("@ContractTerms", textBox5.Text);
                        command.Parameters.AddWithValue("@StartDate", dateTimePicker1.Value.Date);
                        command.Parameters.AddWithValue("@EndDate", dateTimePicker2.Value.Date);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Contract updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadContracts();
                        }
                        else
                        {
                            MessageBox.Show("No contract was updated. Please check the Contract ID.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database Error: {sqlEx.Message}\nError Number: {sqlEx.Number}", "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating contract: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Track Payment - Enhanced functionality
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a contract to track payment.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                string contractID = selectedRow.Cells["ContractID"].Value?.ToString();
                string vendorID = selectedRow.Cells["VendorID"].Value?.ToString();
                string eventID = selectedRow.Cells["EventID"].Value?.ToString();
                string contractTerms = selectedRow.Cells["ContractTerms"].Value?.ToString();

                // Create a more detailed payment tracking message
                string paymentInfo = $"PAYMENT TRACKING DETAILS\n\n" +
                                   $"Contract ID: {contractID}\n" +
                                   $"Event ID: {eventID}\n" +
                                   $"Vendor ID: {vendorID}\n" +
                                   $"Contract Terms: {contractTerms}\n\n" +
                                   $"Start Date: {selectedRow.Cells["StartDate"].Value}\n" +
                                   $"End Date: {selectedRow.Cells["EndDate"].Value}\n\n" +
                                   $"Status: Active\n" +
                                   $"Payment Status: Pending Review\n\n" +
                                   $"Note: This feature can be expanded to integrate with\n" +
                                   $"payment processing systems and show real-time payment status.";

                MessageBox.Show(paymentInfo, "Payment Tracking", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error tracking payment: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Search functionality
            SearchContracts();
        }

        private void SearchContracts()
        {
            string searchTerm = textBox2.Text.Trim();
            string searchField = comboBox1.Text;

            if (string.IsNullOrEmpty(searchTerm) || searchTerm == "Input..." || searchField == "Choose field")
            {
                LoadContracts(); // Load all contracts if no search criteria
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "";

                    switch (searchField)
                    {
                        case "Id":
                            query = "SELECT EventID, ContractID, VendorID, ContractTerms, StartDate, EndDate FROM ManageContracts WHERE ContractID = @SearchTerm OR VendorID = @SearchTerm OR EventID = @SearchTerm";
                            break;
                        case "Name": // Assuming this searches contract terms
                            query = "SELECT EventID, ContractID, VendorID, ContractTerms, StartDate, EndDate FROM ManageContracts WHERE ContractTerms LIKE @SearchTerm";
                            searchTerm = "%" + searchTerm + "%";
                            break;
                        case "Email": // You might want to add email field to your database
                            MessageBox.Show("Email search is not available for contracts. Please use Id or Name.", "Search Option Not Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        default:
                            MessageBox.Show("Please select a valid search field.", "Invalid Search", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.SelectCommand.Parameters.AddWithValue("@SearchTerm", searchTerm);

                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;

                    if (dataTable.Rows.Count == 0)
                    {
                        MessageBox.Show("No contracts found matching your search criteria.", "No Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Found {dataTable.Rows.Count} contract(s) matching your search.", "Search Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching contracts: " + ex.Message, "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInputs()
        {
            // Validate Event ID
            if (string.IsNullOrWhiteSpace(textBox1.Text) || !int.TryParse(textBox1.Text, out _))
            {
                MessageBox.Show("Please enter a valid Event ID (numbers only).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return false;
            }

            // Validate Contract ID
            if (string.IsNullOrWhiteSpace(textBox3.Text) || !int.TryParse(textBox3.Text, out _))
            {
                MessageBox.Show("Please enter a valid Contract ID (numbers only).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox3.Focus();
                return false;
            }

            // Validate Vendor ID
            if (string.IsNullOrWhiteSpace(textBox4.Text) || !int.TryParse(textBox4.Text, out _))
            {
                MessageBox.Show("Please enter a valid Vendor ID (numbers only).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox4.Focus();
                return false;
            }

            // Validate Contract Terms
            if (string.IsNullOrWhiteSpace(textBox5.Text))
            {
                MessageBox.Show("Please enter contract terms.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox5.Focus();
                return false;
            }

            // Validate dates
            if (dateTimePicker2.Value <= dateTimePicker1.Value)
            {
                MessageBox.Show("End date must be after start date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dateTimePicker2.Focus();
                return false;
            }

            // Validate date is not in the past for new contracts
            if (dateTimePicker1.Value.Date < DateTime.Now.Date)
            {
                DialogResult result = MessageBox.Show("Start date is in the past. Do you want to continue?", "Date Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                {
                    dateTimePicker1.Focus();
                    return false;
                }
            }

            return true;
        }

        private void ClearFields()
        {
            textBox1.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now.AddDays(30); // Default to 30 days later
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Event handler for EventID textbox changes
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            // Clear placeholder text when entering search box
            if (textBox2.Text == "Input...")
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            // Restore placeholder text if search box is empty
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                textBox2.Text = "Input...";
                textBox2.ForeColor = Color.Gray;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Go back to Vendor Dashboard
            this.Close();
            try
            {
                Vendor_Dashboard vendor_Dashboard = new Vendor_Dashboard();
                vendor_Dashboard.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening Vendor Dashboard: " + ex.Message, "Navigation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Exit application with confirmation
            DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Exit Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void ManageContract_Load(object sender, EventArgs e)
        {
            // Form load event - setup initial state
            ClearFields();

            // Setup search textbox placeholder
            textBox2.Text = "Input...";
            textBox2.ForeColor = Color.Gray;
            textBox2.Enter += textBox2_Enter;
            textBox2.Leave += textBox2_Leave;

            // Set default combo box selection
            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;

            // Test database connection on form load
            TestDatabaseConnection();
        }

        // ADD THIS METHOD TO TEST DATABASE CONNECTION
        private void TestDatabaseConnection()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // Connection successful - no need to show message unless there's an error
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database connection failed: {ex.Message}\n\nPlease check your connection string and database availability.",
                              "Database Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}