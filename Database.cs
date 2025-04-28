using MySql.Data.MySqlClient;

namespace AfwezigheidsApp
{
    public static class Database
    {
        // Haal de database verbindingsstring uit environment variabelen
        // Dit is veiliger dan het direct in de code plaatsen
        private static readonly string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") 
            ?? throw new InvalidOperationException("DB_CONNECTION_STRING environment variable is not set.");

        private static readonly MySqlConnectionStringBuilder builder;

        static Database()
        {
            // Initialiseer de connection string builder met enkele performance instellingen
            builder = new MySqlConnectionStringBuilder(connectionString)
            {
                MinimumPoolSize = 5,       // Minimum aantal verbindingen in de pool
                MaximumPoolSize = 20,      // Maximum aantal verbindingen in de pool
                ConnectionLifeTime = 300   // 5 minuten levensduur voor connecties
            };
        }

        /// <summary>
        /// Geeft een nieuwe database verbinding terug
        /// Test eerst of de verbinding werkt voordat deze wordt teruggegeven
        /// </summary>
        public static MySqlConnection GetConnection()
        {
            var conn = new MySqlConnection(builder.ConnectionString);
            try
            {
                // Test de verbinding door kort te openen en weer te sluiten
                conn.Open();
                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Databaseverbinding mislukt: " + ex.Message);
            }
            return new MySqlConnection(builder.ConnectionString);
        }
    }
}
