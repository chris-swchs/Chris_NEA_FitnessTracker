using NEA.Models.Entities;
using NEA.Models.LinkedLists;
using NEA.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

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
            bool continueAlgorithm = true;

            UserSettings settings = user.GetSettings();

            CustomLinkedList<Exercise> selectedExercises = new CustomLinkedList<Exercise>();
            Exercise[] eligibileExercises = FindEligibleExercises(workoutType, settings);

            Dictionary<MuscleGroup, double> targetMuscleCoverage = CalculateTargetCoverage();
            Dictionary<MuscleGroup, double> muscleCoverages = InitialiseMuscleCoverages();

            Dictionary<Exercise, double> staticScores = CalculateStaticScores();
            Dictionary<Exercise, double> regenertionScores = previousWorkoutSchedule != null ? CalculateRegenerationScores() : null;

            double preferredSessionDuration = settings.PreferredSessionDuration;
            int totalDuration = 0;

            Workout[] previousWorkouts = previousWorkoutSchedule != null ? previousWorkoutSchedule.GetWorkouts() : null;

            while(continueAlgorithm & eligibileExercises.Length > 0)
            {
                continueAlgorithm = false;
                double highestScore = Double.NegativeInfinity;
                Exercise highestScoreExercise = null;

                foreach(Exercise exercise in eligibileExercises)
                {
                    if (totalDuration + exercise.GetEstimatedDuration() <= preferredSessionDuration)
                    {
                        continueAlgorithm = true;

                        double score = CalculateExerciseScore();
                        if (score > highestScore)
                        {
                            highestScore = score;
                            highestScoreExercise = exercise;
                        }
                    }
                }

                if (highestScoreExercise != null)
                {
                    int index = Array.IndexOf(eligibileExercises, highestScoreExercise);

                    selectedExercises.Append(highestScoreExercise);
                    eligibileExercises[index] = null;

                    muscleCoverages = UpdateMuscleCoverages();
                    totalDuration = totalDuration + highestScoreExercise.GetEstimatedDuration();
                }
            }

            return ExerciseLibrary;
        }

        public static Dictionary<Exercise, double>  CalculateStaticScores(Dictionary<Exercise, double> eligibleExercises, User user, UserSettings settings)
        {
            foreach (var ExerciseScorePair in eligibleExercises)
            {
                Exercise exercise = ExerciseScorePair.Key;

                Dictionary<Exercise, double> staticScores = new Dictionary<Exercise, double>();
                double PrimaryGoalCompatibility = CompatibilityDefinitions.ExerciseType_Goal.Get(exercise.ExerciseType, settings.PrimaryGoal);
                double SecondaryGoalCompatibility = CompatibilityDefinitions.ExerciseType_Goal.Get(exercise.ExerciseType, settings.SecondaryGoal);
                bool isCompound = exercise.isCompound();

                DateTime? lastExerciseDate = db.GetMostRecentExerciseDate(user.UserID, exercise);
                int? daysSinceLastUsed = lastExerciseDate.HasValue ? (ApplicationClock.GetCurrentDate() - lastExerciseDate.Value).Days : (int?)null;
                // P
                Func<bool, double, double> CompoundisolationInterpolationScore =
                (Compound, Value) => Compound
                    ? Value
                    : 1.0 - Value;
                //

                // H
                Func<int?, double> NoveltyScore = days =>
                       days.HasValue
                           ? 1.0 - Math.Exp(-0.1 * days.Value)
                           : 1.0;

                Func<int, double> ConsistencyScore = frequency =>
                    Math.Min(1.0, frequency / 4.0);


                Func<double, double, double, double> MotivationDisciplineScore =
                    (novelty, consistency, interpolation) =>
                        ((1 - interpolation) * novelty) + (interpolation * consistency);
                //


                Func<double, double, double, double, double> CalculateScore = (Gp, Gs, H, P) =>
                    (1.0 * Gp) + (1.0 * Gs) + (1.0 * H) + (1.0 * P);


                double score = CalculateScore(PrimaryGoalCompatibility, SecondaryGoalCompatibility,
                    MotivationDisciplineScore(NoveltyScore(daysSinceLastUsed), ConsistencyScore(1), settings.MotivationDisciplineInterpolation),
                    CompoundisolationInterpolationScore(isCompound, settings.CompoundIsolationInterpolation));

            }
           
        }

        public static double CalculateExerciseScore()
        {

        }
        public static Dictionary<Exercise, double> CalculateRegenerationScores()
        {

        }

        public static Dictionary<MuscleGroup, double> InitialiseMuscleCoverages()
        {

        }

        public static Dictionary<MuscleGroup, double> CalculateTargetCoverage()
        {

        }

        public static Dictionary<MuscleGroup, double> UpdateMuscleCoverages()
        {

        }

        public static Exercise[] FindEligibleExercises(WorkoutType workoutType, UserSettings settings)
        {
            CustomLinkedList<Exercise> eligibleExercises = new CustomLinkedList<Exercise>();

            foreach (Exercise exercise in ExerciseLibrary)
            {
                CustomLinkedList<MuscleGroup> WorkoutTypeMuscleGroups = new CustomLinkedList<MuscleGroup>();

                foreach (MainMuscleGroup mainMuscleGroup in EnumDefinitions.GetWorkoutTypeMainMuscleGroups(workoutType))
                {
                    foreach (MuscleGroup muscleGroup in EnumDefinitions.GetMuscleGroupsFromMain(mainMuscleGroup))
                    {
                        WorkoutTypeMuscleGroups.Append(muscleGroup);
                    }
                }

                Equipment[] userEquipment = settings.AvailableEquipment;
                bool equipmentEligible = db.GetExerciseAvailableEquipment(exercise.ExerciseName).All(equipment => userEquipment.Contains(equipment));
                bool muscleGroupEligible = WorkoutTypeMuscleGroups.Find(exercise.TargetMuscle);

                if (muscleGroupEligible & equipmentEligible)
                {
                    eligibleExercises.Append(exercise);
                }
            }
            return eligibleExercises.ToArray();
        }
    }
}
