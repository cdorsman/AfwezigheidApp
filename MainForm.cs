using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AfwezigheidsApp
{
    public partial class MainForm : Form
    {
        private readonly string _gebruikersRol;
        private readonly int _werknemerId;
        private readonly bool _isTeamleider;
        private System.Windows.Forms.Timer? _refreshTimer;
        private DataGridView? dgvAfwezigheid;
        private MenuStrip? menuStrip;
        private ToolStripMenuItem? menuOpties;
        private ToolStripMenuItem? menuVerversen;
        private ToolStripMenuItem? menuRegistreerVerlof;
        private ToolStripMenuItem? menuBekijkAlleVerlof;
        private ToolStripMenuItem? menuSluiten;

        /// <summary>
        /// Het hoofdscherm van de applicatie dat verschillende functionaliteiten toont
        /// afhankelijk van de rol van de ingelogde gebruiker
        /// </summary>
        public MainForm(string gebruikersRol, int werknemerId)
        {
            _gebruikersRol = gebruikersRol;
            _werknemerId = werknemerId;
            _isTeamleider = _gebruikersRol.Equals("teamleider", StringComparison.OrdinalIgnoreCase);
            
            // Initialize the designer components first
            InitializeComponent();
            
            // Then create and configure our additional components
            SetupAdditionalControls();
            ConfigureGrid();
            SetupRefreshTimer();
            LaadAfwezigheid();
        }

        private void SetupAdditionalControls()
        {
            // Create UI components
            this.dgvAfwezigheid = new DataGridView();
            this.menuStrip = new MenuStrip();
            this.menuOpties = new ToolStripMenuItem();
            this.menuVerversen = new ToolStripMenuItem();
            this.menuRegistreerVerlof = new ToolStripMenuItem();
            this.menuBekijkAlleVerlof = new ToolStripMenuItem();
            this.menuSluiten = new ToolStripMenuItem();
            
            ((System.ComponentModel.ISupportInitialize)(this.dgvAfwezigheid)).BeginInit();
            this.menuStrip.SuspendLayout();
            
            // MenuStrip
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.menuOpties});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(800, 28);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip";
            
            // Menu Opties
            this.menuOpties.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.menuRegistreerVerlof,
                this.menuBekijkAlleVerlof,
                this.menuVerversen,
                this.menuSluiten});
            this.menuOpties.Name = "menuOpties";
            this.menuOpties.Size = new System.Drawing.Size(69, 24);
            this.menuOpties.Text = "Opties";
            
            // Menu item Registreer Verlof
            this.menuRegistreerVerlof.Name = "menuRegistreerVerlof";
            this.menuRegistreerVerlof.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.menuRegistreerVerlof.Size = new System.Drawing.Size(250, 26);
            this.menuRegistreerVerlof.Text = "Registreer verlof";
            this.menuRegistreerVerlof.Click += new System.EventHandler(this.MenuRegistreerVerlof_Click);
            
            // Menu item Bekijk Alle Verlof (alleen zichtbaar voor teamleiders)
            this.menuBekijkAlleVerlof.Name = "menuBekijkAlleVerlof";
            this.menuBekijkAlleVerlof.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.menuBekijkAlleVerlof.Size = new System.Drawing.Size(250, 26);
            this.menuBekijkAlleVerlof.Text = "Bekijk alle verlof";
            this.menuBekijkAlleVerlof.Visible = _isTeamleider; // Alleen zichtbaar voor teamleiders
            this.menuBekijkAlleVerlof.Click += new System.EventHandler(this.MenuBekijkAlleVerlof_Click);
            
            // Menu item Verversen
            this.menuVerversen.Name = "menuVerversen";
            this.menuVerversen.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.menuVerversen.Size = new System.Drawing.Size(250, 26);
            this.menuVerversen.Text = "Verversen";
            this.menuVerversen.Click += new System.EventHandler(this.MenuVerversen_Click);
            
            // Menu item Sluiten
            this.menuSluiten.Name = "menuSluiten";
            this.menuSluiten.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.menuSluiten.Size = new System.Drawing.Size(250, 26);
            this.menuSluiten.Text = "Sluiten";
            this.menuSluiten.Click += new System.EventHandler(this.MenuSluiten_Click);
            
            // Configuratie van het DataGridView met afwezigheidsgegevens
            this.dgvAfwezigheid.Dock = DockStyle.None;
            this.dgvAfwezigheid.Location = new System.Drawing.Point(0, 50);
            this.dgvAfwezigheid.Name = "dgvAfwezigheid";
            this.dgvAfwezigheid.Size = new System.Drawing.Size(800, 450);
            this.dgvAfwezigheid.TabIndex = 0;
            
            // Update form properties
            this.Text = "Afwezigheid";
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.MaximumSize = new System.Drawing.Size(800, 500);
            
            // Add controls to form
            this.Controls.Add(this.dgvAfwezigheid);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            
            ((System.ComponentModel.ISupportInitialize)(this.dgvAfwezigheid)).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
        }

        private void SetupRefreshTimer()
        {
            // Stop en verwijder de oude timer als die bestaat
            _refreshTimer?.Stop();
            _refreshTimer?.Dispose();
            
            // Maak een nieuwe timer aan
            _refreshTimer = new System.Windows.Forms.Timer();
            _refreshTimer.Interval = 30000; // Vernieuw elke 30 seconden
            _refreshTimer.Tick += (s, e) => LaadAfwezigheid();
            _refreshTimer.Start();
        }

        private void ConfigureGrid()
        {
            if (dgvAfwezigheid == null) return;

            // Stel eigenschappen in voor het datagrid
            dgvAfwezigheid.AutoGenerateColumns = false;
            dgvAfwezigheid.AllowUserToOrderColumns = true;

            // Definieer kolommen
            dgvAfwezigheid.Columns.Clear();
            dgvAfwezigheid.Columns.AddRange(
                new DataGridViewTextBoxColumn
                {
                    Name = "verlof_id",
                    DataPropertyName = "verlof_id",
                    HeaderText = "ID",
                    Width = 50,
                    Visible = false
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "naam",
                    DataPropertyName = "naam",
                    HeaderText = "Werknemer",
                    Width = 150
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "verlof_type",
                    DataPropertyName = "verlof_type",
                    HeaderText = "Type verlof",
                    Width = 100
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "start_datum",
                    DataPropertyName = "start_datum",
                    HeaderText = "Start datum",
                    Width = 120,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "d MMMM yyyy" }
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "eind_datum",
                    DataPropertyName = "eind_datum",
                    HeaderText = "Eind datum",
                    Width = 120,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "d MMMM yyyy" }
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "status",
                    DataPropertyName = "status",
                    HeaderText = "Status",
                    Width = 100
                }
            );
        }

        /// <summary>
        /// Haalt de afwezigheidsgegevens op uit de database en vult het datagrid
        /// </summary>
        private void LaadAfwezigheid()
        {
            // Check of het datagrid bestaat
            if (dgvAfwezigheid == null) return;
            
            // Maak verbinding met de database
            using var conn = Database.GetConnection();
            conn.Open();

            // Pas de query aan om alleen de afwezigheid van de ingelogde werknemer te tonen
            string query;
            if (_isTeamleider)
            {
                // Teamleiders zien alle verlofaanvragen
                query = @"SELECT 
                    v.verlof_id,
                    CONCAT(w.voornaam, ' ', w.achternaam) as naam,
                    v.verlof_type,
                    v.start_datum,
                    v.eind_datum,
                    v.status
                FROM Verlof v 
                JOIN Werknemers w ON v.werknemer_id = w.werknemer_id 
                ORDER BY v.start_datum DESC";  // Sorteer op startdatum (nieuwste bovenaan)
            }
            else
            {
                // Werknemers zien alleen hun eigen verlofaanvragen
                query = @"SELECT 
                    v.verlof_id,
                    CONCAT(w.voornaam, ' ', w.achternaam) as naam,
                    v.verlof_type,
                    v.start_datum,
                    v.eind_datum,
                    v.status
                FROM Verlof v 
                JOIN Werknemers w ON v.werknemer_id = w.werknemer_id 
                WHERE v.werknemer_id = @WerknemerId
                ORDER BY v.start_datum DESC";  // Sorteer op startdatum (nieuwste bovenaan)
            }

            using var cmd = new MySqlCommand(query, conn);
            if (!_isTeamleider)
            {
                // Voeg parameter toe voor werknemer ID
                cmd.Parameters.AddWithValue("@WerknemerId", _werknemerId);
            }
            
            // Vul het datagrid met de opgehaalde gegevens
            using var adapter = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dgvAfwezigheid.DataSource = dt;
        }

        private void MenuRegistreerVerlof_Click(object? sender, EventArgs e)
        {
            // Open het registratieformulier en geef de werknemer-ID door
            var form = new AfwezigheidRegistratieFormulier(_werknemerId);
            form.ShowDialog();
            
            // Ververs het afwezigheidsoverzicht nadat het formulier is gesloten
            LaadAfwezigheid();
        }

        private void MenuBekijkAlleVerlof_Click(object? sender, EventArgs e)
        {
            // Deze functie wordt alleen aangeroepen als de gebruiker een teamleider is
            // omdat de menuknop alleen dan zichtbaar is
            LaadAfwezigheid();
        }

        private void MenuVerversen_Click(object? sender, EventArgs e)
        {
            LaadAfwezigheid();
        }

        private void MenuSluiten_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Stop en verwijder de timer bij het sluiten van het formulier
            base.OnFormClosing(e);
            _refreshTimer?.Stop();
            _refreshTimer?.Dispose();
        }
    }
}
