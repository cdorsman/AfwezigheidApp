using System.Windows.Forms;

namespace AfwezigheidsApp
{
    public partial class MainForm : Form
    {
        private readonly string _gebruikersRol;
        private readonly int _werknemerId;

        /// <summary>
        /// Het hoofdscherm van de applicatie dat verschillende functionaliteiten toont
        /// afhankelijk van de rol van de ingelogde gebruiker
        /// </summary>
        public MainForm(string gebruikersRol, int werknemerId)
        {
            _gebruikersRol = gebruikersRol;
            _werknemerId = werknemerId;
            InitializeComponent();
            ConfigureMenuBasedOnRole();
        }

        /// <summary>
        /// Stelt de menu-items samen gebaseerd op de rol van de gebruiker
        /// Teamleiders krijgen extra knoppen te zien
        /// </summary>
        private void ConfigureMenuBasedOnRole()
        {
            // Menu items voor alle gebruikers (zowel teamleiders als gewone medewerkers)
            var btnRegistreerVerlof = new Button
            {
                Text = "Registreer verlof",
                Width = 250,
                Height = 40,
                Location = new Point(275, 50)
            };
            btnRegistreerVerlof.Click += (s, e) => BtnRegistreerVerlof_Click(s!, e);

            var btnBekijkEigenVerlof = new Button
            {
                Text = "Bekijk alle verlof",
                Width = 250,
                Height = 40,
                Location = new Point(275, 120)
            };
            btnBekijkEigenVerlof.Click += (s, e) => BtnBekijkEigenVerlof_Click(s!, e);

            Controls.Add(btnRegistreerVerlof);
            Controls.Add(btnBekijkEigenVerlof);

            // Extra opties voor teamleiders - alleen tonen als de gebruiker teamleider is
            if (_gebruikersRol.Equals("teamleider", StringComparison.OrdinalIgnoreCase))
            {
                var btnBekijkAlleVerloven = new Button
                {
                    Text = "Bekijk alle verlofaanvragen",
                    Width = 250,
                    Height = 40,
                    Location = new Point(275, 190)
                };
                btnBekijkAlleVerloven.Click += (s, e) => BtnBekijkAlleVerloven_Click(s!, e);
                Controls.Add(btnBekijkAlleVerloven);
            }
        }

        /// <summary>
        /// Opent het formulier om nieuw verlof te registreren
        /// </summary>
        private void BtnRegistreerVerlof_Click(object sender, EventArgs e)
        {
            // Open het registratieformulier en geef de werknemer-ID door
            var form = new AfwezigheidRegistratieFormulier(_werknemerId);
            form.ShowDialog();
        }

        /// <summary>
        /// Opent het overzicht van eigen verlofaanvragen
        /// </summary>
        private void BtnBekijkEigenVerlof_Click(object sender, EventArgs e)
        {
            // Open het overzichtsformulier voor alleen eigen verlofaanvragen
            var form = new ZieAfwezigheidFormulier(_werknemerId, isTeamleider: false);
            form.ShowDialog();
        }

        /// <summary>
        /// Opent het overzicht van alle verlofaanvragen (alleen voor teamleiders)
        /// </summary>
        private void BtnBekijkAlleVerloven_Click(object sender, EventArgs e)
        {
            // Open het overzichtsformulier met alle verlofaanvragen
            var form = new ZieAfwezigheidFormulier(_werknemerId, isTeamleider: true);
            form.ShowDialog();
        }
    }
}
