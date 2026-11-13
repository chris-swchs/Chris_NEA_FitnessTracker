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

            CustomLinkedList<string> list = new CustomLinkedList<string>("John", "James", "Harry");

            WorkoutScheduleGeneration.SendDatabase(DB);
            AcountManager.SendDatabase(DB);

            list.Append("5");
            list.Append("10");
            list.Prepend("20");


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

            // USER 2: Beginner focused on cardiovascular fitness
            User cardioUser = new User(
                "TEST002",
                "cardio@example.com",
                "TestPassword123",
                DateTime.Today
            );

            UserSettings cardioSettings = new UserSettings
            {
                PrimaryGoal = Goal.CardiovascularFitness,
                SecondaryGoal = Goal.GeneralFitness,
                Age = 22,
                ExperienceLevel = ExperienceLevel.Beginner,
                PreferredSessionDuration = 40,
                PreferredTrainingDays = 3,
                MotivationDisciplineInterpolation = 0.6f,
                PreferredTrainingIntensity = Frequency.Moderate,
                CompoundIsolationInterpolation = 0.7f,
                PreferredWorkoutDifficulty = Difficulty.Moderate,
                CardioPreference = Frequency.High,
                MainMuscleGroupPriority = MainMuscleGroup.Legs
            };

            cardioUser.UpdateSettings(cardioSettings);


            // USER 3: Intermediate focused on strength
            User strengthUser = new User(
                "TEST003",
                "strength@example.com",
                "TestPassword123",
                DateTime.Today
            );

            UserSettings strengthSettings = new UserSettings
            {
                PrimaryGoal = Goal.Strength,
                SecondaryGoal = Goal.MuscleGrowth,
                Age = 29,
                ExperienceLevel = ExperienceLevel.Intermediate,
                PreferredSessionDuration = 70,
                PreferredTrainingDays = 4,
                MotivationDisciplineInterpolation = 0.85f,
                PreferredTrainingIntensity = Frequency.High,
                CompoundIsolationInterpolation = 0.9f,
                PreferredWorkoutDifficulty = Difficulty.Hard,
                CardioPreference = Frequency.Low,
                MainMuscleGroupPriority = MainMuscleGroup.Back
            };

            strengthUser.UpdateSettings(strengthSettings);

            // Assign settings to user
            bodybuilder.UpdateSettings(bodybuilderSettings);

            Dictionary<DateTime, WorkoutType> workoutSlots = new Dictionary<DateTime, WorkoutType>
               {
                { new DateTime(2026, 11, 3), default(WorkoutType) },
                { new DateTime(2026, 11, 4), default(WorkoutType) },
                { new DateTime(2026, 11, 5), default(WorkoutType) },
                { new DateTime(2026, 11, 7), default(WorkoutType) },
                { new DateTime(2026, 11, 8), default(WorkoutType) },
                { new DateTime(2026, 11, 9), default(WorkoutType) }
               };

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new StartupForm());

            while (true)
            {
                WorkoutScheduleGeneration.GenerateWorkoutSchedule(strengthUser, workoutSlots);
            }
        }
    }
}
