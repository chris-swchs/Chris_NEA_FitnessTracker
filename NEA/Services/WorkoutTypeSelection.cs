using NEA.Models.Entities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NEA.Services
{
    public static class WorkoutTypeSelection
    {
        private static DatabaseManager db;

        public static void SendDatabase(DatabaseManager DB)
        {
            db = DB;
        }
        public static void RunAlgorithm(User user, Dictionary<DateTime, WorkoutType> WorkoutSlots)
        {
            // Dates of workouts have been defined, but the workout type assigned to the date has not
            Dictionary<DateTime, WorkoutType> assignedWorkouts = WorkoutSlots;

            // Loops through each workout date
            foreach (var slot in assignedWorkouts)
            {
                double HighestScore = -1000;
                // Cant be set to null - just incase (score > HighScore) condition is never met
                WorkoutType HighestScoreWorkoutType = WorkoutType.Push;
                foreach (WorkoutType workoutType in Enum.GetValues(typeof(WorkoutType)))
                {
                    double score = CalculateWorkoutTypeScore(workoutType, user, assignedWorkouts);

                    if (score > HighestScore)
                    {
                        HighestScoreWorkoutType = workoutType;
                        HighestScore = score;
                    }
                }
                assignedWorkouts[slot.Key] = HighestScoreWorkoutType;
            }
            Console.WriteLine(assignedWorkouts);
        }
        private static double CalculateWorkoutTypeScore(WorkoutType workoutType, User user, Dictionary<DateTime, WorkoutType> assignedWorkouts)
        {
            UserSettings settings = user.GetSettings();
            // Takes into acount 4 settings, prmary goal, sconday goal, muscle priority
            double PrimaryGoalCompatibility = CompatibilityDefinitions.WorkoutType_Goal.Get(workoutType, settings.PrimaryGoal);
            double SecondaryGoalCompatibility = CompatibilityDefinitions.WorkoutType_Goal.Get(workoutType, settings.SecondaryGoal);
            double MuscleGroupCompatility = CompatibilityDefinitions.WorkoutType_MuscleGroup.Get(workoutType, settings.MainMuscleGroupPriority);
            double CardioPreferenceCompatibility = CompatibilityDefinitions.WorkoutType_CardioPreference.Get(workoutType, settings.CardioPreference);

            DateTime? lastWorkoutDate = db.GetMostRecentDateOfWorkoutType(user.UserID, workoutType);

            foreach (var slot in assignedWorkouts)
            {
                if (slot.Value == workoutType)
                {
                    if (slot.Key > lastWorkoutDate)
                    {
                        lastWorkoutDate = slot.Key;
                    }
                }
            }
            int? DaysSinceLastUsed = lastWorkoutDate.HasValue ? (DateTime.Today - lastWorkoutDate.Value).Days : (int?)null;

            Func<int?, double> CalculateRecenency = days =>
                days.HasValue
                    ? Math.Exp(-2 * days.Value)
                    : 0.0;

            Func<double, double, double, double, double, double> CalculateScore = (Gp, Gs, M, C, R) =>
                (4.0 * Gp) + (4.0 * Gs) + (3.0 * M) + (2.0 * C) - (4.0 * R);


            return CalculateScore(PrimaryGoalCompatibility, SecondaryGoalCompatibility, MuscleGroupCompatility, CardioPreferenceCompatibility, CalculateRecenency(DaysSinceLastUsed));
        }
    }
}
