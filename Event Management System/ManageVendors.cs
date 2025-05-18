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
    public partial class ManageVendors : Form
    {
        string connectionString = "Data Source=DESKTOP-Q3RNHLM;Initial Catalog=Project;Integrated Security=True;TrustServerCertificate=True";

        public ManageVendors()
        {
            this.Load += new System.EventHandler(this.ManageVendors_Load);
            InitializeComponent();
        }

        // Method to load vendors from the database into the DataGridView
        private void LoadVendors()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT VendorID, EventID, ServicesOffered, SponsorID, AllocateStaff FROM Vendors";
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Button click to go back to the main menu
        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
            Organizer_Dashboard od = new Organizer_Dashboard();
            od.Show();
        }

        // Button click to add a new vendor
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateFields())
                {
                    int eventID = Convert.ToInt32(textBox7.Text);
                    string servicesOffered = textBox2.Text;
                    int sponsorID = Convert.ToInt32(textBox6.Text);
                    int allocateStaff = (int)numericUpDown1.Value;

                    // Connect to database and insert the new vendor
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string query = "INSERT INTO Vendors (EventID, ServicesOffered, SponsorID, AllocateStaff) " +
                                       "VALUES (@EventID, @ServicesOffered, @SponsorID, @AllocateStaff)";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@EventID", eventID);
                        command.Parameters.AddWithValue("@ServicesOffered", servicesOffered);
                        command.Parameters.AddWithValue("@SponsorID", sponsorID);
                        command.Parameters.AddWithValue("@AllocateStaff", allocateStaff);

                        connection.Open();
                        command.ExecuteNonQuery();  // Executes the insert command
                    }

                    MessageBox.Show("Vendor added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadVendors();  // Refresh the DataGridView after adding the vendor
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Button click to delete a selected vendor
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    int vendorID = Convert.ToInt32(row.Cells["VendorID"].Value);  // Assuming VendorID column exists in DataGridView

                    // Connect to database and delete the vendor
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string query = "DELETE FROM Vendors WHERE VendorID = @VendorID";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@VendorID", vendorID);

                        connection.Open();
                        command.ExecuteNonQuery();  // Executes the delete command
                    }
                }

                MessageBox.Show("Selected vendor(s) deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadVendors();  // Refresh the DataGridView after deletion
            }
            else
            {
                MessageBox.Show("Please select a vendor to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Button click to update a selected vendor
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                int vendorID = Convert.ToInt32(selectedRow.Cells["VendorID"].Value);  // Assuming VendorID column exists in DataGridView
                int eventID = Convert.ToInt32(textBox7.Text);
                string servicesOffered = textBox2.Text;
                int sponsorID = Convert.ToInt32(textBox6.Text);
                int allocateStaff = (int)numericUpDown1.Value;

                // Connect to database and update the vendor
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Vendors SET EventID = @EventID, ServicesOffered = @ServicesOffered, SponsorID = @SponsorID, AllocateStaff = @AllocateStaff WHERE VendorID = @VendorID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@VendorID", vendorID);
                    command.Parameters.AddWithValue("@EventID", eventID);
                    command.Parameters.AddWithValue("@ServicesOffered", servicesOffered);
                    command.Parameters.AddWithValue("@SponsorID", sponsorID);
                    command.Parameters.AddWithValue("@AllocateStaff", allocateStaff);

                    connection.Open();
                    command.ExecuteNonQuery();  // Executes the update command
                }

                MessageBox.Show("Vendor updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadVendors();  // Refresh the DataGridView after updating
            }
            else
            {
                MessageBox.Show("Please select a single vendor to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Button click to reset all fields
        private void button4_Click(object sender, EventArgs e)
        {
            textBox7.Clear();
            textBox2.Clear();
            textBox6.Clear();
            numericUpDown1.Value = 0;
        }

        // Button click to exit the application
        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Load vendors when the form is loaded
        private void ManageVendors_Load(object sender, EventArgs e)
        {
            LoadVendors();  // Load vendors when the form is loaded
        }

        // Validate the input fields
        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(textBox7.Text) || string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox6.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Ensure eventID and sponsorID are valid numbers
            if (!int.TryParse(textBox7.Text, out _) || !int.TryParse(textBox6.Text, out _))
            {
                MessageBox.Show("Please enter valid numeric values for EventID and SponsorID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
    }
}