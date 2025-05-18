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
    public partial class ManageProfile : Form
    {
        string connectionString = "Data Source=DESKTOP-Q3RNHLM;Initial Catalog=Project;Integrated Security=True;TrustServerCertificate=True";

        public ManageProfile()
        {
            InitializeComponent();
            this.Load += new EventHandler(Form_Load);
        }

        private void ManageProfile_Load(object sender, EventArgs e)
        {
            // You can add code here to load data or initialize components when the form is loaded
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Handle add or save profile action
            SaveProfile();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            // Add event preferences to the comboBox1
            comboBox1.Items.Add("Seminars");
            comboBox1.Items.Add("Intelluctual");
            comboBox1.Items.Add("Convocation");
            comboBox1.Items.Add("Sessions");
            comboBox1.Items.Add("Competitions");

            // Optionally, set a default selection if needed
            comboBox1.SelectedIndex = 0; // This will set the first item as the selected one (optional)
        }

        private void SaveProfile()
        {
            string name = textBox1.Text;
            string email = textBox2.Text;
            string phone = textBox3.Text;
            string position = comboBox1.SelectedItem.ToString();
            string department = textBox4.Text;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(department))
            {
                MessageBox.Show("All fields must be filled!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Profile (FullName, Email, ContactNumber, EventPreference, NewPassword) VALUES (@Name, @Email, @Phone, @Position, @Department)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@Position", position);
                cmd.Parameters.AddWithValue("@Department", department);

                conn.Open();
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("Profile saved successfully!");
                    ClearFields();
                    LoadProfiles();
                }
                else
                {
                    MessageBox.Show("Error saving profile.");
                }
            }
        }

        private void LoadProfiles()
        {
            // Load profiles into the data grid view
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Profile";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                System.Data.DataTable dt = new System.Data.DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
            Attendee_Dashboard attendee = new Attendee_Dashboard();
            attendee.Show();

        }

        private void DeleteProfile()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int profileId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ProfileId"].Value);
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Profile WHERE ProfileId = @ProfileId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ProfileId", profileId);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Profile deleted successfully!");
                        LoadProfiles();
                    }
                    else
                    {
                        MessageBox.Show("Error deleting profile.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a profile to delete.");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
            /*Attendee_Dashboard attendee = new Attendee_Dashboard();
            attendee.Show();*/
        }

        private void UpdateProfile()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int profileId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ProfileId"].Value);
                string name = textBox1.Text;
                string email = textBox2.Text;
                string phone = textBox3.Text;
                string position = comboBox1.SelectedItem.ToString();
                string department = textBox4.Text;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Profiles SET Name = @Name, Email = @Email, Phone = @Phone, Position = @Position, Department = @Department WHERE ProfileId = @ProfileId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ProfileId", profileId);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    cmd.Parameters.AddWithValue("@Position", position);
                    cmd.Parameters.AddWithValue("@Department", department);

                    conn.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Profile updated successfully!");
                        LoadProfiles();
                    }
                    else
                    {
                        MessageBox.Show("Error updating profile.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a profile to update.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Handle search functionality
            SearchProfile();
        }

        private void SearchProfile()
        {
            string searchTerm = textBox1.Text;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Profiles WHERE Name LIKE @SearchTerm";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                System.Data.DataTable dt = new System.Data.DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void ClearFields()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            comboBox1.SelectedIndex = -1;
        }
    }
}

