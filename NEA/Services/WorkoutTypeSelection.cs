using NEA.Models.Entities;
using System;
using System.Windows.Forms;

namespace NEA.Services
{
    public static class WorkoutTypeSelection  
    {
        public static void RunAlgorithm(UserSettings userSettings)
        {
            // for loop temporary - is meant to loop through each available workout slot
            for (int i = 0; i < 4; i++)
            {
                double[] Scores = new double[Enum.GetValues(typeof(WorkoutType)).Length];
                double HighestScore = -1000;
                WorkoutType HighestScoreWorkoutType;
                foreach (WorkoutType workoutType in Enum.GetValues(typeof(WorkoutType)))
                {

                    double score = CalculateWorkoutTypeScore(workoutType, userSettings);
                    
                    if (score > HighestScore)

                    {
                        HighestScoreWorkoutType = workoutType;
                    }
                }

            }
        }
        private static double CalculateWorkoutTypeScore(WorkoutType workoutType, UserSettings userSettings)
        {
            // Takes into acount 4 settings, prmary goal, sconday goal, muscle priority
            double PrimaryGoalCompatibility = CompatibilityDefinitions.WorkoutType_Goal.Get(workoutType, userSettings.PrimaryGoal);
            double SecondaryGoalCompatibility = CompatibilityDefinitions.WorkoutType_Goal.Get(workoutType, userSettings.SecondaryGoal);
            double MuscleGroupCompatility = CompatibilityDefinitions.WorkoutType_MuscleGroup.Get(workoutType, userSettings.MainMuscleGroupPriority);
            double CardioPreferenceCompatibility = CompatibilityDefinitions.WorkoutType_CardioPreference.Get(workoutType, userSettings.CardioPreference);
            
            // Need a function which can check when the user last did a workout with a specific workout type
            int DaysSinceLastUsed = 0;

            Func<int?, double> CalculateRecenency = days =>
                days.HasValue
                    ? Math.Exp(-2 * days.Value)
                    : 0.0;

            Func<double, double, double, double, double, double> CalculateScore = (Gp, Gs, M, C, R) =>
                (4.0 * Gp) + (4.0 * Gp) + (3.0 * M) + (2.0 * C) - (4.0 * R);
                

            return CalculateScore(PrimaryGoalCompatibility, SecondaryGoalCompatibility, MuscleGroupCompatility, CardioPreferenceCompatibility, CalculateRecenency(DaysSinceLastUsed);
        }
    }
}
