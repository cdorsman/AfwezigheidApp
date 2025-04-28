using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AfwezigheidsApp
{
    public partial class ZieAfwezigheidFormulier : Form
    {
        private readonly int _werknemerId;
        private readonly bool _isTeamleider;
        private System.Windows.Forms.Timer? _refreshTimer;
        private DataGridView? dgvAfwezigheid;
        private MenuStrip? menuStrip;
        private ToolStripMenuItem? menuOpties;
        private ToolStripMenuItem? menuVerversen;
        private ToolStripMenuItem? menuSluiten;

        public ZieAfwezigheidFormulier(int werknemerId, bool isTeamleider)
        {
            _werknemerId = werknemerId;
            _isTeamleider = isTeamleider;
            InitializeComponent();
            ConfigureGrid();
            SetupRefreshTimer();
            LaadAfwezigheid();
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
            
            // Note: Removed the refresh button as it's now available in the menu
        }

        private void InitializeComponent()
        {
            this.dgvAfwezigheid = new DataGridView();
            this.menuStrip = new MenuStrip();
            this.menuOpties = new ToolStripMenuItem();
            this.menuVerversen = new ToolStripMenuItem();
            this.menuSluiten = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAfwezigheid)).BeginInit();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            
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
            this.menuVerversen,
            this.menuSluiten});
            this.menuOpties.Name = "menuOpties";
            this.menuOpties.Size = new System.Drawing.Size(69, 24);
            this.menuOpties.Text = "Opties";
            
            // Menu item Verversen
            this.menuVerversen.Name = "menuVerversen";
            this.menuVerversen.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.menuVerversen.Size = new System.Drawing.Size(189, 26);
            this.menuVerversen.Text = "Verversen";
            this.menuVerversen.Click += new System.EventHandler(this.MenuVerversen_Click);
            
            // Menu item Sluiten
            this.menuSluiten.Name = "menuSluiten";
            this.menuSluiten.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.menuSluiten.Size = new System.Drawing.Size(189, 26);
            this.menuSluiten.Text = "Sluiten";
            this.menuSluiten.Click += new System.EventHandler(this.MenuSluiten_Click);
            
            // Configuratie van het DataGridView met afwezigheidsgegevens
            this.dgvAfwezigheid.Dock = DockStyle.None;
            this.dgvAfwezigheid.Location = new System.Drawing.Point(0, 50);
            this.dgvAfwezigheid.Name = "dgvAfwezigheid";
            this.dgvAfwezigheid.Size = new System.Drawing.Size(800, 450);
            this.dgvAfwezigheid.TabIndex = 0;
            
            // ZieAfwezigheidFormulier
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.MaximumSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.dgvAfwezigheid);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "ZieAfwezigheidFormulier";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Afwezigheid";
            ((System.ComponentModel.ISupportInitialize)(this.dgvAfwezigheid)).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
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

            // Toon altijd alle verlofaanvragen voor alle werknemers, ongeacht de rol
            string query = @"SELECT 
                v.verlof_id,
                CONCAT(w.voornaam, ' ', w.achternaam) as naam,
                v.verlof_type,
                v.start_datum,
                v.eind_datum,
                v.status
            FROM Verlof v 
            JOIN Werknemers w ON v.werknemer_id = w.werknemer_id 
            ORDER BY v.start_datum DESC";  // Sorteer op startdatum (nieuwste bovenaan)

            using var cmd = new MySqlCommand(query, conn);
            
            // Vul het datagrid met de opgehaalde gegevens
            using var adapter = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dgvAfwezigheid.DataSource = dt;
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