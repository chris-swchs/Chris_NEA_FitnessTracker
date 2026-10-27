using NEA.Models.Entities;
using NEA.Models.LinkedLists;
using NEA.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NEA
{

     class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DatabaseManager DB = new DatabaseManager();
            DB.InitializeDatabase();

            LinkedList list = new LinkedList();

            WorkoutTypeSelection.SendDatabase(DB);

            list.Append("5");
            list.Append("10");
            list.Prepend("20");

            Console.WriteLine("hello");

            list.PrintList();

            //Adding all the exercises
            DB.AddExerciseRecord("flat Bench Press",
                ExerciseType.Strength,
                MuscleGroup.LowerChest,
                "Push the bar up and down while laying flat on a flat-adjusted bench");

            // Create test user
            User bodybuilder = new User(
                "TEST001",
                "bodybuilder@example.com",
                "TestPassword123",
                DateTime.Today
            );

            // Create bodybuilding-focused settings
            UserSettings bodybuilderSettings = new UserSettings
            {
                PrimaryGoal = Goal.MuscleGrowth,
                SecondaryGoal = Goal.Strength,

                Age = 25,
                ExperienceLevel = ExperienceLevel.Advance,

                PreferredSessionDuration = 75,
                PreferredTrainingDays = 5,

                MotivationDisciplineInterpolation = 0.8f,
                PreferredTrainingIntensity = Frequency.High,

                CompoundIsolationInterpolation = 0.5f,
                PreferredWorkoutDifficulty = Difficulty.Hard,

                CardioPreference = Frequency.Low,
                MainMuscleGroupPriority = MainMuscleGroup.Chest
            };

            // Assign settings to user
            bodybuilder.UpdateSettings(bodybuilderSettings);

            Dictionary<DateTime, WorkoutType> workoutSlots =
               new Dictionary<DateTime, WorkoutType>
               {
                { new DateTime(2026, 11, 3), default(WorkoutType) },
                { new DateTime(2026, 11, 5), default(WorkoutType) },
                { new DateTime(2026, 11, 7), default(WorkoutType) },
                { new DateTime(2026, 11, 8), default(WorkoutType) }
               };

            WorkoutTypeSelection.RunAlgorithm(bodybuilder, workoutSlots);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

        }
    }
}
