using MySql.Data.MySqlClient;

namespace AfwezigheidsApp
{
    public partial class LoginFormulier : Form
    {
        public LoginFormulier()
        {
            InitializeComponent();
            Logger.Debug("LoginFormulier initialized");
        }

        private void btnLogin_Click(object? sender, EventArgs e)
        {
            Logger.Debug("Login button clicked");
            
            // Controleer of beide velden zijn ingevuld
            if (string.IsNullOrWhiteSpace(txtGebruikersnaam?.Text) || string.IsNullOrWhiteSpace(txtWachtwoord?.Text))
            {
                MessageBox.Show("Vul alle velden in.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using var conn = Database.GetConnection();
            try
            {
                conn.Open();

                // Genereer een SHA512 hash van het wachtwoord via MySQL
                using var hashCmd = new MySqlCommand("SELECT SHA2(@password, 512) as hash", conn);
                hashCmd.Parameters.AddWithValue("@password", txtWachtwoord.Text);
                string passwordHash = hashCmd.ExecuteScalar()?.ToString() ?? "";

                // Haal de gebruikersgegevens op uit de database
                string query = @"SELECT werknemer_id, wachtwoord_hash, rol 
                               FROM Gebruikers
                               WHERE gebruikersnaam = @gebruikersnaam";
                
                using var loginCmd = new MySqlCommand(query, conn);
                loginCmd.Parameters.AddWithValue("@gebruikersnaam", txtGebruikersnaam.Text);

                string storedHash = "";
                int werknemerId = 0;
                string rol = "";
                bool userFound = false;

                // Lees de gebruikersgegevens uit
                using (var reader = loginCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        userFound = true;
                        storedHash = reader.GetString("wachtwoord_hash");
                        werknemerId = reader.GetInt32("werknemer_id");
                        rol = reader.GetString("rol");
                    }
                    else
                    {
                        // Gebruiker niet gevonden, toon foutmelding
                        MessageBox.Show("Ongeldige inloggegevens.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                if (userFound)
                {
                    // Vergelijk de gegenereerde hash met de opgeslagen hash
                    bool passwordValid = passwordHash.Equals(storedHash, StringComparison.OrdinalIgnoreCase);

                    if (passwordValid)
                    {
                        // Login succesvol, open het MainForm als hoofdscherm
                        this.Hide();
                        var hoofdForm = new MainForm(rol, werknemerId);
                        hoofdForm.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        // Wachtwoord onjuist
                        MessageBox.Show("Ongeldige inloggegevens.", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log de fout en toon een gebruikersvriendelijke melding
                Logger.Debug($"Login error: {ex.Message}");
                MessageBox.Show($"Er is een fout opgetreden: {ex.Message}", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
