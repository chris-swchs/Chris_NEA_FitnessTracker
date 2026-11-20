using NEA.Models.Entities;
using NEA.Models.LinkedLists;
using NEA.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime;
using System.Windows.Forms;

namespace NEA.Services
{
    public static class ExerciseSelection
    {
        private static DatabaseManager db;
        private static readonly Random random = new Random();

        // Constants
        const double MinimumScoreForVariance = 2.0;
        const double totalMuscleCoverageScalar = 6.0;

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
            Workout[] previousWorkouts = previousWorkoutSchedule != null ? previousWorkoutSchedule.GetWorkouts() : null;

            CustomLinkedList<Exercise> selectedExercises = new CustomLinkedList<Exercise>();
            Exercise[] eligibileExercises = FindEligibleExercises(selectedMuscleGroups, settings);

            Dictionary<MuscleGroup, double> muscleCoverages = InitialiseMuscleCoverages(selectedMuscleGroups);
            Dictionary<MuscleGroup, double> targetMuscleCoverage = CalculateTargetCoverage(muscleCoverages, settings);
;
            Dictionary<Exercise, double> staticScores = CalculateStaticScores(eligibileExercises, user, settings);
            Dictionary<Exercise, double> regenerationScores = previousWorkoutSchedule != null 
                ? CalculateRegenerationScores(eligibileExercises,previousWorkouts , maximumVariance) : null;

            double preferredSessionDuration = settings.PreferredSessionDuration;
            double totalDuration = 0;

            while(continueAlgorithm & eligibileExercises.Length > 0)
            {
                continueAlgorithm = false;
                double highestScore = Double.NegativeInfinity;
                Exercise highestScoreExercise = null;

                foreach(Exercise exercise in eligibileExercises)
                {
                    if (exercise == null)
                        continue;

                    if (totalDuration + exercise.GetEstimatedDuration() <= preferredSessionDuration)
                    {
                        continueAlgorithm = true;

                        double score = CalculateExerciseScore(exercise, settings, selectedExercises.ToArray(), muscleCoverages, 
                            targetMuscleCoverage, staticScores, regenerationScores);
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

                    muscleCoverages = UpdateMuscleCoverages(muscleCoverages, highestScoreExercise);
                    totalDuration = totalDuration + highestScoreExercise.GetEstimatedDuration();
                } 
                else
                {
                    continueAlgorithm = false;
                }
            }

            return selectedExercises.ToArray();
        }

        public static Dictionary<Exercise, double> CalculateStaticScores(Exercise[] eligibleExercises, User user, UserSettings settings)
        {
            Dictionary<Exercise, double> staticScores = new Dictionary<Exercise, double>();

            foreach (Exercise exercise in eligibleExercises)
            {

                double PrimaryGoalCompatibility = CompatibilityDefinitions.ExerciseType_Goal.Get(exercise.ExerciseType, settings.PrimaryGoal);
                double SecondaryGoalCompatibility = CompatibilityDefinitions.ExerciseType_Goal.Get(exercise.ExerciseType, settings.SecondaryGoal);
                bool isCompound = exercise.isCompound();
                
                DateTime? lastExerciseDate = db.GetMostRecentExerciseDate(user.UserID, exercise);
                int? daysSinceLastUsed = lastExerciseDate.HasValue ? (ApplicationClock.GetCurrentDate() - lastExerciseDate.Value).Days : (int?)null;
                int completionCount = db.GetExerciseCompletionCount(user.UserID, exercise, ApplicationClock.GetCurrentDate().AddDays(-28));

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
                    MotivationDisciplineScore(NoveltyScore(daysSinceLastUsed), ConsistencyScore(completionCount), settings.MotivationDisciplineInterpolation),
                    CompoundisolationInterpolationScore(isCompound, settings.CompoundIsolationInterpolation));

                staticScores[exercise] = score;
            }

            return staticScores;
        }

        // Calculates the dynamic score, then adds it to the rest of the scores
        public static double CalculateExerciseScore(Exercise exercise, UserSettings usersettings, Exercise [] selectedExercises,
            Dictionary<MuscleGroup, double> muscleCoverages, Dictionary<MuscleGroup, double> targetMuscleCoverages,
            Dictionary<Exercise, double> staticScores, Dictionary<Exercise, double> regenerationScores)
        {
            double totalMuscleCoverageScore = 0.0;
            double totalOverCoveragePenalty = 0.0;

            double regenerationScore = regenerationScores != null? regenerationScores[exercise] : 0.0;
            double staticScore = staticScores[exercise];

            Dictionary<MuscleGroup, double> Contributions = new Dictionary<MuscleGroup, double>(exercise.SecondaryMuscleGroups);
            Contributions[exercise.TargetMuscle] = 1.0;

            Func<double, double, double> CalculateMuscleCoverageDeficit = (muscleCoverage, targetCoverage) =>
                   targetCoverage > 0 ? Math.Max(0.0, 1.0 - (muscleCoverage / targetCoverage)): 0.0;

            Func<double, double, double, double> CalculateMuscleCoverageScore = (targetCoverageShare, contribution, deficit) =>
                   targetCoverageShare * contribution * deficit;

            Func<double, double, double, double, double> CalculateOverCoveragePenalty = (muscleCoverage, contribution, targetCoverage, targetCoverageShare) =>
                targetCoverage > 0 ? Math.Max(0.0, targetCoverageShare*((muscleCoverage + contribution - targetCoverage)
                - Math.Max(0.0, muscleCoverage - targetCoverage))) 
                : 0.0;

            Func<MovementPattern, MovementPattern, double>
                CalculateMovementPatternSimilarity =
                    (patternA, patternB) =>
                        patternA == patternB ? 1.0 : 0.0;

            Func<double[], double[], double> CalculateContributionSimilarity = (A, B) =>
            {
                if (A.Length != B.Length)
                    throw new ArgumentException("Vectors must have the same length.");

                double dotProduct = 0.0;
                double magnitudeA = 0.0;
                double magnitudeB = 0.0;

                for (int i = 0; i < A.Length; i++)
                {
                    dotProduct += A[i] * B[i];
                    magnitudeA += Math.Pow(A[i], 2);
                    magnitudeB += Math.Pow(B[i], 2);
                }

                magnitudeA = Math.Sqrt(magnitudeA);
                magnitudeB = Math.Sqrt(magnitudeB);

                if (magnitudeA == 0 || magnitudeB == 0)
                    return 0.0;

                return dotProduct / (magnitudeA * magnitudeB);
            };

            Func<double, double, double> CalculateOverallSimilarity = (matchingMovementPattern, contributionSimilarity) =>
                  matchingMovementPattern * contributionSimilarity;

            Func<double, double, double, double> CalculateDynamicScore = (M, D, O) =>
               (1.0 * M) - (1.0 * D) - (1.0 * O);

            foreach (var muscleGroupContribution in Contributions)
            {

                MuscleGroup muscleGroup = muscleGroupContribution.Key;

                if (!targetMuscleCoverages.ContainsKey(muscleGroup))
                    continue;

                double contribution = muscleGroupContribution.Value;

                double targetCoverage = targetMuscleCoverages[muscleGroup];
                double targetCoverageShare = targetCoverage/totalMuscleCoverageScalar;
                double muscleCoverage = muscleCoverages[muscleGroup];

                totalMuscleCoverageScore += CalculateMuscleCoverageScore(targetCoverageShare, contribution,
                    CalculateMuscleCoverageDeficit(muscleCoverage, targetCoverage));
                totalOverCoveragePenalty += CalculateOverCoveragePenalty(muscleCoverage, contribution, targetCoverage, targetCoverageShare);
            }

            double highestSimilarity = 0.0;

            double[] currentVector = GetMuscleContributionVector(exercise);

            foreach (Exercise selectedExercise in selectedExercises)
            {
                double[] selectedVector =
                    GetMuscleContributionVector(selectedExercise);

                double similarity = CalculateOverallSimilarity(CalculateMovementPatternSimilarity(exercise.MovementPattern,selectedExercise.MovementPattern
                    ), CalculateContributionSimilarity(currentVector, selectedVector));

                highestSimilarity = Math.Max(highestSimilarity, similarity);
            }

            return CalculateDynamicScore(totalMuscleCoverageScore, highestSimilarity, totalOverCoveragePenalty) + regenerationScore + staticScore;
        }

        public static Dictionary<Exercise, double> CalculateRegenerationScores(Exercise[] eligibleExercises, Workout[] previousWorkouts , double maxVariation)
        {
            Dictionary<Exercise, double> regenerationScores = new Dictionary<Exercise, double>();

            foreach (var exercise in eligibleExercises)
            {

                int? dayProximity = CalculateExerciseProximity(exercise, previousWorkouts);


                Func<int?, double> CalculateProximityPenalty = days =>
                    days.HasValue
                        ? Math.Exp(-0.5 * days.Value)
                        : 0.0;

                Func<double, double, double> CalculateVariation =
                    (variation, randomValue) =>
                          (randomValue * 2.0 - 1.0) * variation;

                Func<double, double, double> CalculateScore = (V, R) =>
                    V - (1.0 * R);

                double score = CalculateScore(CalculateVariation(maxVariation, random.NextDouble()), CalculateProximityPenalty(dayProximity));

                regenerationScores[exercise] = score;
            }

            return regenerationScores;
        }

        public static Dictionary<MuscleGroup, double> InitialiseMuscleCoverages(MuscleGroup[] selectedMuscleGroups)
        {
            Dictionary<MuscleGroup, double> muscleGroupCoverages = new Dictionary<MuscleGroup, double>();

            foreach (var muscleGroup in selectedMuscleGroups)
            {
                muscleGroupCoverages[muscleGroup] = 0.0;
            }
            return muscleGroupCoverages;
        }

        public static Dictionary<MuscleGroup, double> CalculateTargetCoverage(Dictionary<MuscleGroup, double> muscleCoverages, UserSettings settings)
        {
            Dictionary<MuscleGroup, double> targetMuscleCoverages = new Dictionary<MuscleGroup, double>();
            Dictionary<MuscleGroup, double> muscleCoveragesWeightings = new Dictionary<MuscleGroup, double>();
            double totalWeight = 0.0;

            foreach (var muscleGroupCoverage in muscleCoverages)
            {
                double weight = EnumDefinitions.GetMainMuscleGroup(muscleGroupCoverage.Key) == settings.MainMuscleGroupPriority ? 1.25 : 1;
                totalWeight += weight;
                muscleCoveragesWeightings[muscleGroupCoverage.Key] = weight;
            }

            foreach (var muscleGroup in  muscleCoverages.Keys)
            {
                Func<double, double, double> CalculateTotalMuscleCoverage = (preferredSessionDuration, scalar) =>
                    preferredSessionDuration/scalar;

                Func<double, double, double> CalculatePercentageShare = (weighting, totalWeighting) =>
                    weighting / totalWeighting;

                Func<double, double, double> CalculateTargetCoverage = (TotalMuscleCoverage, PercentageShare) =>
                    TotalMuscleCoverage * PercentageShare;

                targetMuscleCoverages[muscleGroup] = CalculateTargetCoverage(CalculateTotalMuscleCoverage(settings.PreferredSessionDuration, totalMuscleCoverageScalar),
                    CalculatePercentageShare(muscleCoveragesWeightings[muscleGroup], totalWeight));
            }

            return targetMuscleCoverages;
        }

        public static Dictionary<MuscleGroup, double> UpdateMuscleCoverages(Dictionary<MuscleGroup, double> muscleCoverages,Exercise selectedExercise)
        {
            if (muscleCoverages.ContainsKey(selectedExercise.TargetMuscle))
            {
                muscleCoverages[selectedExercise.TargetMuscle] += 1.0;
            }

            foreach (var contribution in selectedExercise.SecondaryMuscleGroups)
            {
                if (muscleCoverages.ContainsKey(contribution.Key)
                    && contribution.Key != selectedExercise.TargetMuscle)
                {
                    muscleCoverages[contribution.Key] += contribution.Value;
                }
            }

            return muscleCoverages;
        }

        public static Exercise[] FindEligibleExercises(MuscleGroup[] selectedMuscleGroups, UserSettings settings)
        {
            CustomLinkedList<Exercise> eligibleExercises = new CustomLinkedList<Exercise>();

            foreach (Exercise exercise in ExerciseLibrary)
            {
                Equipment[] userEquipment = settings.AvailableEquipment;
                bool equipmentEligible = db.GetExerciseAvailableEquipment(exercise.ExerciseName).All(equipment => userEquipment.Contains(equipment));
                bool muscleGroupEligible = selectedMuscleGroups.Contains(exercise.TargetMuscle);

                if (muscleGroupEligible & equipmentEligible)
                {
                    eligibleExercises.Append(exercise);
                }
            }
            return eligibleExercises.ToArray();
        }

        // Make sure it also acounts for ensuring exercise is completed
        private static int CalculateExerciseProximity(Exercise currentExercise, Workout [] workouts)
        {
            if (workouts == null)
                return 100000;

            int? closestDays = null;
            DateTime currentDate = ApplicationClock.GetCurrentDate();

            foreach (var workout in workouts)
            {
                foreach (Exercise exercise in workout.Exercises.ToArray())
                {
                    if (currentExercise.ExerciseName != exercise.ExerciseName)
                        continue;

                    int days = Math.Abs((workout.Date.Date - currentDate.Date).Days);

                    if (!closestDays.HasValue || days < closestDays.Value)
                    {
                        closestDays = days;
                    }
                }
            }

            if (!closestDays.HasValue)
                return 100000;

            return closestDays.Value;
        }

        private static double[] GetMuscleContributionVector(Exercise exercise)
        {
            MuscleGroup[] muscleGroups =
                (MuscleGroup[])Enum.GetValues(typeof(MuscleGroup));

            double[] contributions = new double[muscleGroups.Length];

            for (int i = 0; i < muscleGroups.Length; i++)
            {
                MuscleGroup muscle = muscleGroups[i];

                if (muscle == exercise.TargetMuscle)
                {
                    contributions[i] = 1.0;
                }
                else if (exercise.SecondaryMuscleGroups.ContainsKey(muscle))
                {
                    contributions[i] = exercise.SecondaryMuscleGroups[muscle];
                }
            }

            return contributions;
        }
    }
}
