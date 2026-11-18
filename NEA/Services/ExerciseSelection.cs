using NEA.Models.Entities;
using NEA.Models.LinkedLists;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NEA

namespace NEA.Services
{
    public static class ExerciseSelection
    {
        private static DatabaseManager db;
        private static readonly Random random = new Random();

        // Constants
        const double MinimumScoreForVariance = 2.0;

        // TO DO: Retrieve exercise library from database
        private static Exercise[] ExerciseLibrary = new Exercise[0];
        public static void SendDatabase(DatabaseManager DB)
        {
            db = DB;
        }
        public static Exercise [] RunAlgorithm(User user, WorkoutType workoutType, MuscleGroup[] selectedMuscleGroups, WorkoutSchedule previousWorkoutSchedule, double maximumVariance)
        {
            UserSettings settings = user.GetSettings();

            CustomLinkedList<Exercise> selectedExercises = new CustomLinkedList<Exercise>();
            Exercise[] eligibileExercises = FindEligibleExercises(workoutType, settings);

            double preferredSessionDuration = settings.PreferredSessionDuration;
            int totalDuration = 0;

            Workout[] previousWorkouts = previousWorkoutSchedule != null ? previousWorkoutSchedule.GetWorkouts() : null;
            return ExerciseLibrary;
        }

        public static Exercise[] FindEligibleExercises(WorkoutType workoutType, UserSettings settings)
        {
            CustomLinkedList<Exercise> eligibleExercises = new CustomLinkedList<Exercise>();

            foreach (Exercise exercise in ExerciseLibrary)
            {
                bool exerciseEligible = false;
                foreach (MuscleGroup muscleGroup in EnumDefinitions.GetWorkoutTypeMuscleGroups(workoutType))
                {
                    if (exercise.TargetMuscle == muscleGroup & !exerciseEligible)
                    {
                        eligibleExercises.Append(exercise);
                        exerciseEligible = true;
                    }
                }
            }
            return eligibleExercises.ToArray();
        }
    }
}
