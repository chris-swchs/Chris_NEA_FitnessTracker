using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace NEA
{

    // REMINDER:
    // MAKE GET GetUserAvailableEquipment() AND GetExerciseAvailableEquipment() RETURN A LIST OF DATA WITH THE ENUMERATION TYPE Equipment
    //Give GetMultiValuesInFieldFromKey() a more suitable name

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
                string CreateTables = @"
               CREATE TABLE IF NOT EXISTS User (
                    UserID TEXT PRIMARY KEY UNIQUE,
                    Email  TEXT NOT NULL UNIQUE,
                    PasswordHash TEXT NOT NULL,
                    DateCreated TEXT
                );
                
                CREATE TABLE IF NOT EXISTS Exercise (
                    ExerciseID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    ExerciseType TEXT NOT NULL,
                    TargetMuscle TEXT NOT NULL,
                    Details TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS UserAvailableEquipment (
                    UserAvailableEquipmentID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Equipment TEXT NOT NULL,
                    UserID TEXT,
                    FOREIGN KEY (UserID) REFERENCES User(UserID)
                );

                CREATE TABLE IF NOT EXISTS ExerciseAvailableEquipment (
                    ExerciseAvailableEquipmentID INTEGER PRIMARY KEY AUTOINCREMENT,
                    Equipment TEXT NOT NULL,
                    ExerciseID INTEGER,
                    FOREIGN KEY (ExerciseID) REFERENCES Exercise(ExerciseID)
                );

                CREATE TABLE IF NOT EXISTS UserSetTargets (
                    UserSetTargetsID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,,
                    UserID NOT NULL TEXT,
                    FOREIGN KEY (UserID) REFERENCES User(UserID),
                    ExerciseID INTEGER NOT NULL,
                    FOREIGN KEY (ExerciseID) REFERENCES Exercise(ExerciseID)
                    Reps INTEGER NOT NULL CHECK (Reps > 0),
                    Weight REAL NOT NULL CHECK (Weight > 0),
                    Order INTEGER NOT NULL CHECK (Order > 0),
                );

                CREATE TABLE IF NOT EXISTS UserTimedTargets (
                    UserSetTargetsID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,,
                    UserID NOT NULL TEXT,
                    FOREIGN KEY (UserID) REFERENCES User(UserID),
                    ExerciseID NOT NULL INTEGER,
                    FOREIGN KEY (ExerciseID) REFERENCES Exercise(ExerciseID)
                    Duration INTEGER NOT NULL CHECK (Duration > 0),
                    Weight REAL NOT NULL CHECK (Weight > 0),
                    Speed REAL CHECK (Weight > 0),
                );
                ";

                using (var command = new SqliteCommand(CreateTables, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        // GENERALISED DATABASE FUNCTIONS

        //Retrieves an element of data within a field for a record inside of a table
        private string GetFieldFromKey(string table, string pkFieldName, string pk, string field)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                    SELECT @field
                    FROM @table
                    WHERE @pkFieldName = @pk;";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@field", field);
                    command.Parameters.AddWithValue("@table", table);
                    command.Parameters.AddWithValue("@pkFieldName", pkFieldName);
                    command.Parameters.AddWithValue("@pk", pk);


                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        return result.ToString();
                    }

                    return null;
                }
            }
        }

        //Retrieves the primary key for a record in a table with 2 specific attributes
        private string GetKeyFromTwoFields(string table, string pkFieldName, string field1Name,
            string field1, string field2Name, string field2)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                    SELECT @pkFieldName
                    FROM @table
                    WHERE @field1Name = @field1;
                    AND @field2name = @field2";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@pkFieldName", pkFieldName);
                    command.Parameters.AddWithValue("@table", table);
                    command.Parameters.AddWithValue("@field1Name", field1Name);
                    command.Parameters.AddWithValue("@field1", field1);
                    command.Parameters.AddWithValue("@field2name", field2Name);
                    command.Parameters.AddWithValue("@field2", field2);


                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        return result.ToString();
                    }

                    return null;
                }
            }
        }

        //Retrieves an array of elements of data within a field for all records with a specific attribute
        private string[] GetMultiValuesInFieldFromKey(string table, string pkFieldName, string keyAttribute, string field)
        {
            List<string> Cells = new List<string>();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                    SELECT @field
                    FROM @table
                    WHERE @pkFieldName = @pk;";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@field", field);
                    command.Parameters.AddWithValue("@table", table);
                    command.Parameters.AddWithValue("@pkFieldName", pkFieldName);
                    command.Parameters.AddWithValue("@pk", keyAttribute);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Cells.Add(reader[table].ToString());
                        }
                    }
                }
                return Cells.ToArray();
            }
        }


        // EXERCISE METHODS


        //Functions for retrieving a field of data from the ExerciseTable

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

        public void AddExerciseRecord(string name, ExerciseType exerciseType, MuscleGroup targetMuscle, string details)
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

        public string GetExerciseName(string ExerciseID)
        {
            return GetFieldFromKey("Exercise", "ExerciseID", ExerciseID, "Name");
        } 

        public string GetExerciseType(string ExerciseID)
        {
            return GetFieldFromKey("Exercise", "ExerciseID", ExerciseID, "ExerciseType");
        }

        public string GetExerciseTargetMuscle(string ExerciseID)
        {
            return GetFieldFromKey("Exercise", "ExerciseID", ExerciseID, "TargetMuscle");
        }

        public string GetExerciseDetails(string ExerciseID)
        {
            return GetFieldFromKey("Exercise", "ExerciseID", ExerciseID, "ExerciseDetails");
        }


        // USER-AVAILABLE-EQUIPMENT METHODS

        //Associates a piece of equipment with a user
        public void AddEquipmentForUser(Equipment Equipment, string UserID)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                INSERT INTO UserAvailableEquipment
                (Equipment, UserID)
                VALUES
                (@Equipment, @UserID);";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ExerciseType", Equipment.ToString());
                    command.Parameters.AddWithValue("@UserID", UserID);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Returns a list of all the available equipment the user has

        public string[] GetUserAvailableEquipment(string UserID)
        {
            List<string> Equipments = new List<string>();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                     SELECT Equipment
                     FROM UserAvailableEquipment
                     WHERE UserID = @UserID;";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Equipments.Add(reader["Equipment"].ToString());
                        }
                    }
                }
                return Equipments.ToArray();
            }
        }

        // EXERCISE-AVAILABLE-EQUIPMENT METHODS

        //Makes it so that an exercise supports / requires a piece of equipment
        public void AddEquipmentForExercise(Equipment Equipment, int ExerciseID)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                INSERT INTO UserAvailableEquipment
                (Equipment, ExerciseID)
                VALUES
                (@Equipment, @ExerciseID);";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Equipment", Equipment.ToString());
                    command.Parameters.AddWithValue("@ExerciseID", ExerciseID);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Returns a list of all the equipment for which an exercise supports 
        public string[] GetExerciseAvailableEquipment(string ExerciseID)
        {
            List<string> Equipments = new List<string>();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                     SELECT Equipment
                     FROM UserAvailableEquipment
                     WHERE ExerciseID = @ExerciseID;";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ExerciseID", ExerciseID);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Equipments.Add(reader["Equipment"].ToString());
                        }
                    }
                }
                return Equipments.ToArray();
            }
        }

        // USER-SET-TARGETS METHODS
        public void AddUserSetTarget(string UserID, int ExerciseID, int Reps, float Weight, int Order)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                INSERT INTO UserTimedTargets
                (UserID, ExerciseID, Reps, Weight, Order)
                VALUES
                (@UserID, @ExerciseID, @Reps, @Weight, @Order);";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@ExerciseID", ExerciseID);
                    command.Parameters.AddWithValue("@Duration", Reps);
                    command.Parameters.AddWithValue("@Weight", Weight);
                    command.Parameters.AddWithValue("@Speed", Order);
                    command.Parameters.AddWithValue("@Speed", Order);

                    command.ExecuteNonQuery();
                }
            }
        }

        // USER-TIMED-TARGETS METHODS
        public void AddUserTimedTarget(string UserID, int ExerciseID, int Duration, float Weight, float Speed)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                INSERT INTO UserTimedTargets
                (UserID, ExerciseID, Duration, Weight, Speed)
                VALUES
                (@UserID, @ExerciseID, @Duration, @Weight, @Speed);";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@ExerciseID", ExerciseID);
                    command.Parameters.AddWithValue("@Duration", Duration);
                    command.Parameters.AddWithValue("@Weight", Weight);
                    command.Parameters.AddWithValue("@Speed", Speed);

                    command.ExecuteNonQuery();
                }
            }
        }
        public string GetTargetsForUserSetExercise(string UserID, string ExerciseID)
        {
            string PK = GetKeyFromTwoFields("UserSetTargets", "UserSetTargetsID", "UserID", UserID, "ExerciseID", ExerciseID);
            string[] repetitions = GetMultiValuesInFieldFromKey("Exercise", "ExerciseID", ExerciseID, "ExerciseDetails");
            return "";
        }

    }
}

