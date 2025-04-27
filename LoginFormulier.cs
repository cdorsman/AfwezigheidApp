using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System;

namespace AfwezigheidsApp
{
    public partial class LoginFormulier : Form
    {
        public LoginFormulier()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtGebruikersnaam?.Text) || string.IsNullOrWhiteSpace(txtWachtwoord?.Text))
            {
                MessageBox.Show("Vul alle velden in.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using var conn = Database.GetConnection();
            try
            {
                conn.Open();
                string query = @"SELECT werknemer_id, wachtwoord_hash, rol FROM Werknemers 
                               WHERE gebruikersnaam = @gebruikersnaam";
                using var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@gebruikersnaam", txtGebruikersnaam.Text);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string hashedPassword = reader.GetString("wachtwoord_hash");
                    var sha256Hash = System.Security.Cryptography.SHA256.HashData(
                        System.Text.Encoding.UTF8.GetBytes(txtWachtwoord.Text));
                    var base64Hash = Convert.ToBase64String(sha256Hash);
                    
                    if (hashedPassword == base64Hash)
                    {
                        int werknemerId = reader.GetInt32("werknemer_id");
                        string rol = reader.GetString("rol");
                        
                        this.Hide();
                        var mainForm = new MainForm(rol, werknemerId);
                        mainForm.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Ongeldige inloggegevens.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Ongeldige inloggegevens.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Er is een fout opgetreden: {ex.Message}", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
