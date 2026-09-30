using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using Microsoft.Data.Sqlite;
using System.Security.Cryptography;

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

                // Creates USER table
                string createUserTableQuery = @"
               CREATE TABLE IF NOT EXISTS User (
                    UserID TEXT PRIMARY KEY,
                    Email  TEXT NOT NULL UNIQUE
                    PasswordHash TEXT NOT NULL UNIQUE,
                    DateCreated TEXT
                    
                );";

                // Creates EXERCISE table
                string createExerciseTableQuery = @"
               CREATE TABLE IF NOT EXISTS User (
                    ExerciseID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL
                    ExerciseType TEXT NOT NULL,
                    TargetMuscle TEXT NOT NULL
                    Details TEXT NOT NULL
                );";

                using (var command = new SqliteCommand(createUserTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Adds a record to the exercise table
                void AddExerciseRecord(string name, ExerciseType ExerciseType, MuscleGroup TargetMuscle, string Details)
                {
                    string sql = @"INSERT INTO Exercises (Name, Type, TargetMuscle, Details) 
                   VALUES (@Name, @ExerciseType, @TargetMuscle, @Details);";

                    using (var command = new SqliteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);

                        // Using the enum datatype all ExerciseType and TargetMuscle entries are featured in the program
                        command.Parameters.AddWithValue("@ExerciseType", ExerciseType.ToString());
                        command.Parameters.AddWithValue("@TargetMuscle", TargetMuscle.ToString());

                        command.Parameters.AddWithValue("@Details", Details);

                        command.ExecuteNonQuery();

                        Console.WriteLine("Database and 'Users' table created successfully!");
                    }
                }
            }
        }
    }
}
