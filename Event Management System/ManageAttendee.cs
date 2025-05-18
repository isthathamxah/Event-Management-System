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
    public partial class ManageAttendee : Form
    {
        // Connection string (update with your actual database connection string)
        private string connectionString = "Data Source=DESKTOP-Q3RNHLM;Initial Catalog=Project;Integrated Security=True;TrustServerCertificate=True";


        public ManageAttendee()
        {
            InitializeComponent();
            LoadAttendees(); // Load data on form load
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Go to admin dashboard
            this.Close();
            Admin_Dashboard admin = new Admin_Dashboard();
            admin.Show();
        }

        private void LoadAttendees()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM Attendees";
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading data: {ex.Message}");
                }
            }
        }

        private void ResetFields()
        {
            textBox7.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox6.Clear();
            textBox4.Clear();
        }

        private void button2_Click(object sender, EventArgs e) // Delete button
        {
            if (!string.IsNullOrWhiteSpace(textBox2.Text))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        string query = "DELETE FROM Attendees WHERE ID = @ID";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@ID", textBox2.Text.Trim());
                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Attendee deleted successfully!");
                                ResetFields();
                                LoadAttendees(); // Reload attendees after deletion
                            }
                            else
                            {
                                MessageBox.Show("No attendee found with the given ID.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting attendee: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please provide an ID to delete.");
            }
        }

        private void button1_Click(object sender, EventArgs e) // Add button
        {
            if (!string.IsNullOrWhiteSpace(textBox7.Text) &&
                !string.IsNullOrWhiteSpace(textBox2.Text) &&
                !string.IsNullOrWhiteSpace(textBox3.Text) &&
                !string.IsNullOrWhiteSpace(textBox6.Text))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        string query = "INSERT INTO Attendees (AttendeeName, ContactEmail, Address, AddMessage) " +
                                       "VALUES (@AttendeeName, @ContactEmail, @Address, @AddMessage)";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@AttendeeName", textBox7.Text);
                            cmd.Parameters.AddWithValue("@ContactEmail", textBox2.Text);
                            cmd.Parameters.AddWithValue("@Address", textBox6.Text);
                            cmd.Parameters.AddWithValue("@AddMessage", textBox4.Text);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Attendee added successfully!");
                                ResetFields();
                                LoadAttendees(); // Reload attendees after adding
                            }
                            else
                            {
                                MessageBox.Show("Error adding attendee.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error adding attendee: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please fill in all the fields.");
            }
        }

        private void button3_Click(object sender, EventArgs e) // Update button
        {
            if (!string.IsNullOrWhiteSpace(textBox7.Text) &&
                !string.IsNullOrWhiteSpace(textBox2.Text) &&
                !string.IsNullOrWhiteSpace(textBox3.Text) &&
                !string.IsNullOrWhiteSpace(textBox6.Text))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        string query = "UPDATE Attendees SET AttendeeName = @AttendeeName, ContactEmail = @ContactEmail, " +
                                       "Address = @Address, AddMessage = @AddMessage WHERE ID = @ID";
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@ID", textBox2.Text.Trim());
                            cmd.Parameters.AddWithValue("@AttendeeName", textBox7.Text);
                            cmd.Parameters.AddWithValue("@ContactEmail", textBox3.Text);
                            cmd.Parameters.AddWithValue("@Address", textBox6.Text);
                            cmd.Parameters.AddWithValue("@AddMessage", textBox4.Text);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Attendee updated successfully!");
                                ResetFields();
                                LoadAttendees(); // Reload attendees after updating
                            }
                            else
                            {
                                MessageBox.Show("No attendee found with the given ID.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating attendee: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please fill in all the fields.");
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            // Add functionality here, for example:
            MessageBox.Show("Picture box clicked!");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void button4_Click(object sender, EventArgs e) // Reset Fields button
        {
            ResetFields();
        }
    }
}