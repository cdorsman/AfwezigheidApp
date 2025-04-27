using MySql.Data.MySqlClient;
using AfwezigheidsApp;

namespace AfwezigheidsApp
{
    public partial class AfwezigheidRegistratieFormulier : Form
    {
        private readonly int _werknemerId;

        // Start het formulier en zijn componenten
        public AfwezigheidRegistratieFormulier(int werknemerId)
        {
            _werknemerId = werknemerId;
            InitializeComponent();
            // Set minimum date to today
            dtpStartDatum.MinDate = DateTime.Today;
            dtpEindDatum.MinDate = DateTime.Today;
        }

        private bool HeeftOverlappendVerlof(DateTime startDatum, DateTime eindDatum)
        {
            using var conn = Database.GetConnection();
            conn.Open();
            string query = @"
                SELECT COUNT(*) FROM Verlof 
                WHERE werknemer_id = @werknemer_id 
                AND status != 'Afgekeurd'
                AND ((start_datum <= @eind_datum AND eind_datum >= @start_datum)
                OR (start_datum <= @start_datum AND eind_datum >= @eind_datum)
                OR (start_datum >= @start_datum AND eind_datum <= @eind_datum))";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@werknemer_id", _werknemerId);
            cmd.Parameters.AddWithValue("@start_datum", startDatum);
            cmd.Parameters.AddWithValue("@eind_datum", eindDatum);

            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        // Stuur de afwezigheidsgegevens naar de database
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string verlofType = cmbVerlofType.SelectedItem?.ToString() ?? "";
            DateTime startDatum = dtpStartDatum.Value.Date;
            DateTime eindDatum = dtpEindDatum.Value.Date;

            if (string.IsNullOrEmpty(verlofType))
            {
                MessageBox.Show("Selecteer een type verlof.", "Validatie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (eindDatum < startDatum)
            {
                MessageBox.Show("Einddatum kan niet voor de startdatum liggen.", "Validatie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (startDatum < DateTime.Today)
            {
                MessageBox.Show("Startdatum kan niet in het verleden liggen.", "Validatie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (HeeftOverlappendVerlof(startDatum, eindDatum))
                {
                    MessageBox.Show("Er is al verlof geregistreerd voor (een deel van) deze periode.", "Validatie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    string query = "INSERT INTO Verlof (werknemer_id, verlof_type, start_datum, eind_datum, status) VALUES (@werknemer_id, @verlof_type, @start_datum, @eind_datum, 'In behandeling')";
                    using var cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@werknemer_id", _werknemerId);
                    cmd.Parameters.AddWithValue("@verlof_type", verlofType);
                    cmd.Parameters.AddWithValue("@start_datum", startDatum);
                    cmd.Parameters.AddWithValue("@eind_datum", eindDatum);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Aanvraag is ingediend.", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout bij het registreren van afwezigheid: {ex}");
                MessageBox.Show($"Fout bij het indienen: {ex.Message}", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
