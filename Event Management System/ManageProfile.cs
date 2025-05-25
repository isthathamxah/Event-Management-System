using Event;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace Event_Management_System
{
    public partial class ManageProfile : Form
    {
        private readonly string connectionString = "Data Source=DESKTOP-Q3RNHLM;Initial Catalog=Project;Integrated Security=True;TrustServerCertificate=True";
        private int selectedProfileId = -1;
        private bool isEditMode = false;

        public ManageProfile()
        {
            InitializeComponent();
            this.Load += ManageProfile_Load;
        }

        private void ManageProfile_Load(object sender, EventArgs e)
        {
            InitializeComboBox();
            LoadProfiles();
            SetupDataGridView();
        }

        private void InitializeComboBox()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(new string[]
            {
                "Seminars",
                "Intellectual",
                "Convocation",
                "Sessions",
                "Competitions"
            });
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void SetupDataGridView()
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (isEditMode)
            {
                UpdateProfile();
            }
            else
            {
                SaveProfile();
            }
        }

        private void SaveProfile()
        {
            if (!ValidateInput()) return;

            string name = textBox1.Text.Trim();
            string email = textBox2.Text.Trim();
            string phone = textBox3.Text.Trim();
            string eventPreference = comboBox1.SelectedItem.ToString();
            string newPassword = textBox4.Text.Trim();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Check if email already exists
                    string checkQuery = "SELECT COUNT(*) FROM Profile WHERE Email = @Email";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@Email", email);
                        conn.Open();
                        int count = (int)checkCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Email already exists! Please use a different email.", "Duplicate Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Insert new profile
                    string query = "INSERT INTO Profile (FullName, Email, ContactNumber, EventPreference, NewPassword) VALUES (@Name, @Email, @Phone, @EventPreference, @NewPassword)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Phone", phone);
                        cmd.Parameters.AddWithValue("@EventPreference", eventPreference);
                        cmd.Parameters.AddWithValue("@NewPassword", newPassword);

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Profile saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearFields();
                            LoadProfiles();
                        }
                        else
                        {
                            MessageBox.Show("Error saving profile.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateProfile()
        {
            if (!ValidateInput() || selectedProfileId == -1) return;

            string name = textBox1.Text.Trim();
            string email = textBox2.Text.Trim();
            string phone = textBox3.Text.Trim();
            string eventPreference = comboBox1.SelectedItem.ToString();
            string newPassword = textBox4.Text.Trim();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    // Check if email already exists for other profiles
                    string checkQuery = "SELECT COUNT(*) FROM Profile WHERE Email = @Email AND ProfileId != @ProfileId";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@Email", email);
                        checkCmd.Parameters.AddWithValue("@ProfileId", selectedProfileId);
                        conn.Open();
                        int count = (int)checkCmd.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Email already exists! Please use a different email.", "Duplicate Email", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Update profile
                    string query = "UPDATE Profile SET FullName = @Name, Email = @Email, ContactNumber = @Phone, EventPreference = @EventPreference, NewPassword = @NewPassword WHERE ProfileId = @ProfileId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProfileId", selectedProfileId);
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Phone", phone);
                        cmd.Parameters.AddWithValue("@EventPreference", eventPreference);
                        cmd.Parameters.AddWithValue("@NewPassword", newPassword);

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearFields();
                            LoadProfiles();
                            ResetToAddMode();
                        }
                        else
                        {
                            MessageBox.Show("Error updating profile.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProfiles()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT ProfileId, FullName, Email, ContactNumber, EventPreference FROM Profile ORDER BY FullName";
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridView1.DataSource = dt;

                        // Hide ProfileId column but keep it for selection purposes
                        if (dataGridView1.Columns["ProfileId"] != null)
                        {
                            dataGridView1.Columns["ProfileId"].Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading profiles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteProfile()
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a profile to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete this profile?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                int profileId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ProfileId"].Value);

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Profile WHERE ProfileId = @ProfileId";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProfileId", profileId);
                        conn.Open();
                        int deleteResult = cmd.ExecuteNonQuery();

                        if (deleteResult > 0)
                        {
                            MessageBox.Show("Profile deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadProfiles();
                            ClearFields();
                            ResetToAddMode();
                        }
                        else
                        {
                            MessageBox.Show("Error deleting profile.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SearchProfile()
        {
            string searchTerm = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(searchTerm))
            {
                LoadProfiles();
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT ProfileId, FullName, Email, ContactNumber, EventPreference FROM Profile WHERE FullName LIKE @SearchTerm OR Email LIKE @SearchTerm ORDER BY FullName";
                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        dataGridView1.DataSource = dt;

                        if (dataGridView1.Columns["ProfileId"] != null)
                        {
                            dataGridView1.Columns["ProfileId"].Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Search error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Full Name is required!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Email is required!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Focus();
                return false;
            }

            if (!IsValidEmail(textBox2.Text.Trim()))
            {
                MessageBox.Show("Please enter a valid email address!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Contact Number is required!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox3.Focus();
                return false;
            }

            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an Event Preference!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBox1.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("New Password is required!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox4.Focus();
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
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

        private void ResetToAddMode()
        {
            isEditMode = false;
            selectedProfileId = -1;
            button1.Text = "Save All Changes";
            button2.Text = "Cancel";
        }

        private void SetEditMode(DataGridViewRow row)
        {
            isEditMode = true;
            selectedProfileId = Convert.ToInt32(row.Cells["ProfileId"].Value);

            textBox1.Text = row.Cells["FullName"].Value?.ToString() ?? "";
            textBox2.Text = row.Cells["Email"].Value?.ToString() ?? "";
            textBox3.Text = row.Cells["ContactNumber"].Value?.ToString() ?? "";

            string eventPreference = row.Cells["EventPreference"].Value?.ToString() ?? "";
            if (comboBox1.Items.Contains(eventPreference))
            {
                comboBox1.SelectedItem = eventPreference;
            }

            button1.Text = "Update Profile";
            button2.Text = "Cancel Edit";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                SetEditMode(selectedRow);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isEditMode)
            {
                // Cancel edit mode
                ClearFields();
                ResetToAddMode();
            }
            else
            {
                // Search functionality
                SearchProfile();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            try
            {
                Attendee_Dashboard attendee = new Attendee_Dashboard();
                attendee.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening Attendee Dashboard: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Show();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to exit?", "Confirm Exit",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        // Add these additional helper methods for better functionality

        // Method to add context menu for right-click operations
        private void AddContextMenu()
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();

            ToolStripMenuItem editItem = new ToolStripMenuItem("Edit");
            editItem.Click += (s, e) => {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    SetEditMode(dataGridView1.SelectedRows[0]);
                }
            };

            ToolStripMenuItem deleteItem = new ToolStripMenuItem("Delete");
            deleteItem.Click += (s, e) => DeleteProfile();

            contextMenu.Items.Add(editItem);
            contextMenu.Items.Add(deleteItem);

            dataGridView1.ContextMenuStrip = contextMenu;
        }

        // Method to handle keyboard shortcuts
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.F5:
                    LoadProfiles();
                    return true;
                case Keys.Escape:
                    if (isEditMode)
                    {
                        ClearFields();
                        ResetToAddMode();
                    }
                    return true;
                case Keys.Delete:
                    if (dataGridView1.Focused)
                    {
                        DeleteProfile();
                    }
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}