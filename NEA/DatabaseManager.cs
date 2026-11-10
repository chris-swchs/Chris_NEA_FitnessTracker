using Microsoft.Data.Sqlite;
using NEA.Models.Entities;
using NEA.Models.LinkedLists;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace NEA
{

    // REMINDER:
    // MAKE GET GetUserAvailableEquipment() AND GetExerciseAvailableEquipment() RETURN A LIST OF DATA WITH THE ENUMERATION TYPE Equipment
    //Give GetMultiValuesInFieldFromKey() a more suitable name
    // FIX TABLE NAMES

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
                      Email TEXT NOT NULL UNIQUE,
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
                   
                  CREATE TABLE IF NOT EXISTS Workout (
                      WorkoutID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                      UserID TEXT NOT NULL,
                      Date TEXT NOT NULL CHECK (length(Date) = 10 
                      AND Date GLOB '[0-9][0-9][0-9][0-9]-[0-1][0-9]-[0-3][0-9]'),
                      Completed INTEGER NOT NULL DEFAULT 0
                        CHECK (Completed IN (0, 1)),
                      WorkoutType TEXT NOT NULL,
                      FOREIGN KEY (UserID) REFERENCES User(UserID) 
                  );

                  CREATE TABLE IF NOT EXISTS WorkoutRepBasedSet (
                      WorkoutRepBasedSetID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                      WorkoutID INTEGER NOT NULL,
                      ExerciseID INTEGER NOT NULL,
                      TargetWeight REAL NOT NULL,
                      TargetReps INTEGER NOT NULL ,
                      ExerciseOrder INTEGER  NOT NULL ,
                      RestDuration INTEGER NOT NULL,
                      Completed INTEGER NOT NULL DEFAULT 0
                        CHECK (Completed IN (0, 1)),
                      FOREIGN KEY (WorkoutID) REFERENCES Workout(WorkoutID), 
                      FOREIGN KEY (ExerciseID) REFERENCES Exercise(ExerciseID)
                  );

                  CREATE TABLE IF NOT EXISTS WorkoutTimedExercise (
                      WorkoutTimedExerciseID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                      WorkoutID INTEGER NOT NULL,
                      ExerciseID INTEGER NOT NULL ,
                      TargetWeight REAL NOT NULL,
                      TargetSpeed REAL NOT NULL,
                      Duration INTEGER NOT NULL ,
                      ExerciseOrder INTEGER  NOT NULL ,
                      RestDuration INTEGER NOT NULL,
                      Completed INTEGER NOT NULL DEFAULT 0
                        CHECK (Completed IN (0, 1)),
                      FOREIGN KEY (WorkoutID) REFERENCES Workout(WorkoutID), 
                      FOREIGN KEY (ExerciseID) REFERENCES Exercise(ExerciseID) 
                  );
                

                  CREATE TABLE IF NOT EXISTS UserRepBasedSetTargets (
                      UserSetTargetsID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                      UserID TEXT NOT NULL,
                      ExerciseID INTEGER NOT NULL,
                      Reps INTEGER NOT NULL CHECK (Reps > 0),
                      Weight REAL NOT NULL CHECK (Weight >= 0),
                      [Order] INTEGER NOT NULL CHECK ([Order] > 0), -- Wrapped in brackets because ORDER is a reserved keyword
                      RestDuration INT NOT NULL CHECK (RestDuration > 0),
                      FOREIGN KEY (UserID) REFERENCES User(UserID), 
                      FOREIGN KEY (ExerciseID) REFERENCES Exercise(ExerciseID) 
                  );

                  CREATE TABLE IF NOT EXISTS UserTimedTargets (
                      UserTimedTargetsID INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                      UserID TEXT NOT NULL,
                      ExerciseID INTEGER NOT NULL,
                      Duration INTEGER NOT NULL CHECK (Duration > 0),
                      Weight REAL NOT NULL CHECK (Weight >= 0),
                      Speed REAL CHECK (Speed > 0),
                      RestDuration INT CHECK (RestDuration > 0),
                      FOREIGN KEY (UserID) REFERENCES User(UserID), 
                      FOREIGN KEY (ExerciseID) REFERENCES Exercise(ExerciseID) 
                  );";


                using (var command = new SqliteCommand(CreateTables, connection))
                {
                    command.ExecuteNonQuery();
                }
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "PRAGMA foreign_keys = ON;";
                    command.ExecuteNonQuery();
                }
            }
        }
        //UTILITY FUNCTIONS
        private DateTime StringToDate(object String)
        {
            return DateTime.ParseExact(String.ToString(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        }


        // GENERALISED DATABASE FUNCTIONS

        //Retrieves an element of data within a field for a record inside of a table
        private T GetFieldFromKey<T>(string table, string pkFieldName, string pk, string field)
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

                    if (result != null && result != DBNull.Value)
                    {
                        return (T)Convert.ChangeType(result, typeof(T));
                    }

                    return default(T);
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
                    WHERE @field1Name = @field1
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
            CustomLinkedList<string> cells = new CustomLinkedList<string>();

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
                            cells.Append(reader[table].ToString());
                        }
                    }
                }
                return cells.ToArray();
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
            return GetFieldFromKey<string>("Exercise", "ExerciseID", ExerciseID, "Name");
        }

        public ExerciseType GetExerciseType(string ExerciseID)
        {
            return (ExerciseType)Enum.Parse(typeof(ExerciseType), GetFieldFromKey<string>("Exercise", "ExerciseID", ExerciseID, "ExerciseType"));
        }

        public MuscleGroup GetExerciseTargetMuscle(string ExerciseID)
        {
            return (MuscleGroup)Enum.Parse(typeof(MuscleGroup), GetFieldFromKey<string>("Exercise", "ExerciseID", ExerciseID, "TargetMuscle"));
        }

        public string GetExerciseDetails(string ExerciseID)
        {
            return GetFieldFromKey<string>("Exercise", "ExerciseID", ExerciseID, "Details");
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
                    command.Parameters.AddWithValue("@Equipment", Equipment.ToString());
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
                INSERT INTO ExerciseAvailableEquipment
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

        // Consider making one of the paramaters a set object, rather than splitting it into reps, weight and order
        public void AddUserSetTarget(string UserID, int ExerciseID, int Reps, float Weight, int Order)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                INSERT INTO UserRepBasedSetTargets
                (UserID, ExerciseID, Reps, Weight, Order)
                VALUES
                (@UserID, @ExerciseID, @Reps, @Weight, @Order);";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@ExerciseID", ExerciseID);
                    command.Parameters.AddWithValue("@Reps", Reps);
                    command.Parameters.AddWithValue("@Weight", Weight);
                    command.Parameters.AddWithValue("@Order", Order);

                    command.ExecuteNonQuery();
                }
            }
        }

        // I have decided not to have an update_set function as this will get too complicated considering there will be
        // multiple sets (records) within the table. Instead, all the set targets can be removed for an exercise, and new
        // ones can be added through AddUserSetTarget
        public void RemoveUserSetTargets(string UserID, int ExerciseID, int Reps, float Weight, int Order)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                DELETE FROM UserRepBasedSetTarget WHERE UserID = @UserID AND ExerciseID = @Exercise";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@ExerciseID", ExerciseID);

                    command.ExecuteNonQuery();
                }
            }
        }

        public Set[] GetTargetsForUserSets(string UserID, string ExerciseID)
        {
            string PK = GetKeyFromTwoFields("UserRepBasedSetTargets", "UserRepBasedSetTargetsID", "UserID", UserID, "ExerciseID", ExerciseID);
            int[] repetitions = Array.ConvertAll(GetMultiValuesInFieldFromKey("UserRepBasedSetTargets", "UserRepBasedSetTargetsID", PK, "Reps"), int.Parse);
            float[] weight = Array.ConvertAll(GetMultiValuesInFieldFromKey("UserRepBasedSetTargets", "UserRepBasedSetTargetsID", PK, "Weight"), float.Parse);
            float[] restDurations = Array.ConvertAll(GetMultiValuesInFieldFromKey("UserRepBasedSetTargets", "UserRepBasedSetTargetsID", PK, "RestDuration"), float.Parse);

            Set[] Sets = new Set[repetitions.Length];
            for (int i = 0; i < repetitions.Length; i++)
            {
                Set set = new Set(weight[i], repetitions[i], restDurations[i]);
                Sets[i] = set;
            }
            return Sets;
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

        public void UpdateUserTimedTarget(string UserID, int ExerciseID, int Duration, float Weight, float Speed)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                UPDATE UserTimedTargets
                SET Duration = @Duration,
                    Weight = @Weight,
                    Speed = @Speed,
                WHERE UserID = @UserID AND ExerciseID = @ExerciseID)";

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

        public TimedExercise GetTargetsForUserTimedExercise(string UserID, string ExerciseID)
        {
            string PK = GetKeyFromTwoFields("UserTimedTargets", "UserTimedTargetsID", "UserID", UserID, "ExerciseID", ExerciseID);
            float weight = GetFieldFromKey<float>("UserTimedTargets", "UserTimedTargetsID", PK, "Weight");
            float speed = GetFieldFromKey<float>("UserTimedTargets", "UserTimedTargetsID", PK, "Speed");
            float duration = GetFieldFromKey<float>("UserTimedTargets", "UserTimedTargetsID", PK, "Duration");
            int RestDuration = GetFieldFromKey<int>("UserTimedTargets", "UserTimedTargetsID", PK, "RestDuration");

            string exerciseName = GetExerciseName(ExerciseID);
            string exerciseDescription = GetExerciseDetails(ExerciseID);
            MuscleGroup exerciseTargetMuscle = GetExerciseTargetMuscle(ExerciseID);
            ExerciseType exerciseType = GetExerciseType(ExerciseID);

            TimedExercise timedExercise = new TimedExercise(exerciseName, exerciseDescription, exerciseType, exerciseTargetMuscle,
                RestDuration, weight, speed, duration);

            return timedExercise;
        }

        // WORKOUT METHODS

        public void AddWorkout(string UserID, DateTime Date, bool Completed, WorkoutType workoutType)
        {
            string sqlDateText = Date.ToString("yyyy-MM-DD HH:mm:ss", CultureInfo.InvariantCulture);
            int Completed_Int = Completed ? 1 : 0;
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = @"

                INSERT INTO Workout
                (UserID, Date, Completed, WorkoutType)
                VALUES
                 (@UserID, @Date, @Completed, @workoutType);";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@UserID", UserID);
                    command.Parameters.AddWithValue("@Date", sqlDateText);
                    command.Parameters.AddWithValue("@Completed", Completed_Int);
                    command.Parameters.AddWithValue("@workoutType", workoutType);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void RemoveWorkout(Workout workout, User user)
        {
           string workoutDate = Convert.ToString(workout.Date);
           string userID = user.UserID;
           int WorkoutID = Convert.ToInt16(GetKeyFromTwoFields("Workout", "WorkoutID", "Date", workoutDate, "UserID", userID));

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                using (SqliteTransaction transaction = connection.BeginTransaction())
                {
                    string query = @"                      
                    DELETE FROM WorkoutRepBasedSet WHERE WorkoutID = @WorkoutID;

                    DELETE FROM WorkoutTimedExercise WHERE WorkoutID = @WorkoutID;
                
                    DELETE FROM Workout WHERE WorkoutID = @WorkoutID;
                    ";

                    using (SqliteCommand command = new SqliteCommand(query, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@WorkoutID", WorkoutID);
                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
            }
        }

        public DateTime? GetMostRecentTimedExerciseDate(string userID, Exercise exercise)
        {
            bool repBased = false;

            if (exercise is RepBasedExercise)
            {
                repBased = true;
            }

            using (SqliteConnection connection =
                new SqliteConnection(connectionString))
            {
                connection.Open();

                string query = @"
                SELECT MAX(w.Date)
                FROM @TableName AS t
                INNER JOIN Workout AS w
                ON t.WorkoutID = w.WorkoutID
                WHERE t.ExerciseID = @ExerciseID
                AND w.UserID = @UserID
                AND w.Completed = 1;";

                using (SqliteCommand command =
                    new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ExerciseID", exercise.ExerciseID);
                    command.Parameters.AddWithValue("@UserID", userID);
                    command.Parameters.AddWithValue("@TableName", repBased ? "UserRepBasedexercise" : "UserTimedExercise");

                    object result = command.ExecuteScalar();

                    if (result == null || result == DBNull.Value)
                    {
                        return null;
                    }

                    return StringToDate(result);
                }
            }
        }

        // USER-REP-BASED-SET CONJOINED WITH USER-TIMED-EXERCISE METHODS
        public DateTime? GetMostRecentDateOfWorkoutType(string userID, WorkoutType workoutType)
        {
            using (SqliteConnection connection =
                new SqliteConnection(connectionString))
            {
                connection.Open();

                string query = @"
                SELECT MAX(Date)
                FROM Workout
                WHERE WorkoutType = @WorkoutType
                AND UserID = @UserID
                AND Completed = 1;";

                using (SqliteCommand command =
                    new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@WorkoutType", workoutType.ToString());
                    command.Parameters.AddWithValue("@UserID", userID);

                    object result = command.ExecuteScalar();

                    if (result == null || result == DBNull.Value)
                    {
                        return null;
                    }

                    return StringToDate(result);
                }
            }
        }


        // USER-REP-BASED-SET METHODS
        public void AddRepBasedExerciseToWorkout(int workoutID, RepBasedExercise exercise, int exerciseOrder)
        {
            int ExerciseID = GetFieldFromKey<int>("Exercise", "Name", exercise.ExerciseName, "ExerciseID");
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // Ensures all sets are inserted together, as a transaction ensures atomicity
                using (SqliteTransaction transaction = connection.BeginTransaction())
                {
                    string query = @"
                    INSERT INTO WorkoutRepBasedSet
                    (WorkoutID, ExerciseID, TargetWeight, TargetReps, ExerciseOrder,  RestDuration, Completed) 
                    VALUES
                    (@WorkoutID, @ExerciseID, @TargetWeight, @TargetReps, @ExerciseOrder, @RestDuration, @Completed);";

                    using (SqliteCommand command =
                        new SqliteCommand(query, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@WorkoutID", workoutID);
                        command.Parameters.AddWithValue("@ExerciseID", ExerciseID);
                        command.Parameters.AddWithValue("@ExerciseOrder", exerciseOrder);

                        var weightParameter = command.Parameters.Add("@TargetWeight", SqliteType.Real);
                        var repsParameter = command.Parameters.Add("@TargetReps", SqliteType.Integer);
                        var restParameter = command.Parameters.Add("@RestDuration", SqliteType.Integer);
                        var completedParameter = command.Parameters.Add("@Completed", SqliteType.Integer);

                        foreach (Set set in exercise.Sets)
                        {
                            int Completed_Int = set.Completed ? 1 : 0;

                            weightParameter.Value = set.Weight;
                            repsParameter.Value = set.Reps;
                            restParameter.Value = set.RestDuration;
                            completedParameter.Value = Completed_Int;

                            command.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                }
            }
        }

        // USER-TIMED-EXERCISE METHODS
        public void AddTimedExerciseToWorkout(int workoutID, TimedExercise exercise, int exerciseOrder)
        {
            int Completed_Int = exercise.Completed ? 1 : 0;
            int ExerciseID = GetFieldFromKey<int>("Exercise", "Name", exercise.ExerciseName, "ExerciseID");
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = @"
                INSERT INTO WorkoutTimedExercise
                (WorkoutID, ExerciseID, TargetWeight, TargetSpeed, Duration, ExerciseOrder, RestDuration, Completed) 
                VALUES
                (@WorkoutID, @ExerciseID, @TargetWeight, @TargetSpeed, @Duration, @ExerciseOrder, @RestDuration, @Completed);";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@WorkoutID", workoutID);
                    command.Parameters.AddWithValue("@ExerciseID", ExerciseID);
                    command.Parameters.AddWithValue("@TargetWeight", exercise.Weight);
                    command.Parameters.AddWithValue("@TargetSpeed", exercise.Speed);
                    command.Parameters.AddWithValue("@Duration", exercise.Duration);
                    command.Parameters.AddWithValue("@ExerciseOrder", exerciseOrder);
                    command.Parameters.AddWithValue("@RestDuration", exercise.RestDuration);
                    command.Parameters.AddWithValue("@Completed", Completed_Int);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}


