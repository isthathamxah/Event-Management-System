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
    public partial class SignUp_Attendee : Form
    {
        string connectionString = "Data Source=DESKTOP-Q3RNHLM;Initial Catalog=Project;Integrated Security=True;TrustServerCertificate=True";

        public SignUp_Attendee()
        {
            InitializeComponent();
            this.Load += new EventHandler(Form_Load);
        }

        private void Form_Load(object sender, EventArgs e)
        {
            // Add event preferences to the comboBox1
            comboBox1.Items.Add("Conference");
            comboBox1.Items.Add("Workshop");
            comboBox1.Items.Add("Seminar");
            comboBox1.Items.Add("Networking Event");
            comboBox1.Items.Add("Webinar");

            // Optionally, set a default selection if needed
            comboBox1.SelectedIndex = 0; // This will set the first item as the selected one (optional)
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Register the attendee into the database
            string name = textBox1.Text.Trim();
            string email = textBox2.Text.Trim();
            string contact = textBox3.Text.Trim();
            string eventPreference = comboBox1.SelectedItem.ToString();
            string password = textBox4.Text.Trim();
            string confirmPassword = textBox5.Text.Trim();
            string username = textBox5.Text.Trim();
            string Role = "Attendee";

            // Validate input fields
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(contact) ||
                string.IsNullOrEmpty(eventPreference) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Insert the new attendee into the database
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO AttendeeSign (UserName, FullName, Email, ContactNumber, EventPreference, Password, Role) VALUES (@username, @Name, @Email, @Contact, @EventPreference, @Password, @Role)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {

                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Contact", contact);
                        cmd.Parameters.AddWithValue("@EventPreference", eventPreference);
                        cmd.Parameters.AddWithValue("@Password", password);
                        cmd.Parameters.AddWithValue("@Role", Role);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Registration successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // After successful registration, go to Attendee Dashboard
                            this.Close();
                            Attendee_Dashboard attendeeDashboard = new Attendee_Dashboard();
                            attendeeDashboard.Show();
                        }
                        else
                        {
                            MessageBox.Show("Error registering attendee.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while registering: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Go to the login page
            this.Close();
            Login login = new Login();
            login.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Go to the login page
            this.Close();
            Login login = new Login();
            login.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Go to the landing page
            this.Close();
            LandingPage landing = new LandingPage();
            landing.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Name textbox
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            // Email textbox
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // Contact textbox
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            // Event preference combobox
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            // Password textbox
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            // Confirm password textbox
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // Show password checkbox
            textBox4.PasswordChar = checkBox1.Checked ? '\0' : '*';
            textBox5.PasswordChar = checkBox1.Checked ? '\0' : '*';
        }
    }
}


