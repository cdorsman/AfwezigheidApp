using MySql.Data.MySqlClient;

namespace AfwezigheidsApp
{
    public static class Database
    {
        private static readonly string connectionString =
            Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
            throw new Exception("DB_CONNECTION_STRING environment variable is not set");

        private static readonly MySqlConnectionStringBuilder builder;

        static Database()
        {
            builder = new MySqlConnectionStringBuilder(connectionString)
            {
                MinimumPoolSize = 5,
                MaximumPoolSize = 20,
                ConnectionLifeTime = 300 // 5 minutes
            };
        }

        public static MySqlConnection GetConnection()
        {
            var conn = new MySqlConnection(builder.ConnectionString);
            try
            {
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
