using System.Windows.Forms;

namespace AfwezigheidsApp
{
    public partial class MainForm : Form
    {
        private readonly string _gebruikersRol;
        private readonly int _werknemerId;

        public MainForm(string gebruikersRol, int werknemerId)
        {
            _gebruikersRol = gebruikersRol;
            _werknemerId = werknemerId;
            InitializeComponent();
            ConfigureMenuBasedOnRole();
        }

        private void ConfigureMenuBasedOnRole()
        {
            // Menu items voor alle gebruikers
            var btnRegistreerVerlof = new Button
            {
                Text = "Registreer verlof",
                Width = 150,
                Height = 40,
                Location = new Point(50, 30)
            };
            btnRegistreerVerlof.Click += (s, e) => BtnRegistreerVerlof_Click(s!, e);

            var btnBekijkEigenVerlof = new Button
            {
                Text = "Bekijk eigen verlof",
                Width = 150,
                Height = 40,
                Location = new Point(50, 90)
            };
            btnBekijkEigenVerlof.Click += (s, e) => BtnBekijkEigenVerlof_Click(s!, e);

            Controls.Add(btnRegistreerVerlof);
            Controls.Add(btnBekijkEigenVerlof);

            // Extra opties voor teamleiders
            if (_gebruikersRol.Equals("teamleider", StringComparison.OrdinalIgnoreCase))
            {
                var btnBekijkAlleVerloven = new Button
                {
                    Text = "Bekijk alle verloven",
                    Width = 150,
                    Height = 40,
                    Location = new Point(50, 150)
                };
                btnBekijkAlleVerloven.Click += (s, e) => BtnBekijkAlleVerloven_Click(s!, e);
                Controls.Add(btnBekijkAlleVerloven);
            }
        }

        private void BtnRegistreerVerlof_Click(object sender, EventArgs e)
        {
            var form = new AfwezigheidRegistratieFormulier(_werknemerId);
            form.ShowDialog();
        }

        private void BtnBekijkEigenVerlof_Click(object sender, EventArgs e)
        {
            var form = new ZieAfwezigheidFormulier(_werknemerId, isTeamleider: false);
            form.ShowDialog();
        }

        private void BtnBekijkAlleVerloven_Click(object sender, EventArgs e)
        {
            var form = new ZieAfwezigheidFormulier(_werknemerId, isTeamleider: true);
            form.ShowDialog();
        }
    }
}
