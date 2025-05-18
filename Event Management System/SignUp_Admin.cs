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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Event_Management_System
{
    public partial class SignUp_Admin : Form
    {
        public SignUp_Admin()
        {
            InitializeComponent();
        }

        // Navigate to Landing Page
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
            LandingPage landing = new LandingPage();
            landing.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            LandingPage landing = new LandingPage();
            landing.Show();
        }

        // Validate input and redirect on successful signup
        private void button1_Click(object sender, EventArgs e)
        {
            // Retrieve field values
            string username = textBox7.Text;
            string firstName = textBox1.Text.Trim();
            string lastName = textBox2.Text.Trim();
            string email = textBox3.Text.Trim();
            string phoneNum = textBox6.Text.Trim();
            string password = textBox4.Text.Trim();
            string confirmPassword = textBox5.Text.Trim();
            string role = "Admin"; // Set role as Admin for this form

            // Validate input fields
            if (string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(phoneNum) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("All fields must be filled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check email format (basic validation)
            if (!email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("Please enter a valid email address.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate phone number length (basic validation for 10 digits)
            if (phoneNum.Length != 10 || !phoneNum.All(char.IsDigit))
            {
                MessageBox.Show("Please enter a valid 10-digit phone number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate password match
            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Insert into database
            string connectionString = "Data Source=DESKTOP-Q3RNHLM;Initial Catalog=Project;Integrated Security=True;TrustServerCertificate=True";
            string query = "INSERT INTO Users1 (Username, FirstName, LastName, Email, PhoneNumber, Password, Role) VALUES (@Username, @FirstName, @LastName, @Email, @PhoneNumber, @Password, @Role)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@PhoneNumber", phoneNum);
                        command.Parameters.AddWithValue("@Password", password); // Use hashing for password in production
                        command.Parameters.AddWithValue("@Role", role);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Admin account created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Redirect to Login page
                            this.Close();
                            Login login = new Login();
                            login.Show();
                        }
                        else
                        {
                            MessageBox.Show("Registration failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving user data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }






        // Navigate to Login Page
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Login login = new Login();
            login.Show();
        }

        // Show/Hide password
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox4.UseSystemPasswordChar = false; // Show password
                textBox5.UseSystemPasswordChar = false; // Show confirm password
            }
            else
            {
                textBox4.UseSystemPasswordChar = true; // Hide password
                textBox5.UseSystemPasswordChar = true; // Hide confirm password
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Logic for first-name input field (if needed)
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // Logic for last-name input field (if needed)
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // Logic for email input field (if needed)
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            // Logic for phone number input field (if needed)
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            // Logic for password input field (if needed)
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            // Logic for confirm-password input field (if needed)
        }
    }
}
