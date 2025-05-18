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
    public partial class ManageOrganizer : Form
    {
        // Connection string (Replace with your database connection string)
        private string connectionString = "Data Source=DESKTOP-Q3RNHLM;Initial Catalog=Project;Integrated Security=True;TrustServerCertificate=True";


        public ManageOrganizer()
        {
            this.Load += new System.EventHandler(this.ManageOrganizer_Load);

            InitializeComponent();
        }

        private void ManageOrganizer_Load(object sender, EventArgs e)
        {
            LoadOrganizers();
        }

        // Load organizers into DataGridView
        private void LoadOrganizers()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT OrganizerID, OrganizationName, ContactEmail, Address FROM Organizers";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt; // Fill the DataGridView with data
            }
        }

        // Back button - Navigate back to the Admin Dashboard
        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
            Admin_Dashboard admin = new Admin_Dashboard();
            admin.Show();
        }

        // Exit button - Closes the form
        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Add button - Adds a new organizer to the database
        private void button1_Click(object sender, EventArgs e)
        {
            string organizationName = textBox7.Text;
            string contactEmail = textBox2.Text;
            string address = textBox6.Text;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Organizers (OrganizationName, ContactEmail, Address, CreatedAt, UpdatedAt) VALUES (@OrganizationName, @ContactEmail, @Address, GETDATE(), GETDATE())";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@OrganizationName", organizationName);
                cmd.Parameters.AddWithValue("@ContactEmail", contactEmail);
                cmd.Parameters.AddWithValue("@Address", address);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            // Reload the DataGridView after adding
            LoadOrganizers();
            MessageBox.Show("Organizer added successfully!");
        }

        // Update button - Updates an existing organizer
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                // Get the selected row's OrganizerID
                int organizerID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["OrganizerID"].Value);
                string organizationName = textBox7.Text;
                string contactEmail = textBox2.Text;
                string address = textBox6.Text;

                // Update the organizer in the database
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Organizers SET OrganizationName = @OrganizationName, ContactEmail = @ContactEmail, Address = @Address, UpdatedAt = GETDATE() WHERE OrganizerID = @OrganizerID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@OrganizerID", organizerID);
                    cmd.Parameters.AddWithValue("@OrganizationName", organizationName);
                    cmd.Parameters.AddWithValue("@ContactEmail", contactEmail);
                    cmd.Parameters.AddWithValue("@Address", address);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                // Reload the DataGridView after updating
                LoadOrganizers();
                MessageBox.Show("Organizer updated successfully!");
            }
            else
            {
                MessageBox.Show("Please select an organizer to update.");
            }
        }

        // Delete button - Deletes an organizer from the database
        private void button2_Click(object sender, EventArgs e) // Delete button
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    int organizerID = Convert.ToInt32(row.Cells["OrganizerID"].Value);  // Assuming OrganizerID column exists in DataGridView

                    // Delete the organizer from the database
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = "DELETE FROM Organizers WHERE OrganizerID = @OrganizerID";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@OrganizerID", organizerID);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }

                // Reload the DataGridView after deletion
                LoadOrganizers();
                MessageBox.Show("Organizer(s) deleted successfully!");
            }
            else
            {
                MessageBox.Show("Please select an organizer to delete.");
            }
        }





        // Reset button - Clears all input fields
        private void button4_Click(object sender, EventArgs e)
        {
            textBox7.Clear();
            textBox2.Clear();
            textBox6.Clear();
            textBox3.Clear();
        }

        // Filter organizer list based on search input
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string searchField = comboBox1.SelectedItem?.ToString();
            string searchValue = textBox1.Text;

            if (string.IsNullOrEmpty(searchField) || string.IsNullOrEmpty(searchValue))
            {
                LoadOrganizers();
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = $"SELECT OrganizerID, OrganizationName, ContactEmail, Address FROM Organizers WHERE {searchField} LIKE @SearchValue";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@SearchValue", "%" + searchValue + "%");
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        // Dropdown change event to handle search field selection
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Clear();
        }
    }
}
