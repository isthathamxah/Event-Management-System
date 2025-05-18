/*using System;
using System.Windows.Forms;



namespace EMS
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        // Method to validate user input and direct to the dashboard
        private void button1_Click(object sender, EventArgs e)
        {
            // Check if all fields are filled
            if (string.IsNullOrWhiteSpace(textBox1.Text) || // Username
                string.IsNullOrWhiteSpace(textBox3.Text) || // Password
                comboBox2.SelectedItem == null) // Role
            {
                MessageBox.Show("Please fill all the fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Fetch the entered username, password, and role
            string username = textBox1.Text.Trim();
            string password = textBox3.Text.Trim();
            string role = comboBox2.SelectedItem.ToString();

            //based on role re-direct
            if (role == "Admin")
            {
                // Check if the entered credentials are valid
                if (username == "admin" && password == "admin")
                {
                    this.Close();
                    Admin_Dashboard adminDashboard = new Admin_Dashboard();
                    adminDashboard.Show();
                }
                else
                {
                    MessageBox.Show("Invalid credentials. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (role == "Attendee")
            {
                // Check if the entered credentials are valid
                if (username == "attendee" && password == "attendee")
                {
                    this.Close();
                    Attendee_Dashboard attendeeDashboard = new Attendee_Dashboard();
                    attendeeDashboard.Show();
                }
                else
                {
                    MessageBox.Show("Invalid credentials. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (role == "Vendor")
            {
                // Check if the entered credentials are valid
                if (username == "vendor" && password == "vendor")
                {
                    this.Close();
                    Vendor_Dashboard vendorDashboard = new Vendor_Dashboard();
                    vendorDashboard.Show();
                }
                else
                {
                    MessageBox.Show("Invalid credentials. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            //organizer
            else if (role == "Organizer")
            {
                // Check if the entered credentials are valid
                if (username == "organizer" && password == "organizer")
                {
                    this.Close();
                    Organizer_Dashboard organizerDashboard = new Organizer_Dashboard();
                    organizerDashboard.Show();
                }
                else
                {
                    MessageBox.Show("Invalid credentials. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

        }

        // Go to the Landing Page
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

        // Redirect to the Register page based on role
        private void button2_Click(object sender, EventArgs e)
        {
            // Check if a role is selected
            if (comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Please select a role to register.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string role = comboBox2.SelectedItem.ToString();
            this.Close();

            if (role == "Admin")
            {
                SignUp_Admin adminRegister = new SignUp_Admin();
                adminRegister.Show();
            }
            else if (role == "Attendee")
            {
                SignUp_Attendee attendeeRegister = new SignUp_Attendee();
                attendeeRegister.Show();
            }
            else if (role == "Vendor")
            {
                SignUp_Vendor vendorRegister = new SignUp_Vendor();
                vendorRegister.Show();
            }
            //organizer
            else if (role == "Organizer")
            {
                SignUp_Organizer organizerRegister = new SignUp_Organizer();
                organizerRegister.Show();
            }
        }

        // Show/Hide password logic
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox3.UseSystemPasswordChar = false; // Show password
            }
            else
            {
                textBox3.UseSystemPasswordChar = true; // Hide password
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Username field logic can be implemented here (if needed)
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // Password field logic can be implemented here (if needed)
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Role combo box selection changed logic (if any specific action needed)
        }

        private void label4_Click(object sender, EventArgs e)
        {
            // Label click event (if needed)
        }
    }
}*/
















using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using Event_Management_System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Event
{
    public partial class Login : Form
    {
        // Define the connection string here
        string connectionString = "Data Source=DESKTOP-Q3RNHLM;Initial Catalog=Project;Integrated Security=True;TrustServerCertificate=True";


        public Login()
        {
            InitializeComponent();
            ValidateDatabaseConnection();
        }

        private void ValidateDatabaseConnection()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show("Database connection successful!", "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database connection failed: " + ex.Message, "Connection Test", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Method to validate user input and direct to the dashboard
        private void button1_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Please fill all the fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string username = textBox1.Text.Trim();
            string password = textBox3.Text.Trim();
            string role = comboBox2.SelectedItem.ToString();


            if (role == "Admin")
            {

                string query = "SELECT COUNT(*) FROM Users1 WHERE Username = @Username AND Password COLLATE SQL_Latin1_General_CP1_CI_AS = @Password AND Role = @Role";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Username", username);
                            command.Parameters.AddWithValue("@Password", password);
                            command.Parameters.AddWithValue("@Role", role);

                            int userCount = (int)command.ExecuteScalar();

                            if (userCount > 0)
                            {
                                this.Close();

                                if (role == "Admin")
                                {
                                    Admin_Dashboard adminDashboard = new Admin_Dashboard();
                                    adminDashboard.Show();
                                }
                                else if (role == "Attendee")
                                {
                                    Attendee_Dashboard attendeeDashboard = new Attendee_Dashboard();
                                    attendeeDashboard.Show();
                                }
                                else if (role == "Vendor")
                                {
                                    Vendor_Dashboard vendorDashboard = new Vendor_Dashboard();
                                    vendorDashboard.Show();
                                }
                                else if (role == "Organizer")
                                {
                                    Organizer_Dashboard organizerDashboard = new Organizer_Dashboard();
                                    organizerDashboard.Show();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid credentials. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error connecting to the database: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                // Check if the entered credentials are valid
                /*if (username == "admin" && password == "admin")
                {
                    this.Close();
                    Admin_Dashboard adminDashboard = new Admin_Dashboard();
                    adminDashboard.Show();
                }
                else
                {
                    MessageBox.Show("Invalid credentials. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }*/
            }
            else if (role == "Attendee")
            {
                string query = "SELECT COUNT(*) FROM AttendeeSign WHERE UserName = @Username AND Password COLLATE SQL_Latin1_General_CP1_CI_AS = @Password AND Role = @Role";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Username", username);
                            command.Parameters.AddWithValue("@Password", password);
                            command.Parameters.AddWithValue("@Role", role);

                            int userCount = (int)command.ExecuteScalar();

                            if (userCount > 0)
                            {
                                this.Close();

                                if (role == "Admin")
                                {
                                    Admin_Dashboard adminDashboard = new Admin_Dashboard();
                                    adminDashboard.Show();
                                }
                                else if (role == "Attendee")
                                {
                                    Attendee_Dashboard attendeeDashboard = new Attendee_Dashboard();
                                    attendeeDashboard.Show();
                                }
                                else if (role == "Vendor")
                                {
                                    Vendor_Dashboard vendorDashboard = new Vendor_Dashboard();
                                    vendorDashboard.Show();
                                }
                                else if (role == "Organizer")
                                {
                                    Organizer_Dashboard organizerDashboard = new Organizer_Dashboard();
                                    organizerDashboard.Show();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid credentials. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error connecting to the database: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }


                // Check if the entered credentials are valid
                /*if (username == "attendee" && password == "attendee")
                {
                    this.Close();
                    Attendee_Dashboard attendeeDashboard = new Attendee_Dashboard();
                    attendeeDashboard.Show();
                }
                else
                {
                    MessageBox.Show("Invalid credentials. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }*/
            }
            else if (role == "Vendor")
            {

                string query = "SELECT COUNT(*) FROM VendorSign WHERE UserName = @Username AND Password COLLATE SQL_Latin1_General_CP1_CI_AS = @Password AND Role = @Role";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Username", username);
                            command.Parameters.AddWithValue("@Password", password);
                            command.Parameters.AddWithValue("@Role", role);

                            int userCount = (int)command.ExecuteScalar();

                            if (userCount > 0)
                            {
                                this.Close();

                                if (role == "Admin")
                                {
                                    Admin_Dashboard adminDashboard = new Admin_Dashboard();
                                    adminDashboard.Show();
                                }
                                else if (role == "Attendee")
                                {
                                    Attendee_Dashboard attendeeDashboard = new Attendee_Dashboard();
                                    attendeeDashboard.Show();
                                }
                                else if (role == "Vendor")
                                {
                                    Vendor_Dashboard vendorDashboard = new Vendor_Dashboard();
                                    vendorDashboard.Show();
                                }
                                else if (role == "Organizer")
                                {
                                    Organizer_Dashboard organizerDashboard = new Organizer_Dashboard();
                                    organizerDashboard.Show();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid credentials. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error connecting to the database: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }




                // Check if the entered credentials are valid
                /*if (username == "vendor" && password == "vendor")
                {
                    this.Close();
                    Vendor_Dashboard vendorDashboard = new Vendor_Dashboard();
                    vendorDashboard.Show();
                }
                else
                {
                    MessageBox.Show("Invalid credentials. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }*/
            }
            //organizer
            else if (role == "Organizer")
            {




                string query = "SELECT COUNT(*) FROM OrganizerSign WHERE Username = @Username AND PasswordHash = @Password AND Role = @Role";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Username", username);
                            command.Parameters.AddWithValue("@Password", password);
                            command.Parameters.AddWithValue("@Role", role);

                            int userCount = (int)command.ExecuteScalar();

                            if (userCount > 0)
                            {
                                MessageBox.Show("Login successful for Organizer!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Close();

                                Organizer_Dashboard organizerDashboard = new Organizer_Dashboard();
                                organizerDashboard.Show();
                            }
                            else
                            {
                                MessageBox.Show("Invalid Organizer credentials. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                        MessageBox.Show($"SQL Error: {sqlEx.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }



                // Check if the entered credentials are valid
                /*if (username == "organizer" && password == "organizer")
                {
                    this.Close();
                    Organizer_Dashboard organizerDashboard = new Organizer_Dashboard();
                    organizerDashboard.Show();
                }
                else
                {
                    MessageBox.Show("Invalid credentials. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }*/
            }


            /*string query = "SELECT COUNT(*) FROM Users1 WHERE Username = @Username AND Password COLLATE SQL_Latin1_General_CP1_CI_AS = @Password AND Role = @Role";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@Role", role);

                    int userCount = (int)command.ExecuteScalar();

                    if (userCount > 0)
                    {
                        this.Close();

                        if (role == "Admin")
                        {
                            Admin_Dashboard adminDashboard = new Admin_Dashboard();
                            adminDashboard.Show();
                        }
                        else if (role == "Attendee")
                        {
                            Attendee_Dashboard attendeeDashboard = new Attendee_Dashboard();
                            attendeeDashboard.Show();
                        }
                        else if (role == "Vendor")
                        {
                            Vendor_Dashboard vendorDashboard = new Vendor_Dashboard();
                            vendorDashboard.Show();
                        }
                        else if (role == "Organizer")
                        {
                            Organizer_Dashboard organizerDashboard = new Organizer_Dashboard();
                            organizerDashboard.Show();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid credentials. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to the database: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }*/
        }


        // Go to the Landing Page
        private void button4_Click(object sender, EventArgs e)
        {
            /*MessageBox.Show("Event added successfully!");*/
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

        // Redirect to the Register page based on role
        private void button2_Click(object sender, EventArgs e)
        {
            // Check if a role is selected
            if (comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Please select a role to register.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string role = comboBox2.SelectedItem.ToString();
            this.Close();

            if (role == "Admin")
            {
                SignUp_Admin adminRegister = new SignUp_Admin();
                adminRegister.Show();
            }
            else if (role == "Attendee")
            {
                SignUp_Attendee attendeeRegister = new SignUp_Attendee();
                attendeeRegister.Show();
            }
            else if (role == "Vendor")
            {
                SignUp_Vendor vendorRegister = new SignUp_Vendor();
                vendorRegister.Show();
            }
            else if (role == "Organizer")
            {
                SignUp_Organizer organizerRegister = new SignUp_Organizer();
                organizerRegister.Show();
            }
        }

        // Show/Hide password logic
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox3.UseSystemPasswordChar = false; // Show password
            }
            else
            {
                textBox3.UseSystemPasswordChar = true; // Hide password
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Username field logic can be implemented here (if needed)
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // Password field logic can be implemented here (if needed)
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Role combo box selection changed logic (if any specific action needed)
        }

        private void label4_Click(object sender, EventArgs e)
        {
            // Label click event (if needed)
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
    }
}
