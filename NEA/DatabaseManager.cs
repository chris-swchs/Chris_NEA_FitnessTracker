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
                    Email  TEXT NOT NULL UNIQUE,
                    PasswordHash TEXT NOT NULL UNIQUE,
                    DateCreated TEXT
                    
                );";
                    
                // Creates EXERCISE table
                string createExerciseTableQuery = @"
               CREATE TABLE IF NOT EXISTS Exercise (
                    ExerciseID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    ExerciseType TEXT NOT NULL,
                    TargetMuscle TEXT NOT NULL,
                    Details TEXT NOT NULL
                );";

                using (var command = new SqliteCommand(createExerciseTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddExerciseRecord(string name,ExerciseType exerciseType, MuscleGroup targetMuscle, string details)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                INSERT INTO Exercise
                (Name, ExerciseType, TargetMuscle, Details)
                VALUES
                (@Name, @ExerciseType, @TargetMuscle, @Details);";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@ExerciseType", exerciseType.ToString());
                    command.Parameters.AddWithValue("@TargetMuscle", targetMuscle.ToString());
                    command.Parameters.AddWithValue("@Details", details);

                    command.ExecuteNonQuery();
    }
}
        }

        //Functions for retrieving a field of data from the ExerciseTable
        //Create one single function responsible for retrieving a specific attribute, in order to reduce duplication of code

        public int GetExerciseID(string exerciseName)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                     SELECT ExerciseID
                     FROM Exercise
                     WHERE Name = @Name;";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Name", exerciseName);

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }

                    return -1;
                }
            }
        }
        public string GetExerciseName(int ExerciseID)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                    SELECT Name
                    FROM Exercise
                    WHERE ExerciseID = @ExerciseID;";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ExerciseID", ExerciseID);

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        return result.ToString();
                    }

                    return null;
                }
            }
        }

        public string GetExerciseType(int ExerciseID)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                    SELECT ExerciseType
                    FROM Exercise
                    WHERE ExerciseID = @ExerciseID;";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ExerciseID", ExerciseID);

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        return result.ToString();
                    }

                    return null;
                }
            }
        }
        public string GetExerciseTargetMuscle(int ExerciseID)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                    SELECT TargetMuscle
                    FROM Exercise
                    WHERE ExerciseID = @ExerciseID;";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ExerciseID", ExerciseID);

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        return result.ToString();
                    }

                    return null;
                }
            }
        }

        public string GetExerciseDetails(int ExerciseID)
        {
           using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                    SELECT Details
                    FROM Exercise
                    WHERE ExerciseID = @ExerciseID;";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ExerciseID", ExerciseID);

                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        return result.ToString();
                    }

                    return null;
                }
            }
        }
    }
}

