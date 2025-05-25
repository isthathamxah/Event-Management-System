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
    public partial class ManageVendors : Form
    {
        // Connection string - update this with your database connection details
        private string connectionString = "Data Source=DESKTOP-Q3RNHLM;Initial Catalog=Project;Integrated Security=True;TrustServerCertificate=True";
        private int selectedVendorId = -1;

        public ManageVendors()
        {
            InitializeComponent();
            LoadVendors();
            SetupDataGridView();
        }

        private void SetupDataGridView()
        {
            // Configure DataGridView for better user experience
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;

            // Handle row selection
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Populate form fields with selected vendor data
                textBox7.Text = selectedRow.Cells["EventID"].Value?.ToString() ?? "";
                textBox2.Text = selectedRow.Cells["VendorID"].Value?.ToString() ?? "";
                textBox6.Text = selectedRow.Cells["SponsorID"].Value?.ToString() ?? "";

                // Handle services offered (assuming it's stored as comma-separated values)
                string services = selectedRow.Cells["ServicesOffered"].Value?.ToString() ?? "";
                SetSelectedServices(services);

                // Handle staff allocation
                if (selectedRow.Cells["AllocateStaff"] != null &&
                    int.TryParse(selectedRow.Cells["AllocateStaff"].Value?.ToString(), out int staff))
                {
                    numericUpDown1.Value = staff;
                }

                // Store the selected vendor ID for update/delete operations
                if (int.TryParse(selectedRow.Cells["VendorID"].Value?.ToString(), out int vendorId))
                {
                    selectedVendorId = vendorId;
                }
            }
        }

        private void SetSelectedServices(string services)
        {
            // Clear all selections first
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, false);
            }

            if (!string.IsNullOrEmpty(services))
            {
                string[] serviceArray = services.Split(',');
                foreach (string service in serviceArray)
                {
                    string trimmedService = service.Trim().ToLower();
                    for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    {
                        if (checkedListBox1.Items[i].ToString().ToLower() == trimmedService)
                        {
                            checkedListBox1.SetItemChecked(i, true);
                            break;
                        }
                    }
                }
            }
        }

        private string GetSelectedServices()
        {
            List<string> selectedServices = new List<string>();
            foreach (string item in checkedListBox1.CheckedItems)
            {
                selectedServices.Add(item);
            }
            return string.Join(", ", selectedServices);
        }

        private void LoadVendors()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"SELECT VendorID, EventID, ServicesOffered, SponsorID, AllocateStaff
                                   FROM Vendors ORDER BY VendorID";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;

                    // Optionally format column headers
                    if (dataGridView1.Columns.Count > 0)
                    {
                        dataGridView1.Columns["VendorID"].HeaderText = "Vendor ID";
                        dataGridView1.Columns["EventID"].HeaderText = "Event ID";
                        dataGridView1.Columns["ServicesOffered"].HeaderText = "Services Offered";
                        dataGridView1.Columns["SponsorID"].HeaderText = "Sponsor ID";
                        dataGridView1.Columns["AllocateStaff"].HeaderText = "AllocateStaff";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading vendors: {ex.Message}", "Database Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(textBox7.Text))
            {
                MessageBox.Show("Event ID is required.", "Validation Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox7.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Vendor ID is required.", "Validation Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Focus();
                return false;
            }

            if (checkedListBox1.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please select at least one service.", "Validation Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void ClearFields()
        {
            textBox7.Clear();
            textBox2.Clear();
            textBox6.Clear();
            numericUpDown1.Value = 0;

            // Clear all checked items
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, false);
            }

            selectedVendorId = -1;
            dataGridView1.ClearSelection();
        }

        // Add Vendor
        private void button1_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"INSERT INTO Vendors (VendorID, EventID, ServicesOffered, SponsorID, AllocateStaff) 
                                   VALUES (@VendorID, @EventID, @ServicesOffered, @SponsorID, @AllocateStaff)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@VendorID", int.Parse(textBox2.Text));
                        command.Parameters.AddWithValue("@EventID", int.Parse(textBox7.Text));
                        command.Parameters.AddWithValue("@ServicesOffered", GetSelectedServices());
                        command.Parameters.AddWithValue("@SponsorID",
                            string.IsNullOrWhiteSpace(textBox6.Text) ? (object)DBNull.Value : int.Parse(textBox6.Text));
                        command.Parameters.AddWithValue("@AllocateStaff", (int)numericUpDown1.Value);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Vendor added successfully!", "Success",
                                           MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadVendors();
                            ClearFields();
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number == 2627) // Primary key violation
                {
                    MessageBox.Show("A vendor with this ID already exists.", "Duplicate Entry",
                                   MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show($"Database error: {sqlEx.Message}", "Database Error",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding vendor: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Delete Vendor
        private void button2_Click(object sender, EventArgs e)
        {
            if (selectedVendorId == -1)
            {
                MessageBox.Show("Please select a vendor to delete.", "No Selection",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this vendor?",
                                                 "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "DELETE FROM Vendors WHERE VendorID = @VendorID";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@VendorID", selectedVendorId);

                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Vendor deleted successfully!", "Success",
                                               MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadVendors();
                                ClearFields();
                            }
                            else
                            {
                                MessageBox.Show("Vendor not found or could not be deleted.", "Delete Failed",
                                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting vendor: {ex.Message}", "Error",
                                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Update Vendor
        private void button3_Click(object sender, EventArgs e)
        {
            if (selectedVendorId == -1)
            {
                MessageBox.Show("Please select a vendor to update.", "No Selection",
                               MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput()) return;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"UPDATE Vendors SET 
                                   EventID = @EventID, 
                                   ServicesOffered = @ServicesOffered, 
                                   SponsorID = @SponsorID, 
                                  AllocateStaff = @AllocateStaff
                                   WHERE VendorID = @VendorID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@VendorID", selectedVendorId);
                        command.Parameters.AddWithValue("@EventID", int.Parse(textBox7.Text));
                        command.Parameters.AddWithValue("@ServicesOffered", GetSelectedServices());
                        command.Parameters.AddWithValue("@SponsorID",
                            string.IsNullOrWhiteSpace(textBox6.Text) ? (object)DBNull.Value : int.Parse(textBox6.Text));
                        command.Parameters.AddWithValue("@AllocateStaff", (int)numericUpDown1.Value);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Vendor updated successfully!", "Success",
                                           MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadVendors();
                        }
                        else
                        {
                            MessageBox.Show("Vendor not found or could not be updated.", "Update Failed",
                                           MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating vendor: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Reset Fields
        private void button4_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        // Search functionality
        private void SearchVendors()
        {
            string searchTerm = textBox1.Text.Trim();
            string searchField = comboBox1.Text;

            if (string.IsNullOrWhiteSpace(searchTerm) || searchField == "Choose field")
            {
                LoadVendors();
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "";

                    switch (searchField.ToLower())
                    {
                        case "id":
                            query = "SELECT * FROM Vendors WHERE VendorID LIKE @SearchTerm";
                            break;
                        case "name":
                            // Assuming you have a Name column, otherwise modify as needed
                            query = "SELECT * FROM Vendors WHERE ServicesOffered LIKE @SearchTerm";
                            break;
                        case "email":
                            // If you have an email column, otherwise modify as needed
                            query = "SELECT * FROM Vendors WHERE ServicesOffered LIKE @SearchTerm";
                            break;
                        default:
                            LoadVendors();
                            return;
                    }

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        dataGridView1.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching vendors: {ex.Message}", "Search Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Search button click (assuming pictureBox1 is used as search button)
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            SearchVendors();
        }

        // Back button
        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
            Organizer_Dashboard dashboard = new Organizer_Dashboard();
            dashboard.Show();
        }

        // Exit button
        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Optional: Handle Enter key in search textbox
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SearchVendors();
                e.Handled = true;
            }
        }

        // Optional: Handle placeholder text
        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.Text == "Input...")
            {
                textBox1.Text = "";
                textBox1.ForeColor = SystemColors.WindowText;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = "Input...";
                textBox1.ForeColor = SystemColors.GrayText;
            }
        }
    }
}