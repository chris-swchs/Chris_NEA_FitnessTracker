using NEA.Models.Entities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NEA.Services
{
    public static class WorkoutTypeSelection
    {
        private static DatabaseManager db;
        private static readonly Random random = new Random();

        // Constants
        const double MinimumScoreForVariance = 2.0;

        public static void SendDatabase(DatabaseManager DB)
        {
            db = DB;
        }
        public static Dictionary<DateTime, WorkoutType> RunAlgorithm(User user, Dictionary<DateTime, WorkoutType> WorkoutSlots, Dictionary<DateTime, WorkoutType> previousScheduleSlots, double maximumVariance)
        {
            // Dates of workouts have been defined, but the workout type assigned to the date has not
            Dictionary<DateTime, WorkoutType> assignedWorkouts = new Dictionary<DateTime, WorkoutType>();

            // Loops through each workout date
            foreach (var slot in WorkoutSlots)
            {
                Console.WriteLine(slot.Key);
                double HighestScore = Double.NegativeInfinity;
                // Cant be set to null - just incase (score > HighScore) condition is never met
                WorkoutType HighestScoreWorkoutType = WorkoutType.Push;
                foreach (WorkoutType workoutType in Enum.GetValues(typeof(WorkoutType)))
                {
                    double score = CalculateWorkoutTypeScore(workoutType, user, assignedWorkouts, previousScheduleSlots, slot, maximumVariance);
                    if (score > HighestScore)
                    {
                        HighestScoreWorkoutType = workoutType;
                        HighestScore = score;
                    }
                     Console.WriteLine($"{workoutType} {score}");
                }
                assignedWorkouts[slot.Key] = HighestScoreWorkoutType;
            }
            foreach (var workout in assignedWorkouts)
            {
                Console.WriteLine($"{workout.Key:yyyy-MM-dd}: {workout.Value}");
            }
            return assignedWorkouts;
        }


        private static double CalculatePreviousSchedulePenalty(WorkoutType workoutType, Dictionary<DateTime, WorkoutType> previousScheduleSlots)
        {
            if (previousScheduleSlots == null)
                return 0.0;

            int? closestDays = null;
            DateTime currentDate = DateTime.Now;

            foreach (var previousWorkout in previousScheduleSlots)
            {
                if (previousWorkout.Key.Date <= currentDate.Date)
                    continue;

                if (previousWorkout.Value != workoutType)
                    continue;

                int days = (previousWorkout.Key.Date - currentDate.Date).Days;

                if (!closestDays.HasValue || days < closestDays.Value)
                {
                    closestDays = days;
                }
            }

            if (!closestDays.HasValue)
                return 0.0;

            const double decayRate = 0.5;

            return Math.Exp(-decayRate * closestDays.Value);
        }

        private static double CalculateWorkoutTypeScore(WorkoutType workoutType, User user, 
            Dictionary<DateTime, WorkoutType> assignedWorkouts, Dictionary<DateTime, WorkoutType> previousScheduleSlots,
            KeyValuePair<DateTime, WorkoutType> currentSlot, double maximumVariance)
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
                    if (slot.Key > lastWorkoutDate || lastWorkoutDate.HasValue == false)
                    {
                        lastWorkoutDate = slot.Key;
                    }
                }
            }

            int? DaysSinceLastUsed = lastWorkoutDate.HasValue ? (currentSlot.Key - lastWorkoutDate.Value).Days : (int?)null;


            Func<int?, double> CalculateRecenency = days =>
                days.HasValue
                    ? Math.Exp(-0.5 * days.Value)
                    : 0.0;

            Func<double, double, double> CalculateVariation =
                (variation, randomValue) =>
                      (randomValue * 2.0 - 1.0) * variation;

            Func<double, double, double, double, double, double, double> CalculateScore = (Gp, Gs, M, C, R, P) =>
                (4.0 * Gp) + (4.0 * Gs) + (3.0 * M) + (2.0 * C) - (4.0 * R) - (3.0 * P);


            double score =  CalculateScore(PrimaryGoalCompatibility, SecondaryGoalCompatibility, MuscleGroupCompatility, CardioPreferenceCompatibility, CalculateRecenency(DaysSinceLastUsed), CalculatePreviousSchedulePenalty(workoutType,  previousScheduleSlots));
            return score > MinimumScoreForVariance? (score + CalculateVariation(maximumVariance, random.NextDouble())) : MinimumScoreForVariance;
        }
    }
}
