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
    public partial class Payment : Form
    {
        string connectionString = "Data Source=DESKTOP-Q3RNHLM;Initial Catalog=Project;Integrated Security=True;TrustServerCertificate=True";

        public Payment()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Go to attendee dashboard
            this.Close();
            Attendee_Dashboard attendee = new Attendee_Dashboard();
            attendee.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(comboBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            try
            {
                int paymentId = 0;

                // Prepare the SQL query to insert data into the Payment table
                string query = "INSERT INTO Payment (DueAmount, AccountNumber, CVC, ExpiryDate) " +
                               "OUTPUT INSERTED.PaymentID " +
                               "VALUES (@DueAmount, @AccountNumber, @CVC, @ExpiryDate)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection
                    connection.Open();

                    // Create the SQL command
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@DueAmount", textBox1.Text);   // Due Amount
                    command.Parameters.AddWithValue("@AccountNumber", textBox2.Text); // Account Number
                    command.Parameters.AddWithValue("@CVC", textBox3.Text);         // CVC
                    command.Parameters.AddWithValue("@ExpiryDate", textBox4.Text);  // Expiry Date

                    // Execute the query and get the inserted PaymentID
                    paymentId = (int)command.ExecuteScalar();
                }

                // Show success message
                MessageBox.Show("Payment processed successfully!");

                // Generate and show ticket
                ShowTicket(paymentId);

                // Close the payment form
                this.Close();
            }
            catch (Exception ex)
            {
                // Show an error message if something goes wrong
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void ShowTicket(int paymentId)
        {
            try
            {
                // Create ticket information
                string ticketInfo = GenerateTicketInfo(paymentId);

                // Option 1: Show ticket in a message box (simple approach)
                MessageBox.Show(ticketInfo, "Your Ticket", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Option 2: If you have a Ticket_Download form, uncomment the lines below
                // Ticket_Download ticketDownload = new Ticket_Download();
                // ticketDownload.SetTicketInfo(ticketInfo); // You'll need to add this method to Ticket_Download
                // ticketDownload.Show();

                // Option 3: Save ticket to file and show location
                // SaveTicketToFile(ticketInfo, paymentId);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating ticket: " + ex.Message);
            }
        }

        private string GenerateTicketInfo(int paymentId)
        {
            StringBuilder ticket = new StringBuilder();

            ticket.AppendLine("═══════════════════════════════");
            ticket.AppendLine("          EVENT TICKET         ");
            ticket.AppendLine("═══════════════════════════════");
            ticket.AppendLine();
            ticket.AppendLine($"Ticket ID: TKT-{paymentId:D6}");
            ticket.AppendLine($"Payment ID: {paymentId}");
            ticket.AppendLine($"Date Issued: {DateTime.Now:MM/dd/yyyy HH:mm}");
            ticket.AppendLine($"Amount Paid: ${textBox1.Text}");
            ticket.AppendLine();

            // You can add more event details here if you have them
            // For example, if you store event information in session or pass it to this form
            ticket.AppendLine("Event Details:");
            ticket.AppendLine("─────────────────────────────");
            ticket.AppendLine("Event Name: [Event Name Here]");
            ticket.AppendLine("Date: [Event Date Here]");
            ticket.AppendLine("Venue: [Event Venue Here]");
            ticket.AppendLine("Time: [Event Time Here]");
            ticket.AppendLine();

            ticket.AppendLine("═══════════════════════════════");
            ticket.AppendLine("     Thank you for booking!    ");
            ticket.AppendLine("   Please keep this ticket     ");
            ticket.AppendLine("═══════════════════════════════");

            return ticket.ToString();
        }

        // Optional: Save ticket to a text file
        private void SaveTicketToFile(string ticketInfo, int paymentId)
        {
            try
            {
                string fileName = $"Ticket_{paymentId}_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);

                System.IO.File.WriteAllText(filePath, ticketInfo);

                MessageBox.Show($"Ticket saved to desktop as: {fileName}", "Ticket Saved");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving ticket file: " + ex.Message);
            }
        }
    }
}