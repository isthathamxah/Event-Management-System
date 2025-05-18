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
    public partial class ManageTickets : Form
    {
        string connectionString = "Data Source=DESKTOP-Q3RNHLM;Initial Catalog=Project;Integrated Security=True;TrustServerCertificate=True";

        public ManageTickets()
        {

            this.Load += new System.EventHandler(this.ManageTickets_Load);
            InitializeComponent();
        }

        // Method to load tickets from the database into the DataGridView
        private void LoadTickets()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT TicketID, EventName, TicketType, Price, NumberOfTickets FROM Tickets";
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    dataGridView1.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Button click to go back to the main menu
        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
            Organizer_Dashboard od = new Organizer_Dashboard();
            od.Show();
        }

        // Button click to add a new ticket
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string eventName = textBox7.Text;
                string ticketID = textBox2.Text;
                string ticketType = checkedListBox1.CheckedItems.Count > 0
                    ? checkedListBox1.CheckedItems[0].ToString() : string.Empty;
                decimal price = decimal.Parse(textBox6.Text);
                int numberOfTickets = (int)numericUpDown1.Value;

                if (string.IsNullOrEmpty(eventName) || string.IsNullOrEmpty(ticketID) || string.IsNullOrEmpty(ticketType))
                {
                    MessageBox.Show("Please fill in all the fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Connect to database and insert the new ticket
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Tickets (EventName, TicketType, Price, NumberOfTickets) " +
                                   "VALUES (@EventName, @TicketType, @Price, @NumberOfTickets)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@EventName", eventName);
                    command.Parameters.AddWithValue("@TicketType", ticketType);
                    command.Parameters.AddWithValue("@Price", price);
                    command.Parameters.AddWithValue("@NumberOfTickets", numberOfTickets);

                    connection.Open();
                    command.ExecuteNonQuery();  // Executes the insert command
                    connection.Close();
                }

                MessageBox.Show("Ticket added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadTickets();  // Refresh the DataGridView after adding the ticket
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Button click to delete a selected ticket
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    int ticketID = Convert.ToInt32(row.Cells["TicketID"].Value);  // Assuming TicketID column exists in DataGridView

                    // Connect to database and delete the ticket
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string query = "DELETE FROM Tickets WHERE TicketID = @TicketID";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@TicketID", ticketID);

                        connection.Open();
                        command.ExecuteNonQuery();  // Executes the delete command
                        connection.Close();
                    }
                }

                MessageBox.Show("Selected ticket(s) deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadTickets();  // Refresh the DataGridView after deletion
            }
            else
            {
                MessageBox.Show("Please select a ticket to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Button click to update a selected ticket
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                int ticketID = Convert.ToInt32(selectedRow.Cells["TicketID"].Value);  // Assuming TicketID column exists in DataGridView
                string eventName = textBox7.Text;
                string ticketType = checkedListBox1.CheckedItems.Count > 0 ? checkedListBox1.CheckedItems[0].ToString() : string.Empty;
                decimal price = decimal.Parse(textBox6.Text);
                int numberOfTickets = (int)numericUpDown1.Value;

                // Connect to database and update the ticket
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Tickets SET EventName = @EventName, TicketType = @TicketType, Price = @Price, NumberOfTickets = @NumberOfTickets WHERE TicketID = @TicketID";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@TicketID", ticketID);
                    command.Parameters.AddWithValue("@EventName", eventName);
                    command.Parameters.AddWithValue("@TicketType", ticketType);
                    command.Parameters.AddWithValue("@Price", price);
                    command.Parameters.AddWithValue("@NumberOfTickets", numberOfTickets);

                    connection.Open();
                    command.ExecuteNonQuery();  // Executes the update command
                    connection.Close();
                }

                MessageBox.Show("Ticket updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadTickets();  // Refresh the DataGridView after updating
            }
            else
            {
                MessageBox.Show("Please select a single ticket to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Button click to reset all fields
        private void button4_Click(object sender, EventArgs e)
        {
            textBox7.Clear();
            textBox2.Clear();
            checkedListBox1.ClearSelected();
            textBox6.Clear();
            numericUpDown1.Value = 0;
        }

        // Button click to exit the application
        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Load tickets when the form is loaded
        private void ManageTickets_Load(object sender, EventArgs e)
        {
            LoadTickets();  // Load tickets when the form is loaded
        }
    }
}

