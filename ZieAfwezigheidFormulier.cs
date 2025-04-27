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
            _refreshTimer?.Stop();
            _refreshTimer?.Dispose();
            
            _refreshTimer = new System.Windows.Forms.Timer();
            _refreshTimer.Interval = 30000; // Refresh every 30 seconds
            _refreshTimer.Tick += (s, e) => LaadAfwezigheid();
            _refreshTimer.Start();
        }

        private void ConfigureGrid()
        {
            if (dgvAfwezigheid == null) return;

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
                    Width = 100,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "dd-MM-yyyy" }
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "eind_datum",
                    DataPropertyName = "eind_datum",
                    HeaderText = "Eind datum",
                    Width = 100,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "dd-MM-yyyy" }
                },
                new DataGridViewTextBoxColumn
                {
                    Name = "status",
                    DataPropertyName = "status",
                    HeaderText = "Status",
                    Width = 100
                }
            );

            var btnRefresh = new Button
            {
                Text = "Vernieuwen",
                Width = 100,
                Height = 30,
                Location = new Point(10, 10)
            };
            btnRefresh.Click += (s, e) => LaadAfwezigheid();
            Controls.Add(btnRefresh);
        }

        private void InitializeComponent()
        {
            this.dgvAfwezigheid = new DataGridView();
            this.SuspendLayout();
            // 
            // dgvAfwezigheid
            // 
            this.dgvAfwezigheid.AllowUserToAddRows = false;
            this.dgvAfwezigheid.AllowUserToDeleteRows = false;
            this.dgvAfwezigheid.ReadOnly = true;
            this.dgvAfwezigheid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAfwezigheid.Dock = DockStyle.Fill;
            this.dgvAfwezigheid.Location = new System.Drawing.Point(0, 0);
            this.dgvAfwezigheid.Name = "dgvAfwezigheid";
            this.dgvAfwezigheid.Size = new System.Drawing.Size(600, 300);
            this.dgvAfwezigheid.TabIndex = 0;
            // 
            // ZieAfwezigheidFormulier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 300);
            this.Controls.Add(this.dgvAfwezigheid);
            this.Name = "ZieAfwezigheidFormulier";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Afwezigheid";
            this.ResumeLayout(false);
        }

        private void LaadAfwezigheid()
        {
            if (dgvAfwezigheid == null) return;
            
            using var conn = Database.GetConnection();
            conn.Open();
            string query = _isTeamleider
                ? @"SELECT 
                    v.verlof_id,
                    CONCAT(w.voornaam, ' ', w.achternaam) as naam,
                    v.verlof_type,
                    v.start_datum,
                    v.eind_datum,
                    v.status
                FROM Verlof v 
                JOIN Werknemers w ON v.werknemer_id = w.werknemer_id 
                ORDER BY v.start_datum DESC"
                : @"SELECT 
                    v.verlof_id,
                    CONCAT(w.voornaam, ' ', w.achternaam) as naam,
                    v.verlof_type,
                    v.start_datum,
                    v.eind_datum,
                    v.status
                FROM Verlof v 
                JOIN Werknemers w ON v.werknemer_id = w.werknemer_id 
                WHERE v.werknemer_id = @werknemer_id
                ORDER BY v.start_datum DESC";

            using var cmd = new MySqlCommand(query, conn);
            if (!_isTeamleider)
            {
                cmd.Parameters.AddWithValue("@werknemer_id", _werknemerId);
            }

            using var adapter = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dgvAfwezigheid.DataSource = dt;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            _refreshTimer?.Stop();
            _refreshTimer?.Dispose();
        }
    }
}