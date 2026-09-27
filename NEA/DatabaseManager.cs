using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.;
using System.IO;
using Microsoft.Data.Sqlite;

namespace NEA
{
    public class DatabaseManager
    {
        // Define the file name and connection string
        private const string DbFileName = "nea_database.db";
        private readonly string connectionString = $"Data Source={DbFileName}";

        public void InitializeDatabase()
        {
            // Creates the database file automatically if it doesn't exist
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // Create your tables here
                string createTableQuery = @"
               CREATE TABLE IF NOT EXISTS User (
                    UserID varchar(50) PRIMARY KEY,
                    Email 
                    PasswordHash VARCHAR(50) NOT NULL UNIQUE,
                    DateCreated VARCHAR(6)
                    
                );";

                using (var command = new SqliteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
