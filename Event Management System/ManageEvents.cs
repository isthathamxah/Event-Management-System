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
    public partial class ManageEvents : Form
    {
        // Connection string
        private string connectionString = "Data Source=DESKTOP-Q3RNHLM;Initial Catalog=Project;Integrated Security=True;TrustServerCertificate=True";

        public ManageEvents()
        {
            this.Load += new System.EventHandler(this.ManageEvents_Load);

            InitializeComponent();
        }

        private void ManageEvents_Load(object sender, EventArgs e)
        {
            // Call the LoadEvents method to load data into DataGridView when the form is loaded
            LoadEvents();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Navigate to admin dashboard
            this.Close();
            Admin_Dashboard admin = new Admin_Dashboard();
            admin.Show();
        }

        // Add Event
        private void button1_Click(object sender, EventArgs e) // Add button
        {
            if (ValidateFields())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        string query = "INSERT INTO Events (EventName, ID, StartDate, Location, Category) VALUES (@EventName, @ID, @StartDate, @Location, @Category)";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@EventName", textBox7.Text.Trim());
                            cmd.Parameters.AddWithValue("@ID", textBox2.Text.Trim());
                            cmd.Parameters.AddWithValue("@StartDate", DateTime.Parse(textBox3.Text.Trim()));
                            cmd.Parameters.AddWithValue("@Location", textBox6.Text.Trim());
                            cmd.Parameters.AddWithValue("@Category", textBox1.Text.Trim());

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Event added successfully!");
                            ResetFields();
                            LoadEvents(); // Reload events after adding
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error adding event: {ex.Message}");
                    }
                }
            }
        }

        // Update Event
        private void button3_Click(object sender, EventArgs e) // Update button
        {
            if (ValidateFields())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        string query = "UPDATE Events SET EventName = @EventName, StartDate = @StartDate, Location = @Location, Category = @Category WHERE ID = @ID";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@EventName", textBox7.Text.Trim());
                            cmd.Parameters.AddWithValue("@StartDate", DateTime.Parse(textBox3.Text.Trim()));
                            cmd.Parameters.AddWithValue("@Location", textBox6.Text.Trim());
                            cmd.Parameters.AddWithValue("@Category", textBox1.Text.Trim());
                            cmd.Parameters.AddWithValue("@ID", textBox2.Text.Trim());

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Event updated successfully!");
                                ResetFields();
                                LoadEvents(); // Reload events after updating
                            }
                            else
                            {
                                MessageBox.Show("No event found with the given ID.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating event: {ex.Message}");
                    }
                }
            }
        }

        // Delete Event
        private void button2_Click(object sender, EventArgs e) // Delete button
        {
            if (!string.IsNullOrWhiteSpace(textBox2.Text))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        string query = "DELETE FROM Events WHERE ID = @ID";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@ID", textBox2.Text.Trim());
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Event deleted successfully!");
                                ResetFields();
                                LoadEvents(); // Reload events after deleting
                            }
                            else
                            {
                                MessageBox.Show("No event found with the given ID.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting event: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please provide an ID to delete.");
            }
        }

        // Reset Fields
        private void button4_Click(object sender, EventArgs e) // Reset Fields button
        {
            ResetFields();
        }

        private void ResetFields()
        {
            textBox7.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox6.Clear();
            textBox1.Clear();
        }

        // Validate Input Fields
        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(textBox7.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox6.Text) ||
                string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return false;
            }

            if (!DateTime.TryParse(textBox3.Text, out _))
            {
                MessageBox.Show("Invalid date format. Please use a valid date.");
                return false;
            }

            return true;
        }

        // Load Events into DataGridView
        private void LoadEvents()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM Events";
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        System.Data.DataTable dataTable = new System.Data.DataTable();
                        adapter.Fill(dataTable);
                        dataGridView1.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading events: {ex.Message}");
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Close the application or form
            this.Close();
        }
    }
}
