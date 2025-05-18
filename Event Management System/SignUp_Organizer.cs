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
    public partial class SignUp_Organizer : Form
    {
        private string connectionString = "Data Source=DESKTOP-Q3RNHLM;Initial Catalog=Project;Integrated Security=True;TrustServerCertificate=True";

        public SignUp_Organizer()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string role = "Organizer";
            // Register Organizer
            try
            {
                // Validate Input
                if (string.IsNullOrWhiteSpace(textBox1.Text) || // Username
                    string.IsNullOrWhiteSpace(textBox3.Text) || // Full Name
                    string.IsNullOrWhiteSpace(textBox4.Text) || // Email
                    string.IsNullOrWhiteSpace(textBox6.Text) || // Password
                    string.IsNullOrWhiteSpace(textBox7.Text))   // Confirm Password

                {
                    MessageBox.Show("All fields are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Ensure passwords match
                /* if (textBox6.Text != textBox7.Text)
                 {
                     MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                     return;
                 }*/

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"INSERT INTO OrganizerSign (UserName, FullName, ContactEmail, Address, PasswordHash, Role) 
                                     VALUES (@UserName, @FullName, @ContactEmail, @Address, @Password, @Role)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserName", textBox1.Text);
                        command.Parameters.AddWithValue("@FullName", textBox3.Text);
                        command.Parameters.AddWithValue("@ContactEmail", textBox4.Text);
                        command.Parameters.AddWithValue("@Address", textBox5.Text);
                        command.Parameters.AddWithValue("@Password", textBox6.Text); // Store plain text password
                        command.Parameters.AddWithValue("@Role", role);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Navigate to Organizer Dashboard
                            this.Close();
                            Organizer_Dashboard organizer = new Organizer_Dashboard();
                            organizer.Show();
                        }
                        else
                        {
                            MessageBox.Show("Registration failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Navigate to Login Form
            this.Close();
            Login login = new Login();
            login.Show();
        }
    }
}