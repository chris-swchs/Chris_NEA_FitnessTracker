using NEA.Models.Entities;
using System;

namespace NEA.Services
{
    public static class WorkoutTypeSelection  
    {
        public static void RunAlgorithm(UserSettings userSettings)
        {
            // for loop temporary - is meant to loop through each available workout slot
            for (int i = 0; i < 10; i++)
            {
                double[] Scores = new double[Enum.GetValues(typeof(WorkoutType)).Length];
                int WorkoutTypeIndex = 0;
                foreach (WorkoutType workoutType in Enum.GetValues(typeof(WorkoutType)))
                {
                     WorkoutTypeIndex = (int)workoutType;

                    double score = CalculateWorkoutTypeScore(workoutType, userSettings);
                    Scores[WorkoutTypeIndex] = score;
                    WorkoutTypeIndex++;
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

            double sum = MathOps.GetSum(PrimaryGoalCompatibility, SecondaryGoalCompatibility, MuscleGroupCompatility, CardioPreferenceCompatibility);
            return sum / 4;
        }
    }
}
