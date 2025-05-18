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
using Event;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;



namespace Event_Management_System
{
    public partial class SignUp_Vendor : Form
    {
        // Connection string to your SQL Server database
        string connectionString = "Data Source=DESKTOP-Q3RNHLM;Initial Catalog=Project;Integrated Security=True;TrustServerCertificate=True";

        public SignUp_Vendor()
        {
            InitializeComponent();
            this.Load += new EventHandler(Form_Load);
        }


        private void Form_Load(object sender, EventArgs e)
        {
            // Add event preferences to the comboBox1
            comboBox1.Items.Add("Marketing");
            comboBox1.Items.Add("IT");
            comboBox1.Items.Add("Retailer");
            comboBox1.Items.Add("Grocery");
            comboBox1.Items.Add("Cosmetics");

            // Optionally, set a default selection if needed
            comboBox1.SelectedIndex = 0; // This will set the first item as the selected one (optional)
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Navigate to the previous form (Login or other)
            this.Close(); // Close current form and go back to previous one.
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Exit the application
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Register button click event: Validate and process the vendor signup
            string username = textBox7.Text;
            string password = textBox4.Text;
            string confirmPassword = textBox5.Text;
            string companyName = textBox1.Text;
            string contactNumber = textBox2.Text;
            string email = textBox3.Text;
            string Role = "Vendor";
            string BusinessType = comboBox1.SelectedItem.ToString();
            string servicesOffered = string.Join(", ", checkedListBox1.CheckedItems.OfType<string>().ToArray());

            // Basic validation: Check if any required field is empty
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(companyName) || string.IsNullOrWhiteSpace(contactNumber) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(servicesOffered))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // Confirm that the passwords match
            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            // Insert vendor details into the database
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO VendorSign (UserName, Password, BusinessName, PersonName, ContactEmail, BusinessType, ServicesOffered, Role) " +
                                   "VALUES (@UserName, @Password, @BusinessName, @PersonName, @ContactEmail, @BusinessType, @ServicesOffered, @Role)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to the command to prevent SQL injection
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@BusinessName", companyName);
                        command.Parameters.AddWithValue("@PersonName", contactNumber);
                        command.Parameters.AddWithValue("@ContactEmail", email);
                        command.Parameters.AddWithValue("@ServicesOffered", servicesOffered);
                        command.Parameters.AddWithValue("@BusinessType", BusinessType);
                        command.Parameters.AddWithValue("@Role", Role);


                        // Execute the query
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Vendor registered successfully!");
                        }
                        else
                        {
                            MessageBox.Show("Error occurred while registering vendor.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any database connection or query errors
                MessageBox.Show("An error occurred: " + ex.Message);
            }

            // Redirect to login or other appropriate page
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Navigate to login form
            this.Hide(); // Hide the current form
            Login loginForm = new Login(); // Assuming Login is the name of the login form
            loginForm.Show(); // Show the login form
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Toggle password visibility
            if (checkBox1.Checked)
            {
                textBox4.UseSystemPasswordChar = false;
                textBox5.UseSystemPasswordChar = false;
            }
            else
            {
                textBox4.UseSystemPasswordChar = true;
                textBox5.UseSystemPasswordChar = true;
            }
        }
    }
}
